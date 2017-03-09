using UnityEngine;
using System.Collections;

public class ConveyorBeltBehaviour : MonoBehaviour {

    [SerializeField]
    float speed;
    [SerializeField] GameObject parent;
    public System.Collections.Generic.List<Transform> movingObjectsList;
    private void Start()
    {
        movingObjectsList = new System.Collections.Generic.List<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        movingObjectsList.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        movingObjectsList.Remove(collision.transform);

   
    }

    private void Update()
    {

        foreach (Transform item in movingObjectsList)
        {


            item.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    
}

 