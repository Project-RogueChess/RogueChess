using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Map_Node : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Vector2Int> prevNodeKeys = new List<Vector2Int>();
    public List<Vector2Int> prevNodeKeysProp => prevNodeKeys;
    public currentNodeTypeEnum myNodeType;

    public Vector2Int mykey;

    private Main_Map main_map;

    private RectTransform panelRectTransform;

    private float panelRectSizePercent;
    private Vector2 basicPanelSize;
    

    RectTransform panelRect;
    private bool isAccentNode;

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
        main_map = gameObject.GetComponentInParent<Main_Map>();
        panelRectTransform = gameObject.GetComponent<RectTransform>();
        basicPanelSize = panelRectTransform.sizeDelta;
    }

    public void ChangeImageSprite(Sprite sprite)
    {
        gameObject.GetComponent<Image>().sprite = sprite;
    }

    public void ChangeNodeRectSizeUp(float panelSize)
    {
        panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x + (panelRectTransform.sizeDelta.x/100* panelSize), panelRectTransform.sizeDelta.y + (panelRectTransform.sizeDelta.y / 100 * panelSize));
    }
    public void ChangeNodeRectSizeDown()
    {
        panelRectTransform.sizeDelta = basicPanelSize;
    }

    public bool IsContainPrevNode(Vector2Int key)
    {
        if (prevNodeKeys.Contains(key) == false)
        {
            Debug.Log("FALSE");
            return false;
        }

        else if(prevNodeKeys.Contains(key) == true)
        {
            Debug.Log("TRUE");
            return true;
        }

        Debug.Log("IsContainError");
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("OnClick");
            Debug.Log(mykey);

            bool isCanMove;

            isCanMove = IsContainPrevNode(main_map.currentNodeXY);

            if (isCanMove == true)
            {
                main_map.ClickedTrueNodeAndMove(mykey);
            }
            else
            {

            }
        }
    }
}

