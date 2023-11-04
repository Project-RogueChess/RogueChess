using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexaGridTileManager))]
public class HexaGridExtension : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexaGridTileManager manager = (HexaGridTileManager)target;
        if(GUILayout.Button("타일맵 생성"))
        {
            if (!Application.isPlaying)
            {
                Debug.Log("에디터 상태로 타일맵을 생성했어요!, 반드시 플레이 버튼을 누르기 전에 [ 타일맵 제거 ] 버튼 눌러줘용");
            }
            manager.DestroyAllTiles(!Application.isPlaying);
            manager.GenerateTile(manager.mapX, manager.mapY, manager.spaceX, manager.spaceY, manager.gridCenter.position);
        }

        if(GUILayout.Button("타일맵 제거"))
        {
            manager.DestroyAllTiles(!Application.isPlaying);
        }
    }
}
