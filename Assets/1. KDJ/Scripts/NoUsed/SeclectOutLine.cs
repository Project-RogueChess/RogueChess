using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SeclectOutLine : MonoBehaviour
{
    Material outline;

    Renderer renderer;
    List<Material> materialList = new List<Material>();

    private void OnMouseDown()
    {
       renderer = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderer.sharedMaterials);
        materialList.Add(outline);

        renderer.materials = materialList.ToArray();
    }

    private void OnMouseUp()
    {
        Renderer renderer = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderer.sharedMaterials);
        materialList.Remove(outline);

        renderer.materials = materialList.ToArray();
    }

    private void Start()
    {
        //outline = new Material(Shader.Find("Custom/Outline"));
    }
}
