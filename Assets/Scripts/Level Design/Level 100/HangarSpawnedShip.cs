using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarSpawnedShip : MonoBehaviour
{
    public float speed;
    public GameObject target;
    private float timer = 15;
    private float count = 0;
    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
         if (Vector2.Distance(transform.position, target.transform.position) < 5)
        {
            Destroy(gameObject);
        }
        count += Time.deltaTime;
        if (count >= timer)
        {

            Destroy(gameObject);
        }
    }
}
