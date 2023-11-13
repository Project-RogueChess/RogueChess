using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    public  GameObject m_goPrefab = null;

    //public Transform[] m_objectList = new Transform[20];
    //public List<GameObject> m_hpBarsList = new List<GameObject>();
    public GameObject[] m_hpBarsList;
    Camera camera = null;
    public GameObject[] t_objects;
    //public List<GameObject> test;
    public GameObject t_HpBar;
    public GameObject hpBars;


    public GameObject[] m_ItemsList;

    public void Awake()
    {
        t_objects = new GameObject[20];
        for (int i = 0; i < t_objects.Length; i++)
        {
            t_objects[i] = hpBars;
        }
    }
    void Start()
    {
        camera = Camera.main;
        //t_objects = GameObject.FindGameObjectsWithTag("Player");

        //for(int i = 0; i < t_objects.Length; i++)
        //{
        //    m_objectList.Add(t_objects[i].transform);
        //    GameObject t_HpBar = Instantiate(m_goPrefab, t_objects[i].transform.position,Quaternion.identity,transform);
        //    m_hpBarsList.Add(t_HpBar);
        //}
        //for(int i =0;i < test.Count; i++)
        //{
        //    m_hpBarsList.Add(t_HpBar);
        //}

       
    }
    void Update()
    {
        //for (int i = 0; i < t_objects.Length; i++)
        //{
        //    m_hpBarsList[i].transform.position = camera.WorldToScreenPoint(t_objects[i].transform.position + new Vector3(0, 1.15f, 1.0f));



        //    if (t_objects[i].GetComponent<Pieces>())
        //    {
        //        m_ItemsList[i * 3 + 0].transform.position = camera.WorldToScreenPoint(t_objects[i].transform.position + new Vector3(-0.4f, 0.9f, 0.7f));
        //        m_ItemsList[i * 3 + 1].transform.position = camera.WorldToScreenPoint(t_objects[i].transform.position + new Vector3(0f, 0.9f, 0.7f));
        //        m_ItemsList[i * 3 + 2].transform.position = camera.WorldToScreenPoint(t_objects[i].transform.position + new Vector3(0.4f, 0.9f, 0.7f));
        //    }
        //    else
        //    {
        //        m_ItemsList[i * 3 + 0].transform.position = camera.WorldToScreenPoint(t_objects[i].transform.position + new Vector3(-0.4f, 0.9f, 0.7f));
        //        m_ItemsList[i * 3 + 1].transform.position = camera.WorldToScreenPoint(t_objects[i].transform.position + new Vector3(0f, 0.9f, 0.7f));
        //        m_ItemsList[i * 3 + 2].transform.position = camera.WorldToScreenPoint(t_objects[i].transform.position + new Vector3(0.4f, 0.9f, 0.7f));
        //    }
        //}
    }
}
