using UnityEngine;

[ExecuteInEditMode]
public class DrawWireDisc : MonoBehaviour
{
    private float discRadius = 0; // The wire disc radius

    public float DiscRadius { get { return discRadius; } }

    // Is called when the object is updated (in the editor)
    void OnRenderObject()
    {
        // If the prefabspawner exists on the object
        if (GetComponent<PrefabSpawner>() != null)
            // Update the wire disc radius based on the spawn radius
            discRadius = GetComponent<PrefabSpawner>().SpawnRadius;
        // Else, if the level bounds behaviour exists on the object
        else if (GetComponent<LevelBoundsBehaviour>() != null)
            // Update the wire disc radius based on the bounds radius
            discRadius = GetComponent<LevelBoundsBehaviour>().BoundsRadius;
    }
}
