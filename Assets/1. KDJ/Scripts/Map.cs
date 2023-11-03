using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public static int hexMapSizeX = 8;
    public static int hexMapSizeZ = 8;
    public static int unitInventorySize = 9;

    public Transform ownInventoryStartposition;
    public Transform mapStartPosition;

    public Plane m_Plane;

    public GameObject squareIndicator;
    public GameObject hexaIndicator;

    // Start is called before the first frame update
    void Start()
    {
        //CreateGridPostiion();
        /*CreateIndicators();
        HideIndicators();*/

        m_Plane = new Plane(Vector3.up, Vector3.zero);
    }

    [HideInInspector]
    public Vector3[] ownInventoryStartpositions;
    [HideInInspector]
    public Vector3[,] mapGridPositions;

    /*private void CreateGridPostiion()
    {
        ownInventoryStartpositions = new Vector3[unitInventorySize];
        //mapGridPositions = new Vector3[hexMapSizeX];
    }*/

/*    private void CreateIndicators()
    {
        throw new NotImplementedException();
    }

    private void HideIndicators()
    {
        throw new NotImplementedException();
    }*/





    // Update is called once per frame
    void Update()
    {
        
    }
}
