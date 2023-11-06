using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



//일단 상점 구현 할 때 써야할 필요 기물 정보

public class Pieces : MonoBehaviour
{
    public Sprite piecesImg;
    public string name;
    public string synergy;
    public int gold;


    public Transform pos;
    public int items;

    private void Awake()
    {
        pos = GetComponent<Transform>();
        items = 0;
    }

    
    public void EquipItem(ItemObject item)
    {
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero; // 장착한 아이템의 위치를 유닛의 중심으로 설정
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && items < 3)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            Debug.Log(transform);
            eventData.pointerDrag.GetComponent<Transform>().position = pos.position;
            items++;
        }
    }




    //public int Gold { get { return gold; } set {  gold = value; } }
    //public string Name { get { return name; } set { name = value; } }
    //public string Synergy { get {  return synergy; } set {  synergy = value; } }
    //public Sprite PiecesImg { get { return piecesImg; } set {  piecesImg = value; } }

    //public Pieces(Sprite PiecesImg,string Name,string Synergy, int Gold )
    //{
    //    this.name = Name;
    //    this.synergy = Synergy;
    //    this.gold = Gold;
    //    this.piecesImg = PiecesImg;
    //}
}
