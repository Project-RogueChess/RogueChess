using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Map_Node : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> prevNodeKeys = new List<Vector2Int>();
    public List<Vector2Int> prevNodeKeysProp => prevNodeKeys;

    public Vector2Int mykey;

    RectTransform panelRect;
    public enum currentNodeTypeEnum
    {
        None,
        NormalMonster,
        EliteMonster,
        Store,
        Treasure,
        RandomEvent,
        Shelter
    }

    public currentNodeTypeEnum myNodeType;

    public void AddPrevNodeKey(Vector2Int key)
    {
        bool alreadySameXY = prevNodeKeys.Contains(key);

        if (alreadySameXY == false)
        {
            prevNodeKeys.Add(key);
        }
    }

    private void Start()
    {
        myNodeType = currentNodeTypeEnum.None;
        gameObject.GetComponent<Image>().color = new Color(160, 0, 0, 100f);
        panelRect = gameObject.GetComponent<RectTransform>();
    }

    public void ChangeImageSprite(Sprite sprite)
    {
        gameObject.GetComponent<Image>().sprite = sprite;
        //panelRect.sizeDelta = new Vector2((panelRect.sizeDelta.x / 100 * 50 + panelRect.sizeDelta.x), (panelRect.sizeDelta.y / 100 * 50 + panelRect.sizeDelta.y));
    }




}

