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
        else if (GetComponent<LevelBoundsBehaviour>() != null)
            discRadius = GetComponent<LevelBoundsBehaviour>().BoundsRadius;
        else
            Destroy(this);
    }
}
