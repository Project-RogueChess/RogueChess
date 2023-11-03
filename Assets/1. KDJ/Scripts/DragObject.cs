using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour
{

    public static bool isOnDrag;
    public static GameObject GO;

    [SerializeField]
    [Range(0, 1)]
    float dragSpeed;

    //private GameObject draggedObject = null;
    private Camera cam;

    private Map map;

    public static bool checkPosition;

    Vector3 backObject;

    RaycastHit hit;

    #region 테스트용(추후삭제 or 재사용)
    public Color highlightColor = Color.red;

    

    #endregion
    
    private void Start()
    {
        GO = this.gameObject;
        map = GameObject.Find("Scripts").GetComponent<Map>();


        
    }
    private void FixedUpdate()
    {
       
       if (isOnDrag)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 100.0f;
            if(map.m_Plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 p = new Vector3(hitPoint.x, 1.0f, hitPoint.z);

                this.transform.position = Vector3.Lerp(this.transform.position, p, dragSpeed);
            }

            /*transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, - Camera.main.transform.position.z));*/
            //5.0f -Camera.main.transform.position.z
            //y값 좌표 고정
            //transform.position = new Vector3(transform.position.x, 1f, transform.position.z);

            // 오브젝트 드래그시 알파값 적용 -> 반투명
            Material mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.6f);
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

        if (checkPosition)
        {
            GameObject aGO = GO.GetComponent<Tile>().otherGO;
            if(aGO = null)
            {
                

                aGO = this.gameObject;

                transform.position = GO.transform.position;

                Material mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);
            }
            else
            {
                aGO.transform.position = backObject;

                transform.position = GO.transform.position;

                aGO = this.gameObject;
            }
            
        }
        else
        {
            transform.position = backObject;
        }

        
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

/*    void HighlightTile(GameObject tile, Color color)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            Material originalMaterial = tileRenderer.material;

            Material highlightMaterial = new Material(originalMaterial);
            highlightMaterial.color = color;

            tileRenderer.material = highlightMaterial;
        }
    }*/

}
