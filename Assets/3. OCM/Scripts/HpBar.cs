using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    public  GameObject m_goPrefab = null;

    //public List<Transform>m_objectList = new List<Transform>();
    //public List<GameObject> m_hpBarsList = new List<GameObject>();

    //Camera camera = null;

    public GameObject[] t_objects;
    //public List<GameObject> test;


    public GameObject t_HpBar;
    // Start is called before the first frame update
    //void Start()
    //{
        //camera = Camera.main;
       // t_objects = GameObject.FindGameObjectsWithTag("Player");

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
    //}

    // Update is called once per frame
    //void Update()
    //{ 
    //    for(int i = 0; i < test.Count; i++)
    //    {
    //        m_hpBarsList[i].transform.position =camera.WorldToScreenPoint(test[i].transform.position + new Vector3(0,1.15f,0.5f));
    //    }
    //}
}
