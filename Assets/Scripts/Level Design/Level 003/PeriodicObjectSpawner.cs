using UnityEngine;
using System.Collections;

public class PeriodicObjectSpawner : MonoBehaviour {

    [SerializeField]
    GameObject prefab;
    [SerializeField]
    float interval;
    private float count;

    private void Update()
    {
        count += Time.deltaTime;

        if (count > interval)
        {
            SpawnPrefab(prefab);
            count = 0;
        }
    }

    public GameObject SpawnPrefab(GameObject prefab)
    {
           return (GameObject)Instantiate(prefab, transform.position, Quaternion.identity, transform);
    }
}
