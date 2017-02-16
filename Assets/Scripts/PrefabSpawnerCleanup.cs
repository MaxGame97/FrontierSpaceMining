using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabSpawnerCleanup : MonoBehaviour {

    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Update is called once per frame
	void Update () {
        GameObject[] prefabSpawnersArray = GameObject.FindGameObjectsWithTag("Prefab Spawner");

        for(int i = 0; i < prefabSpawnersArray.Length; i++)
        {
            PrefabSpawner currentPrefabSpawner = prefabSpawnersArray[i].GetComponent<PrefabSpawner>();

            for(int j = 0; j < currentPrefabSpawner.SpawnedObjects.Count; j++)
            {
                spawnedObjects.Add(currentPrefabSpawner.SpawnedObjects[j]);
            }

            Destroy(prefabSpawnersArray[i].GetComponent<PrefabSpawner>());
        }

        for(int i = 0; i < spawnedObjects.Count; i++)
        {
            for(int j = i + 1; j < spawnedObjects.Count; j++)
            {
                if(Physics2D.IsTouching(spawnedObjects[i].GetComponent<Collider2D>(), spawnedObjects[j].GetComponent<Collider2D>()))
                {
                    Destroy(spawnedObjects[i]);
                    break;
                }
            }
        }

        Destroy(this);
	}
}
