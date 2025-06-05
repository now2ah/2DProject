using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using twoDProject.Dungeon;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    DungeonGenerator dungeonGenerator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        dungeonGenerator = (DungeonGenerator)target;

        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Dungeon"))
        {
            dungeonGenerator.GenerateDungeon();
        }

        EditorGUILayout.EndVertical();
    }
}
