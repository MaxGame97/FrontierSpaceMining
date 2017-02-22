using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrawWireDisc))]
public class DrawWireDiscEditor : Editor
{
    void OnSceneGUI()
    {
        // Gets the target object
        DrawWireDisc t = target as DrawWireDisc;
        
        // Sets the color to white with a slight transparency
        Handles.color = new Color(1, 1, 1, 0.8f);
        
        // Draw a wire disc on the target's position, facing backwards
        Handles.DrawWireDisc(t.transform.position, Vector3.back, t.DiscRadius);

        // Reset the color
        Handles.color = Color.white;
    }
}