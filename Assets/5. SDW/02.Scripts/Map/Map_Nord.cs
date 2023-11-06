using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Map_Nord : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> prevNordKeys = new List<Vector2Int>();

    public Vector2Int mykey;

    public void AddPrevNordKey(Vector2Int key)
    {
        prevNordKeys.Add(key);
    }

    public void SettingNord(int settValue)
    {

    }

    private void Start()
    {
        gameObject.GetComponent<Image>().color = new Color(160, 0, 0, 100f);
    }

}
