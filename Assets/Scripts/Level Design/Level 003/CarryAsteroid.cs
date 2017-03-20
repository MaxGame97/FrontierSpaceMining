using UnityEngine;
using System.Collections;

public class CarryAsteroid : MonoBehaviour {

    [SerializeField] GameObject mineDrone;
    [SerializeField] GameObject prefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pirate")
        {
            GameObject go = (GameObject)Instantiate(prefab, collision.transform.position, Quaternion.identity, collision.transform);
            go.name = "Carried Asteroid";
            go.transform.localPosition += new Vector3(0, 8);
        }
    }
}
