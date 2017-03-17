using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    [Header("Player movement values")]

    [SerializeField] [Range(1f, 40f)] private float acceleration = 30f;         // The player's acceleration speed
    [SerializeField] [Range(1f, 30f)] private float maxSpeed = 12;              // The player's max speed
    [SerializeField] [Range(1f, 150f)] private float rotationSpeed = 100f;      // The player's rotation speed

    [Space(2f)]

    [Header("Player health values")]

    [SerializeField] private float maxHealth = 500f;                            // The Player's maximum health
    [SerializeField] [Range(1f, 20f)] private float velocityThreshold = 10f;    // The velocity needed to take damage on collision

    [SerializeField] private string[] ignoredCollisionTags;                     // The tags that the player will ignore on collision with

    [SerializeField] private GameObject soundFXPrefab;                          // The sound FX prefab object
    [SerializeField] private AudioClip impactSoundClip;                         // The impact sound clip

    [Space(6f)]

    [SerializeField] private GameObject failStateObject;

    private float currentHealth;                                                // The player's current health

    public float MaxHealth { get { return maxHealth; } }
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    private ItemPickupBehaviour itemPickupBehaviour;
    private MiningLaser miningLaser;

    private State movementState;
    private ControlledState controlledState;
    private IdleState idleState;

    private State healthState;
    private AliveState aliveState;
    private DeadState deadState;

    private class ControlledState : State
    {
        PlayerBehaviour player;
        PlayerEngineSoundBehaviour playerEngine;

        Rigidbody2D playerRigidbody;

        float defaultLinearDrag;
        float defaultAngularDrag;

        public ControlledState(PlayerBehaviour player)
        {
            this.player = player;

            // Get the player engine sound component
            playerEngine = player.GetComponent<PlayerEngineSoundBehaviour>();
            
            // Get the player's rigidbody
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            // Get the rigidbody's default linear and angular drag
            defaultLinearDrag = playerRigidbody.drag;
            defaultAngularDrag = playerRigidbody.angularDrag;
        }

        public override void Entry()
        {
            // Set the rigidbody's linear and angular drag to the default values
            playerRigidbody.drag = defaultLinearDrag;
            playerRigidbody.angularDrag = defaultAngularDrag;
        }

        public override void Update()
        {
            // In order for the player object to work properly, it is important
            // that the player's rigidbody has a reasonably large value in
            // "Angular Drag", otherwise the player will not stop spinning by
            // itself after a collision or rotation

            // Increase the torque based on the virtual horizontal axis input
            playerRigidbody.AddTorque(-Input.GetAxis("Horizontal") * player.rotationSpeed);

            float thrustForce;
            float maxSpeed = player.maxSpeed;

            // If the player wants to move forward
            if (Input.GetAxis("Vertical") > 0f)
            {
                // If the player is pressing the boost button
                if (Input.GetButton("Boost"))
                {
                    // Set the thrust force to twice the player's input, times the acceleration
                    thrustForce = Mathf.Clamp(Input.GetAxis("Vertical") * 2, -0.25f, 2f) * player.acceleration;

                    // Set the max speed to twice the player's max speed
                    maxSpeed = player.maxSpeed * 2;
                }
                else
                {
                    // Set the thrust force to the player's input, times the acceleration
                    thrustForce = Mathf.Clamp(Input.GetAxis("Vertical"), -0.25f, 1f) * player.acceleration;
                }

                // Add the thrust force in the forward direction
                playerRigidbody.AddForce(thrustForce * player.transform.up);

                // If the playerRigidbody is moving faster than the max speed
                if (playerRigidbody.velocity.magnitude > maxSpeed)
                {
                    // Calculate how much force is neccesary to counter (neutralize) the thrust force
                    float counterForce = Mathf.Abs(thrustForce) - (maxSpeed / playerRigidbody.velocity.magnitude);

                    // Add the counter force in the opposite direction of travel
                    playerRigidbody.AddForce(counterForce * -playerRigidbody.velocity.normalized);
                }
            }
            // Else, if the player wants to stop
            else if(Input.GetAxis("Vertical") < 0f)
            {
                maxSpeed = player.maxSpeed / 2f;

                if (!Input.GetButton("Boost") || playerRigidbody.velocity.magnitude > maxSpeed)
                {
                    thrustForce = Input.GetAxis("Vertical") * player.acceleration;

                    playerRigidbody.AddForce(playerRigidbody.velocity.normalized * thrustForce);
                }
                else
                {
                    thrustForce = Input.GetAxis("Vertical") * player.acceleration / 2f;
                    
                    // Add the thrust force in the forward direction
                    playerRigidbody.AddForce(thrustForce * player.transform.up);

                    // If the playerRigidbody is moving faster than the max speed
                    if (playerRigidbody.velocity.magnitude > maxSpeed)
                    {
                        // Calculate how much force is neccesary to counter (neutralize) the thrust force
                        float counterForce = Mathf.Abs(thrustForce) - (maxSpeed / playerRigidbody.velocity.magnitude);

                        // Add the counter force in the opposite direction of travel
                        playerRigidbody.AddForce(counterForce * -playerRigidbody.velocity.normalized);
                    }
                }
            }

            // Set the engine's thrust amount to the current value
            playerEngine.PlayerThrustAmount = Mathf.Abs(Input.GetAxis("Vertical"));
        }

        public override void Exit(State exitState)
        {
            player.movementState = exitState;
            player.movementState.Entry();
        }
    }

    private class IdleState : State
    {
        PlayerBehaviour player;
        PlayerEngineSoundBehaviour playerEngine;

        Rigidbody2D playerRigidbody;

        public IdleState(PlayerBehaviour player)
        {
            this.player = player;

            // Get the player engine sound component
            playerEngine = player.GetComponent<PlayerEngineSoundBehaviour>();

            // Get the player's rigidbody
            playerRigidbody = player.GetComponent<Rigidbody2D>();
        }

        public override void Entry()
        {
            // Set the rigidbody's linear and angular drag to the default values
            playerRigidbody.drag = 0f;
            playerRigidbody.angularDrag = 0f;

            // Set the engine's thrust amount to 0
            playerEngine.PlayerThrustAmount = 0f;
        }

        public override void Exit(State exitState)
        {
            player.healthState = exitState;
            player.healthState.Entry();
        }
    }

    private class AliveState : State
    {
        PlayerBehaviour player;

        Rigidbody2D playerRigidbody;

        public AliveState(PlayerBehaviour player)
        {
            this.player = player;

            // Get the player's rigidbody
            playerRigidbody = player.GetComponent<Rigidbody2D>();

            // Set the player's current health to the max health
            player.currentHealth = player.maxHealth;
        }

        public override void Entry()
        {
            player.itemPickupBehaviour.enabled = true;
            player.miningLaser.enabled = true;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            CheckDamage(collision);
        }

        public override void Exit(State exitState)
        {
            player.healthState = exitState;
            player.healthState.Entry();
        }

        // Checks if the player should take damage
        void CheckDamage(Collision2D collision)
        {
            // Check through all ignored collision tags
            for (int i = 0; i < player.ignoredCollisionTags.Length; i++)
            {
                // If the collided tag matched any of the ignored tags
                if (collision.gameObject.tag == player.ignoredCollisionTags[i])
                    // Exit the function
                    return;
            }

            // If not, continue

            // If the player is hit by a bullet
            if (collision.gameObject.tag == "Projectile")
            {
                // Get the bullet behaviour
                ProjectileBehaviour projectile = collision.gameObject.GetComponent<ProjectileBehaviour>();
                // Deal the bullet's damage to the player
                player.DealDamage(projectile.Damage);
            }
            else
            {
                // If the collided object has a rigidbody
                if (collision.rigidbody != null)
                {
                    // If the relative velocity between the player and the collided object is greater than the velocity threshold
                    if (collision.relativeVelocity.magnitude > player.velocityThreshold)
                    {
                        // Get the collision's rigidbody
                        Rigidbody2D collisionRigidbody = collision.rigidbody;

                        // Get the damage multiplier, clamped to max 5
                        float damageMultiplier = Mathf.Clamp(collisionRigidbody.mass / playerRigidbody.mass, 0f, 5f);

                        // Deal damage to the player, based on the relative speed multiplied by the damage multiplier
                        player.DealDamage((collision.relativeVelocity.magnitude - player.velocityThreshold) * damageMultiplier);

                        GameObject impactSound = (GameObject)Instantiate(player.soundFXPrefab);
                        AudioSource audioSource = impactSound.GetComponent<AudioSource>();

                        impactSound.transform.position = player.transform.position;

                        audioSource.clip = player.impactSoundClip;
                    }
                }
            }
        }
    }

    private class DeadState : State
    {
        PlayerBehaviour player;

        public DeadState(PlayerBehaviour player)
        {
            this.player = player;
        }

        public override void Entry()
        {
            player.movementState.Exit(player.idleState);

            player.itemPickupBehaviour.enabled = false;
            player.miningLaser.enabled = false;

            Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
            rigidbody.angularDrag = 0;
            rigidbody.drag = 0;

            Instantiate(player.failStateObject);
        }

        public override void Exit(State exitState)
        {
            player.healthState = exitState;
            player.healthState.Entry();
        }
    }

    void Awake()
    {
        controlledState = new ControlledState(this);
        idleState = new IdleState(this);

        aliveState = new AliveState(this);
        deadState = new DeadState(this);
    }

	// Use this for initialization
	void Start () {
        // Set the rigidbody's inertia to 1 (default)
        GetComponent<Rigidbody2D>().inertia = (1);

        itemPickupBehaviour = GetComponent<ItemPickupBehaviour>();
        miningLaser = GetComponent<MiningLaser>();

        movementState = controlledState;
        movementState.Entry();

        healthState = aliveState;
        healthState.Entry();
	}
	
	void FixedUpdate () {
        movementState.Update();

        healthState.Update();
	}

    void OnCollisionEnter2D(Collision2D collider)
    {
        movementState.OnCollisionEnter2D(collider);

        healthState.OnCollisionEnter2D(collider);
    }

    public void KillPlayer()
    {
        if (healthState != deadState)
            healthState.Exit(deadState);
    }

    // Deals damage to the player
    public void DealDamage(float amount)
    {
        // If the player is alive
        if (healthState == aliveState)
        {
            // Reduce current health with the amount of damage taken
            currentHealth -= amount;

            // If all health has depleted
            if (currentHealth <= 0)
            {
                currentHealth = 0;              // Reset the player's current health

                healthState.Exit(deadState);    // Kills the player
            }
        }
    }
}
