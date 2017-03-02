using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PrefabSpawner))]
class PrefabSpawnerButton : Editor
{
    public override void OnInspectorGUI()
    {
        PrefabSpawner t = target as PrefabSpawner;

        DrawDefaultInspector();

        GUILayout.Space(6f);

        if (GUILayout.Button("Generate"))
            t.SpawnObjects();
    }
}
