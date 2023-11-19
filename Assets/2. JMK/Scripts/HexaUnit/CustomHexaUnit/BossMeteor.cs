using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossMeteor : MonoBehaviour
{
    public BossUnit ownerBoss;
    public Transform model;
    public TrailRenderer trail;

    public GameObject explosionFX;
    public GameObject indicatorTile;
    public Color indicatorColor1;
    public Color indicatorColor2;

    public Vector2Int destIndex;


    private Vector3 _startPos;
    private Vector3 _endPos;

    private bool _endMovement;

    public bool endMovement => _endMovement;

    public void Initialize(Vector2Int targetIndex)
    {
        trail.enabled = false;
        model.gameObject.SetActive(false);
        _endMovement = false;
        destIndex = targetIndex;
        _endPos = TilemapManager.instance.hexa_tilePosList[destIndex.y,destIndex.x];
        _startPos = _endPos + new Vector3(4f, 9f, 1.5f);
        transform.position = _startPos;
    }

    public void FallDown()
    {
        //코루틴
        StartCoroutine(FallDownAction());
    }

    IEnumerator FallDownAction()
    {
        var firstTargetTiles = new List<Vector2Int>();
        var secondTargetTiles = new List<Vector2Int>();

        firstTargetTiles = HexaUnitManager.instance.RangeOfHexaGridIndex(destIndex, 1);
        secondTargetTiles = HexaUnitManager.instance.RingOfHexaGridIndex(destIndex, 2);

        var indicatorTiles = new Dictionary<Vector2Int,GameObject>();

        foreach (var tile in firstTargetTiles)
        {
            var tileGO = Instantiate(indicatorTile);
            tileGO.transform.position = TilemapManager.instance.hexa_tilePosList[tile.y,tile.x];
            tileGO.transform.localScale = Vector3.one * 0.5f;
            tileGO.GetComponent<MeshRenderer>().material.color = indicatorColor1;
            indicatorTiles.Add(tile,tileGO);
        }
        foreach(var tile in secondTargetTiles)
        {
            var tileGO = Instantiate(indicatorTile);
            tileGO.transform.position = TilemapManager.instance.hexa_tilePosList[tile.y, tile.x];
            tileGO.transform.localScale = Vector3.one * 0.5f;
            tileGO.GetComponent<MeshRenderer>().material.color = indicatorColor2;
            indicatorTiles.Add(tile,tileGO);
        }

        var timer = 0f;

        while(timer < 0.8f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        var removeArray = indicatorTiles.Values.ToArray();
        for(int i = 0; i < removeArray.Length; i++)
        {
            Destroy(removeArray[i].gameObject);
        }

        //이동
        trail.enabled = true;
        model.gameObject.SetActive(true);
        while (timer < 1.2f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, _endPos, timer / 1.2f);
            yield return null;
        }
        timer = 0f;

        //타일 공격
        model.gameObject.SetActive(false);

        var copyList = HexaUnitManager.instance.unitList.ToList();

        //첫번째타일
        foreach (var unit in copyList)
        {
            if (unit != ownerBoss && firstTargetTiles.Contains(unit.tileIndex))
                unit.Damaged(30);
        }

        //폭발이펙트
        var firstExplosionGO = Instantiate(explosionFX);
        firstExplosionGO.transform.position = _endPos;
        firstExplosionGO.transform.localScale = Vector3.one * 2f;

        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //두번째타일
        foreach (var unit in copyList)
        {
            if (unit != ownerBoss && secondTargetTiles.Contains(unit.tileIndex))
                unit.Damaged(15);
        }

        //폭발이펙트
        foreach (var index in secondTargetTiles)
        {
            var secondExplosionGO = Instantiate(explosionFX);
            secondExplosionGO.transform.position = TilemapManager.instance.hexa_tilePosList[index.y,index.x];
        }

        trail.enabled = false;
        _endMovement = true;
        gameObject.SetActive(false);
    }
}
