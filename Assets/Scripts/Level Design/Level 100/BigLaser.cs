using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLaser : MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private float duration;

    private bool isActive = false;
    private float intervalCount = 0;
    private float durationCount = 0;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
            player.DealDamage(player.MaxHealth);
        }
    }

    private void Update()
    {
        if (isActive == false)
        {
            intervalCount += Time.deltaTime;
        }

        else if (isActive)
        {
            durationCount += Time.deltaTime;
        }

        if (intervalCount >= interval)
        {
            durationCount = 0;
            isActive = true;
            intervalCount = 0;
        }

        if (durationCount >= duration) isActive = false;

        gameObject.GetComponent<SpriteRenderer>().enabled = isActive;
        gameObject.GetComponent<BoxCollider2D>().enabled = isActive;
    }
}
