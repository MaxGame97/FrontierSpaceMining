using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    [SerializeField][Range(50, 3000)] private float bulletSpeed = 300;    // The Ai's speed of their bullets 
    [SerializeField][Range(5, 300)] private float bulletDamage = 10;    // The Ai's damage of their bullets 


    // Use this for initialization
    void Start () {
        //Constant moving of the bullet
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
    }

    // Update is called once per frame
    void Update () {

        
        

    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
        if(other.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
        }
    }

}
