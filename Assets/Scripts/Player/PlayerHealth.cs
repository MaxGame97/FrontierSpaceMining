using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour {

    [SerializeField] private float maxHealth = 100f;                        // The Player's maximum health
    [SerializeField] [Range(1f, 5f)] private float velocityThreshold = 3f;   // The velocity needed to take damage on collision

    private float currentHealth;                                            // The Player's current health

    public float MaxHealth { get { return maxHealth; } }
    public float CurrentHealth { get { return currentHealth; } }

	void Start ()
    {
        // Set the player's current health to the max health
        currentHealth = maxHealth;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            BulletBehaviour bullet = collision.gameObject.GetComponent<BulletBehaviour>();

            TakeDamage(bullet.BulletDamage);
        }
        else
        {
            if(collision.rigidbody != null)
            {
                if (collision.relativeVelocity.magnitude > velocityThreshold)
                    TakeDamage(collision.relativeVelocity.magnitude * 2);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;    // Reduce current health with the amount of damage taken
        
        if(currentHealth <= 0)      // If all health is lost and the isDead bool is not yet set to true, 
        {
            Death();                // The player Dies
        }
    }

    void Death()
    {
        // TODO - Change this to affect the player's state instead of disabling components
    }
}
