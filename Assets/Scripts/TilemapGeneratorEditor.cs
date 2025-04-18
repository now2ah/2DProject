using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(TilemapGenerator))]
public class TilemapGeneratorEditor : Editor
{
    TilemapGenerator tilemapGenerator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        tilemapGenerator = (TilemapGenerator)target;

        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();

        if(GUILayout.Button("Generate Flat Terrain"))
        {
            tilemapGenerator.GenerateFlatTerrainTilemap();
        }

        if(GUILayout.Button("Generate Height Terrain"))
        {
            tilemapGenerator.GenerateHeightTerrainTilemap();
        }

        EditorGUILayout.EndVertical();
    }
}
