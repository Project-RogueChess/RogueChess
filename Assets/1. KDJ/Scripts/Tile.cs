using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private void OnMouseOver()
    {
        if(DragObject.isOnDrag)
        {
            HighlightTile(this.gameObject, Color.red);
            DragObject.GO = this.gameObject;
        }
    }
    private void OnMouseExit()
    {
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
