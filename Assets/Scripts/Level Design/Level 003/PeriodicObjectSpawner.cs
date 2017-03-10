using UnityEngine;
using System.Collections;

public class PeriodicObjectSpawner : MonoBehaviour {

    [SerializeField]
    GameObject prefab;
    [SerializeField]
    //float interval;
    private float count;
    [SerializeField]
    GameObject mineDrone;

   /*  private void Update()
     {
         count += Time.deltaTime;

         if (count > interval)
         {
             SpawnPrefab(prefab);
             count = 0;
         }
     }
     */

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pirate")
        {
            Destroy(collision.gameObject.transform.FindChild("Carried Asteroid").gameObject);
            SpawnPrefab(prefab);
        }
    }

    public GameObject SpawnPrefab(GameObject prefab)
    {
           return (GameObject)Instantiate(prefab, transform.position, Quaternion.identity, transform);
    }
}
