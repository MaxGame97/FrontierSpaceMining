using UnityEngine;
using System.Collections;

public class DestroyCollidingObject : MonoBehaviour {

    [SerializeField] GameObject prefab;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.gameObject == prefab)
        {
            transform.parent.gameObject.GetComponentInChildren<ConveyorBeltBehaviour>().movingObjectsList.Remove(collision.transform);
            Destroy(collision.gameObject);
        }
    }
}
