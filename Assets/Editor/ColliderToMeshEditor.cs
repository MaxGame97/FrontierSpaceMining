using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ColliderToMesh))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColliderToMesh myScript = (ColliderToMesh)target;
        if (GUILayout.Button("Build Graphics"))
        {
            myScript.UpdateTexture();
        }
    }

}