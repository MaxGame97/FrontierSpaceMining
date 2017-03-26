using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRayBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
            player.DealDamage(player.MaxHealth);
        }
    }

    private void FixedUpdate()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = GetComponent<MeshRenderer>().enabled;
        if (transform.FindChild("LaserFX").GetComponent<AudioSource>() != null)
        {
            transform.FindChild("LaserFX").GetComponent<AudioSource>().enabled = GetComponent<MeshRenderer>().enabled;

        }
    }
}
