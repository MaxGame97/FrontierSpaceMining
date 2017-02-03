using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    [SerializeField] [Range(0.01f, 0.5f)] private float bulletSpeed = 0.1f; // The bullet's initial speed
    [SerializeField] [Range(0f, 100f)] private float bulletDamage = 10;     // The damage inflicted by the bullet

    public float BulletDamage { get { return bulletDamage; } }

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

}
