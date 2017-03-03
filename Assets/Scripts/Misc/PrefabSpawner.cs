using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PrefabSpawner : MonoBehaviour {
    
    [SerializeField] private GameObject prefab;                         // The prefab to spawn
    [SerializeField] [Range(1, 500)] private int amount;                // The amount of prefabs to spawn
    [SerializeField] [Range(0.1f, 10f)] private float sizeFactorMin;    // The min size factor of the spawned prefabs
    [SerializeField] [Range(0.1f, 10f)] private float sizeFactorMax;    // The max size factor of the spawned prefabs
    [SerializeField] [Range(10f, 250f)] private float spawnRadius;      // The spawning radius

    [Space(6f)]

    [SerializeField] private bool cleanup = true;                       // Specifies if the spawned objects should be cleaned up or not

    private List<GameObject> spawnedObjects = new List<GameObject>();   // Contains the spawned objects

    public float SpawnRadius { get { return spawnRadius; } }
    public List<GameObject> SpawnedObjects { get { return spawnedObjects; } }

    // Use this for initialization
    void Start()
    {
        // Get the spawned objects (the list will be cleared when deselected, this resets the list)
        GetSpawnedObjects();

        // If no objects have been spawned, spawn them
        if (spawnedObjects.Count == 0)
            SpawnObjects();
    }

    // Spawns the objects
    public void SpawnObjects()
    {
        // Get the spawned objects
        GetSpawnedObjects();

        // If there are already spawned objets
        if (spawnedObjects.Count != 0)
        {
            // Destroy all of the previously spawned objects
            for(int i = 0; i < spawnedObjects.Count; i++)
            {
                DestroyImmediate(spawnedObjects[i], true);
            }

            // Clear the spawned objects list
            spawnedObjects.Clear();
        }

        // If the size factor variables are reversed
        if(sizeFactorMax < sizeFactorMin)
        {
            // Reverse them to the correct order

            float tempSizeFactorMax = sizeFactorMax;

            sizeFactorMax = sizeFactorMin;
            sizeFactorMin = tempSizeFactorMax;
        }

        // Loop for the amount of objects that should be spawned
        for (int i = 0; i < amount; i++)
        {
            // Instantiate a new prefab
            GameObject spawnedObject = Instantiate(prefab);

            // Calculate a random spawn position within the radius
            Vector2 localSpawnPosition = Random.insideUnitCircle * spawnRadius;

            // Place it on this position (relative to the spawner)
            spawnedObject.transform.position = new Vector3(localSpawnPosition.x + transform.position.x, localSpawnPosition.y + transform.position.y, transform.position.z);

            // Rotate the object randomly within 360 degrees
            spawnedObject.transform.Rotate(0f, 0f, Random.Range(0f, 360f));

            // Get a random scale factor between the min and max scale factors
            float scaleFactor = Random.Range(sizeFactorMin, sizeFactorMax);

            // Multiply the object's current scale with the randomized scale factor
            spawnedObject.transform.localScale *= scaleFactor;

            // Parent the spawned object to this prefab spawner
            spawnedObject.transform.SetParent(transform);

            // If the spawned object has a rigidbody
            if(spawnedObject.GetComponent<Rigidbody2D>() != null)
            {
                // Get the spawned object's rigidbody
                Rigidbody2D spawnedObjectRigidbody = spawnedObject.GetComponent<Rigidbody2D>();

                // Scale the mass with the randomized scale factor
                spawnedObjectRigidbody.mass *= Mathf.Pow(scaleFactor, 2);
            }
        }

        // If the spawned objects should be cleaned up, call the clean up funtions
        if(cleanup)
            CleanUpObjects();
    }

    // Cleans up the spawned objects (APPROXIMATE)
    public void CleanUpObjects()
    {
        // Get the spawned objects
        GetSpawnedObjects();

        // Compare all spawned objects
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            for (int j = i + 1; j < spawnedObjects.Count; j++)
            {
                // If two objects intersects
                if (spawnedObjects[i].GetComponent<Collider2D>().bounds.Intersects(spawnedObjects[j].GetComponent<Collider2D>().bounds))
                {
                    // Destroy one of the intersecting objects
                    DestroyImmediate(spawnedObjects[i]);
                    break;
                }
            }
        }
    }

    // Gets the spawned objects (if any)
    void GetSpawnedObjects()
    {
        // Clear the spawned objects list (should it be neccesary)
        spawnedObjects.Clear();

        // Add all of the child GameObjects to the spawned objects list
        for(int i = 0; i < transform.childCount; i++)
        {
            spawnedObjects.Add(transform.GetChild(i).gameObject);
        }
    }
}
