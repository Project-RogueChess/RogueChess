using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour
{

    public static bool isOnDrag;
    public static GameObject GO;
    private GameObject draggedObject = null;

    Vector3 backObject;

    public Color highlightColor = Color.red;

    RaycastHit hit;
    private void Start()
    {
        GO = this.gameObject;
    }
    private void Update()
    {
       if(isOnDrag)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, 3.0f-Camera.main.transform.position.z));

            Material mat = gameObject.GetComponent<MeshRenderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.6f);
            ///*transform.position = new Vector3(newPosition.x, 3f, newPosition.z); // y 축을 3으로 고정*/

            //if (Physics.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            //    Input.mousePosition.y, -Camera.main.transform.position.z)), Camera.main.transform.localRotation * Vector3.forward, out hit, 10f))
            //{

            //    HighlightTile(hit.transform.gameObject, highlightColor);

            //}
            
            
        }

    }

    public void OnMouseDown()
    {
        isOnDrag = true;

        backObject = transform.position;

        GetComponent<CapsuleCollider>().enabled = false;       
    }

    public void OnMouseUp()
    {
        isOnDrag = false;
        
        GetComponent<CapsuleCollider>().enabled = true;

        transform.position = GO.transform.position;

        Material mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);

        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10f))
        //{

        //    transform.position = hit.transform.position;
        //    if (hit.collider.CompareTag("Tile"))
        //    {

        //        Debug.Log("Moved to tile");
        //        //HighlightTile(hit.transform.gameObject, highlightColor);
        //    }
        //    else
        //    {
        //        transform.position = backObject;
        //        Debug.Log("Not on a Tile");
        //    }

        //}
        //else
        //{
        //    transform.position = backObject;
        //    Debug.Log("Not on a Tile");

        //}
    }

    void HighlightTile(GameObject tile, Color color)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            Material originalMaterial = tileRenderer.material;

            Material highlightMaterial = new Material(originalMaterial);
            highlightMaterial.color = color;

            tileRenderer.material = highlightMaterial;
        }
    }

}
