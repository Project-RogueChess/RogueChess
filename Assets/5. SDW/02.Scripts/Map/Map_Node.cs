using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Map_Node : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> prevNordKeys = new List<Vector2Int>();

    public List<Vector2Int> prevNordKeysProp => prevNordKeys;

    public Vector2Int mykey;

    public void AddPrevNodeKey(Vector2Int key)
    {
        bool alreadySameXY = prevNordKeys.Contains(key);

        if (alreadySameXY == false)
        {
            prevNordKeys.Add(key);
        }
    }

    public void SettingNord(int settValue)
    {

    }

    private void Start()
    {
        gameObject.GetComponent<Image>().color = new Color(160, 0, 0, 100f);
    }

}
