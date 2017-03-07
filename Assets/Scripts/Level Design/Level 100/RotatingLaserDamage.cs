using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLaserDamage : MonoBehaviour
{
    public float damage;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerBehaviour>().DealDamage(damage);
        }
    }
}
