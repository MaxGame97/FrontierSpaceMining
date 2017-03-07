using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarSpawner : MonoBehaviour
{
    [SerializeField] private float spawnBaseRate;
    [SerializeField] private float speed;

    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject target;

    private float spawnRate;
    private float count;

    private void Start()
    {
        if (spawnBaseRate < 1)
        {
            spawnBaseRate = 1;
        }
        if (speed < 1)
        {
            speed = 1;
        }
        spawnRate = spawnBaseRate * Random.Range(0.5f, 3);
    }
    private void Update()
    {
        count += Time.deltaTime;

        if (count >= spawnRate)
        {
            GameObject go = (GameObject)Instantiate(prefab, transform.position, transform.rotation, transform);
            go.GetComponent<HangarSpawnedShip>().speed = speed;
            go.GetComponent<HangarSpawnedShip>().target = target;
            count = 0;
            spawnRate = spawnBaseRate * Random.Range(0.5f, 3);
        }
    }

}
