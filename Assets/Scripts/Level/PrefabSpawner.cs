using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PrefabSpawner : MonoBehaviour {
    
    [SerializeField] private GameObject prefab;
    [SerializeField] [Range(1, 500)] private int amount;
    [SerializeField] [Range(0.1f, 10f)] private float sizeFactorMin;
    [SerializeField] [Range(0.1f, 10f)] private float sizeFactorMax;
    [SerializeField] [Range(10f, 250f)] private float spawnRadius;

    [Space(6f)]

    [SerializeField] private bool cleanup = true;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    public float SpawnRadius { get { return spawnRadius; } }
    public List<GameObject> SpawnedObjects { get { return spawnedObjects; } }

    void Start()
    {
        GetSpawnedObjects();

        if (spawnedObjects.Count == 0)
            SpawnObjects();
    }

    // Use this for initialization
    public void SpawnObjects()
    {
        GetSpawnedObjects();

        if (spawnedObjects.Count > 0)
        {
            for(int i = 0; i < spawnedObjects.Count; i++)
            {
                DestroyImmediate(spawnedObjects[i], true);
            }

            spawnedObjects.Clear();
        }

        if(sizeFactorMax < sizeFactorMin)
        {
            float tempSizeFactorMax = sizeFactorMax;

            sizeFactorMax = sizeFactorMin;
            sizeFactorMin = tempSizeFactorMax;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject spawnedObject = Instantiate(prefab);

            Vector2 localSpawnPosition = Random.insideUnitCircle * spawnRadius;

            spawnedObject.transform.position = new Vector3(localSpawnPosition.x + transform.position.x, localSpawnPosition.y + transform.position.y, transform.position.z);

            spawnedObject.transform.Rotate(0f, 0f, Random.Range(0f, 360f));

            float scaleFactor = Random.Range(sizeFactorMin, sizeFactorMax);

            spawnedObject.transform.localScale *= scaleFactor;

            spawnedObject.transform.SetParent(transform);

            if(spawnedObject.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D spawnedObjectRigidbody = spawnedObject.GetComponent<Rigidbody2D>();

                spawnedObjectRigidbody.mass *= Mathf.Pow(scaleFactor, 2);
            }
        }

        if(cleanup)
            CleanUpObjects();
    }

    void GetSpawnedObjects()
    {
        spawnedObjects.Clear();

        for(int i = 0; i < transform.childCount; i++)
        {
            spawnedObjects.Add(transform.GetChild(i).gameObject);
        }
    }

    public void CleanUpObjects()
    {
        GetSpawnedObjects();

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            for (int j = i + 1; j < spawnedObjects.Count; j++)
            {
                if (spawnedObjects[i].GetComponent<Collider2D>().bounds.Intersects(spawnedObjects[j].GetComponent<Collider2D>().bounds))
                {
                    DestroyImmediate(spawnedObjects[i]);
                    break;
                }
            }
        }
    }
}
