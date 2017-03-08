using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLaserDamage : MonoBehaviour
{
    [SerializeField] private float entryDamage = 50;            // The damage the player takes if it collides with the laser beam
    [SerializeField] private float damagePerSecond = 100;       // The damage per second the player takes while staying in the laser beam

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerBehaviour>().DealDamage(entryDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerBehaviour>().DealDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
