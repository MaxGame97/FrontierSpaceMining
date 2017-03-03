using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    [SerializeField] [Range(0.01f, 0.5f)] private float bulletSpeed = 0.1f; // The bullet's initial speed

    // Use this for initialization
    void Start () {
        // Increase the speed of the bullet
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
    }

    // OnCollisionEnter2D is called on collision with another collider
     void OnCollisionEnter2D()
    {
        // Destroy the bullet on collision
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shield")
        {
            Destroy(gameObject);
        }
    }
}
