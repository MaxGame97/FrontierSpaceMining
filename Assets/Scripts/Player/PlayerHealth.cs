using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour {

    private PlayerBehaviour playerBehaviour;

    [SerializeField] private float maxHealth = 100f;                        // The Player's maximum health
    [SerializeField] [Range(1f, 5f)] private float velocityThreshold = 3f;  // The velocity needed to take damage on collision

    [SerializeField] private string[] ignoredCollisionTags;                 // The tags that the player will ignore on collision with

    [SerializeField] private GameObject soundFXPrefab;
    [SerializeField] private AudioClip impactSoundClip;

    private float currentHealth;                                            // The Player's current health

    [SerializeField] private bool isEnabled = true;

    public float MaxHealth { get { return maxHealth; } }
    public float CurrentHealth { get { return currentHealth; } }
    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }

    void Start ()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();

        // Set the player's current health to the max health
        currentHealth = maxHealth;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEnabled)
            return;

        // Check through all ignored collision tags
        for(int i = 0; i < ignoredCollisionTags.Length; i++)
        {
            // If the collided tag matched any of the ignored tags
            if (collision.gameObject.tag == ignoredCollisionTags[i])
                // Exit the function
                return;
        }

        // If not, continue

        // If the player is hit by a bullet
        if(collision.gameObject.tag == "Bullet")
        {
            // Get the bullet behaviour
            BulletBehaviour bullet = collision.gameObject.GetComponent<BulletBehaviour>();
            // Deal the bullet's damage to the player
            TakeDamage(bullet.BulletDamage);
        }
        else
        {
            // If the collided object has a rigidbody
            if(collision.rigidbody != null)
            {
                // If the relative velocity between the player and the collided object is greater than the velocity threshold
                if (collision.relativeVelocity.magnitude > velocityThreshold)
                {
                    Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();  // Get the player's rigidbody
                    Rigidbody2D collisionRigidbody = collision.rigidbody;       // Get the collision's rigidbody

                    // Get the damage multiplier, clamped to max 5
                    float damageMultiplier = Mathf.Clamp(collisionRigidbody.mass / playerRigidbody.mass, 0f, 5f);

                    // Deal damage to the player, based on the relative speed multiplied by the damage multiplier
                    TakeDamage((collision.relativeVelocity.magnitude - velocityThreshold) * damageMultiplier);

                    GameObject impactSound = (GameObject)Instantiate(soundFXPrefab);
                    AudioSource audioSource = impactSound.GetComponent<AudioSource>();

                    impactSound.transform.position = transform.position;

                    audioSource.clip = impactSoundClip;
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;    // Reduce current health with the amount of damage taken
        
        if(currentHealth <= 0)      // If all health is lost and the isDead bool is not yet set to true, 
        {
            playerBehaviour.KillPlayer();                // The player Dies
        }
    }
}
