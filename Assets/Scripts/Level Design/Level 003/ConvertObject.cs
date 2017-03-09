using UnityEngine;
using System.Collections;

public class ConvertObject : MonoBehaviour {

    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int prefabAmount;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.transform.parent.gameObject == parent)
        {
            collision.gameObject.transform.parent.parent.gameObject.GetComponentInChildren<ConveyorBeltBehaviour>().movingObjectsList.Remove(collision.transform);
            Destroy(collision.gameObject);
            for (int i = 0; i < prefabAmount; i++)
            {
                
                GameObject go = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity, transform);
                go.transform.position += new Vector3(Random.Range(-4, 4), Random.Range(-4, 4));
            }
        }
    }
}
