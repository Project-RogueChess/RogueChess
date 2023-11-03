using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public GameObject otherGO;



    private void OnMouseOver()
    {
        if(DragObject.isOnDrag)
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
