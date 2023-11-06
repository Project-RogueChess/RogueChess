using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTile : MonoBehaviour
{

    public GameObject otherGO;

    DragObject dragObject;

    private void Start()
    {
        dragObject = GetComponent<DragObject>();
    }

    private void OnMouseOver()
    {
        if (dragObject.IsOnDrag)
        {
            DragObject.checkPosition = true;

            HighlightTile(this.gameObject, Color.red);
            DragObject.GO = this.gameObject;
        }
    }
    private void OnMouseExit()
    {
        DragObject.checkPosition = false;

        HighlightTile(this.gameObject, Color.white);
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
