using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapManager))]
public class TilemapManagerExtension : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TilemapManager tileSys = (TilemapManager)target;
        if(GUILayout.Button("타일맵 생성"))
        {
            if (!Application.isPlaying)
            {
                Debug.Log("에디터 상태로 타일맵을 생성했어요!, 반드시 플레이 버튼을 누르기 전에 [ 타일맵 제거 ] 버튼 눌러줘용");
            }
            tileSys.DestroyAllTilemaps(!Application.isPlaying);
            tileSys.CreateTilemaps();
        }

        if(GUILayout.Button("타일맵 제거"))
        {
            tileSys.DestroyAllTilemaps(!Application.isPlaying);
 
        
        
        
        
        
        
        
    }
    }
}
