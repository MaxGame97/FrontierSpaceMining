using UnityEngine;

[ExecuteInEditMode]
public class DrawWireDisc : MonoBehaviour
{
    private float discRadius = 0; // The wire disc radius

    public float DiscRadius { get { return discRadius; } }

    // Is called when the object is updated (in the editor)
    void Update()
    {
        // If the prefabspawner exists on the object
        if (GetComponent<PrefabSpawner>() != null)
            // Update the wire disc radius based on the spawn radius
            discRadius = GetComponent<PrefabSpawner>().SpawnRadius;
        else
            Destroy(this);
    }
}
