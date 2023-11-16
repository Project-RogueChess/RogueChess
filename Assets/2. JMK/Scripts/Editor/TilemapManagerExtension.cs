using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapManager))]
public class TilemapManagerExtension : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TilemapManager tileSys = (TilemapManager)target;
        if(GUILayout.Button("鸥老甘 积己"))
        {
            tileSys.DestroyAllTilemaps(!Application.isPlaying);
            tileSys.CreateTilemaps();
        }

        if(GUILayout.Button("鸥老甘 力芭"))
        {
            tileSys.DestroyAllTilemaps(!Application.isPlaying);
        }
    }
}
