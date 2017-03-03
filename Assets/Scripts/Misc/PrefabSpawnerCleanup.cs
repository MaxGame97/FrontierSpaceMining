using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabSpawnerCleanup : MonoBehaviour {

    // List containing all of the objects spawned by prefab spawners
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Update is called once per frame
	void Update () {
        // Find all prefab spawners and add them to an array
        GameObject[] prefabSpawnersArray = GameObject.FindGameObjectsWithTag("Prefab Spawner");

        // Check all the prefab spawners
        for(int i = 0; i < prefabSpawnersArray.Length; i++)
        {
            // Get the prefab spawner component of the currently checked prefab spawner
            PrefabSpawner currentPrefabSpawner = prefabSpawnersArray[i].GetComponent<PrefabSpawner>();

            // Add all spawned objects to the spawned objects list
            for(int j = 0; j < currentPrefabSpawner.SpawnedObjects.Count; j++)
            {
                spawnedObjects.Add(currentPrefabSpawner.SpawnedObjects[j]);
            }

            // Destroy the curren prefab spawner (no longer needed)
            Destroy(prefabSpawnersArray[i].GetComponent<PrefabSpawner>());
        }

        // Compare all the spawned objects
        for(int i = 0; i < spawnedObjects.Count; i++)
        {
            for(int j = i + 1; j < spawnedObjects.Count; j++)
            {
                // If two of the spawned objects's colliders intersect
                if(Physics2D.IsTouching(spawnedObjects[i].GetComponent<Collider2D>(), spawnedObjects[j].GetComponent<Collider2D>()))
                {
                    // Destroy one of the intersecting objects
                    Destroy(spawnedObjects[i]);
                    break;
                }
            }
        }

        // Destroy the prefab spawner clean up script
        Destroy(this);
	}
}
