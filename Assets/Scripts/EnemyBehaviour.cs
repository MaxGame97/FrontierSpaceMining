using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    private enum Behaviour { idle, recon, patrol, roaming };

    [Header("Enemy movement values")]

    [SerializeField] [Range(1f, 15f)] private float acceleration = 2;       // The AI's acceleration speed
    [SerializeField] [Range(1f, 20f)]  private float maxSpeed = 2;          // The AI's max speed
    [SerializeField] [Range(0.01f, 0.1f)] private float rotationSpeed = 2;  // The AI's rotation speed
    [SerializeField] [Range(0.5f, 2.5f)] private float minDistance = 1f;    // The AI's minimum distance to the player
    [SerializeField] [Range(1f, 5)] private float bulletAliveTime = 5f;     // The Ai's speed of their bullets 
    [SerializeField] [Range(1f, 10)] private float EMPduration = 5f;        // The Ai's speed of their bullets 

    [Space(2f)]

    [Header("Enemy stealth values")]

    [SerializeField] [Range(5f, 25f)] private float hearingValue = 10f;     // The AI's hearing value, the lower it is the faster the AI will detect the player 
    [SerializeField] [Range(0f, 5f)] private float enemyIntelligence = 0;   // The AI's intelligence, the higher this value is the faster the AI will detect the player
    [SerializeField] [Range(5f, 25f)] private float hearingRadius = 15f;    // The AI's hearing radius
    [SerializeField] [Range(10f, 180f)] private float fieldOfView;          // The enemy's field of view
    [SerializeField] [Range(5f, 25f)] private float viewDistance;           // The enemy's view distance
    [SerializeField] GameObject viewCone;                                   // The prefab object used for the view cone
    [SerializeField] GameObject bullet;                                     // The prefab object used for the bullet

    [Space(2f)]
    
    [Header("Enemy patrol/roaming values")]

    [SerializeField] private Behaviour startingBehaviour;

    [SerializeField] private Transform[] patrolNodes;

    [SerializeField] private float roamingRadius;

    [Space(2f)]

    [Header("Enemy misc values")]

    [SerializeField] GameObject soundFXPrefab;                              // The sound FX prefab

    [SerializeField] AudioClip alertSoundClip;                              // The alert sound clip
    [SerializeField] AudioClip bulletSoundClip;                             // The bullet sound clip

    private float range;                                                    // The AI's range towards the player
    private Quaternion newRotation;                                         // The AI's rotation angle

    private Transform target;                                               // The enemy's target (the player)

    private Rigidbody2D enemyRigidbody;                                     // The enemy's rigidbody
    
    private SpriteRenderer viewConeSprite;                                  // The spriterenderer component of the view cone
    private Transform viewConeTransform;                                    // The transform component of the view cone

    private LayerMask environmentLayerMask;                                 // A layermask containing the environment layer
    private LayerMask playerLayerMask;                                      // A layermask containing the player layer

    private MusicControllerBehaviour musicController;

    private State currentState;                                             // The enemy's current state
    private IdleState idleState;                                            // An idlestate instance
    private PatrolState patrolState;
    private AlertState alertState;                                          // An alertstate instance
    private SearchingState searchingState;                                  // A searchingstate instance
    private DisabledState disabledState;                                    // A disabledstate instance

    // When the enemy is in the idle state, it is waiting for the player to enter it's line of sight
    private class IdleState : State
    {
        // The current enemybehaviour instance
        EnemyBehaviour enemy;

        public IdleState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Change the view cone color to blue
            enemy.viewConeSprite.color = new Color(0f / 255f, 120f / 255f, 255f / 255f);
            // Increase the drag so that the enemy will stop
            enemy.enemyRigidbody.drag = 0.75f; // TODO - Make this solution better
        }

        public override void Update()
        {
            // If the enemy can hear the player, enter alert state
            if(enemy.CanHearPlayer())
                Exit(enemy.searchingState);
            // If the enemy can see the player, enter the alert state
            if (enemy.CanSeePlayer())
                Exit(enemy.alertState);
        }

        public override void Exit(State exitState)
        {
            // Remove the drag so that the enemy feels more "spacey"
            enemy.enemyRigidbody.drag = 0f; // TODO - Make this solution better

            enemy.currentState = exitState;
            exitState.Entry();
        }
    }

    private class PatrolState : State
    {
        // The current enemybehaviour instance
        EnemyBehaviour enemy;

        int patrolIndex;

        public PatrolState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;

            patrolIndex = 0;
        }

        public override void Entry()
        {
            // Change the view cone color to blue
            enemy.viewConeSprite.color = new Color(0f / 255f, 120f / 255f, 255f / 255f);
            // Increase the drag so that the enemy will stop
            enemy.enemyRigidbody.drag = 0.75f; // TODO - Make this solution better
        }

        public override void Update()
        {
            enemy.range = Vector3.Distance(enemy.transform.position, enemy.patrolNodes[patrolIndex].position);

            // If the enemy is too far away from the player's last position
            if (enemy.range > enemy.minDistance)
            {
                enemy.MoveTowards(enemy.patrolNodes[patrolIndex]);
            }
            else
            {
                if (patrolIndex == enemy.patrolNodes.Length)
                    patrolIndex = 0;
                else
                    patrolIndex++;
            }

            // If the enemy can hear the player, enter alert state
            //if (enemy.CanHearPlayer())
                //Exit(enemy.searchingState);
            // If the enemy can see the player, enter the alert state
            //if (enemy.CanSeePlayer())
                //Exit(enemy.alertState);
        }

        public override void Exit(State exitState)
        {
            // Remove the drag so that the enemy feels more "spacey"
            enemy.enemyRigidbody.drag = 0f; // TODO - Make this solution better

            enemy.currentState = exitState;
            exitState.Entry();
        }
    }

    // When the enemy is in the alert state, it is chasing the player until it loses sight of them
    private class AlertState : State
    {
        // The current enemybehaviour instance
        EnemyBehaviour enemy;

        public AlertState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Changes the color of the view cone to red
            enemy.viewConeSprite.color = new Color(255f / 255f, 40f / 255f, 0f / 255f);

            GameObject alertSound = (GameObject)Instantiate(enemy.soundFXPrefab, enemy.transform);
            AudioSource audioSource = alertSound.GetComponent<AudioSource>();

            alertSound.transform.localPosition = Vector3.zero;

            audioSource.clip = enemy.alertSoundClip;
        }

        public override void Update()
        {
            // If the enemy is too far away from the player's last position
            if (enemy.range > enemy.minDistance)
            {
                enemy.MoveTowards(enemy.target);
            }

            if (!enemy.IsInvoking("Shoot"))
            {
                enemy.InvokeRepeating("Shoot", 1, 1);
            }

            // If the enemy has lost sight of the player, enter the searching state
            if (!enemy.CanSeePlayer())
            {

                if (enemy.IsInvoking("Shoot"))
                {
                    enemy.CancelInvoke("Shoot");
                }
                Exit(enemy.searchingState);
            }
        }

        public override void Exit(State exitState)
        {
            enemy.currentState = exitState;
            exitState.Entry();
        }
    }

    // When the enemy is in the searching state, it is moving towards where it last saw the player
    private class SearchingState : State
    {
        // The current enemybehaviour instance
        EnemyBehaviour enemy;
        // The position where the player was last seen
        Vector3 lastSeenPosition;

        public SearchingState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Update the player's last position
            lastSeenPosition = enemy.target.position;
            // Change the color of the view cone to yellow
            enemy.viewConeSprite.color = new Color(255f / 255f, 255f / 255f, 60f / 255f);
        }

        public override void Update()
        {
            // TODO - Maybe add a more sophisticated movement system
            
            // Get the distance to the player's last position
            enemy.range = Vector2.Distance(enemy.transform.position, lastSeenPosition);

            // If the enemy is too far away from the player's last position
            if (enemy.range > enemy.minDistance)
            {
                enemy.MoveTowards(enemy.target);
            }
            // If the enemy has reached the player's last position, exit to the idle state
            else
            {
                Exit(enemy.idleState);
            }

            // If the enemy can hear the player, enter alert state
            if (enemy.CanHearPlayer())
                Exit(enemy.searchingState);
            // If the enemy can see the player, exit to the alert state
            if (enemy.CanSeePlayer())
                Exit(enemy.alertState);
        }

        public override void Exit(State exitState)
        {
            enemy.currentState = exitState;
            exitState.Entry();
        }
    }

    // When the enemy is hit by an EMP it becomes stunned/disabled for a set amount of time
    private class DisabledState : State
    {
        // The current enemybehaviour instance
        EnemyBehaviour enemy;

        float empduration;

        public DisabledState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Change the view cone color to blue
            enemy.viewConeSprite.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
            // Increase the drag so that the enemy will stop
            enemy.enemyRigidbody.drag = 0.75f; // TODO - Make this solution better

            empduration = enemy.EMPduration;
        }

        public override void Update()
        {
            if (enemy.IsInvoking("Shoot"))
            {
                enemy.CancelInvoke("Shoot");
            }
            empduration -= Time.deltaTime;
            if (empduration <= 0)
                Exit(enemy.idleState);

        }

        public override void Exit(State exitState)
        {
            // Remove the drag so that the enemy feels more "spacey"
            enemy.enemyRigidbody.drag = 0f; // TODO - Make this solution better

            enemy.currentState = exitState;
            exitState.Entry();
        }
    }

    void MoveTowards(Transform swag)
    {
        // Get the distance to the player
        range = Vector2.Distance(transform.position, swag.position);

        // If the enemy is too far away from the player
        if (range > minDistance)
        {
            //Move AI towards player
            enemyRigidbody.AddForce(transform.up * acceleration);

            // Clamp the player's velocity to the max speed
            enemyRigidbody.velocity = Vector2.ClampMagnitude(enemyRigidbody.velocity, maxSpeed);

            //Find what to rotate towards and reset x and y value since we dont want to ratate those axis
            newRotation = Quaternion.LookRotation(transform.position - swag.position, Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;

            //rotate AI towards player
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed);
        }
    }

    void Shoot()
    {
        GameObject tempBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);

        GameObject bulletFX = (GameObject)Instantiate(soundFXPrefab, transform.position, new Quaternion());
        AudioSource audioSource = bulletFX.GetComponent<AudioSource>();

        audioSource.clip = bulletSoundClip;

        Destroy(tempBullet, bulletAliveTime);

        musicController.TriggerBattleMusic();
    }

    void Awake()
    {
        idleState = new IdleState(this);            // Instantiates the idle state
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);          // Instantiates the alert state
        searchingState = new SearchingState(this);  // Instantiates the searching state
        disabledState = new DisabledState(this);    // Instantiates the disabled state
    }

    // Use this for initialization
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();                           // Get the enemy's rigidbody component

        target = GameObject.FindGameObjectWithTag("Player").transform;          // Find the player's transform component

        environmentLayerMask = LayerMask.GetMask("Environment");                // Get the environment layer
        playerLayerMask = LayerMask.GetMask("Player");                          // Get the player layer

        musicController = GameObject.Find("Music Controller").GetComponent<MusicControllerBehaviour>();

        GameObject tempCone = Instantiate(viewCone);                            // Create a view cone
        viewConeTransform = tempCone.transform;                                 // Get the view cone's transform component
        viewConeSprite = tempCone.GetComponent<SpriteRenderer>();               // Get the view cone's sprite renderer component

        viewConeTransform.position = transform.position;                        // Move the view cone to the enemy's position
        viewConeTransform.localRotation = transform.localRotation;              // Rotate the view cone to have the same rotation as the enemy
        viewConeTransform.Translate(0f, viewConeSprite.bounds.extents.y, 5f);   // Translate the view cone to border the enemy's front side
        viewConeTransform.parent = transform;                                   // Parent the enemy to the view cone, this will make the view cone follow the enemy

        if (startingBehaviour == Behaviour.idle)
            currentState = idleState;                                           // Set the idle state as the current state    
        else if (startingBehaviour == Behaviour.patrol)
            currentState = patrolState;           
        currentState.Entry();                                                   // Run the current state's entry function
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        // Update the current state
        currentState.Update();
    }

    // OnCollisionEnter2D is called when a collision occurs
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the object the enemy collided with is the player
        if(collision.gameObject.tag == "Player")
        {
            // If the enemy is in the idle state, enter the searching state
            if (currentState == idleState)
                currentState.Exit(searchingState);

            // If the enemy is in the searching state, run the entry function again
            if (currentState == searchingState)
                currentState.Entry();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EMP")
        {
            currentState = disabledState;
            currentState.Entry();
        }
    }

    // Checks whether or not the enemy can see the player
    bool CanSeePlayer()
    {
        Vector3 targetDirection = target.position - transform.position; // Get the direction to the player
        float angle = Vector3.Angle(targetDirection, transform.up);     // Get the angle to the player

        // If the player is within the enemy's field of view
        if (Mathf.Abs(angle) <= fieldOfView / 2)
        {
            // If the player is within the enemy's view distance
            if (Vector3.Distance(transform.position, target.transform.position) <= viewDistance)
            {
                // Raycast for an environment object
                RaycastHit2D environmentRaycast = Physics2D.Raycast(transform.position, targetDirection, Mathf.Infinity, environmentLayerMask);
                // Rayvast for the player object
                RaycastHit2D playerRaycast = Physics2D.Raycast(transform.position, targetDirection, Mathf.Infinity, playerLayerMask);

                // If either | no environment object was found | OR | the player object is closer than the environment object |
                if (playerRaycast.distance < environmentRaycast.distance || environmentRaycast.collider == null)
                {
                    // Return true, the enemy can see the player object
                    return true;
                }
            }
        }

        // If any of the above statements were false, return false, the enemy can't see the player object
        return false;
    }

    //Check whether or not the enemy can hear the player
    bool CanHearPlayer()
    {
        float thrustAmount;
        PlayerEngineSoundBehaviour playerEngine;
        playerEngine = GameObject.Find("Player").GetComponent<PlayerEngineSoundBehaviour>();
        thrustAmount = playerEngine.GetSpeedSound();

        range = Vector2.Distance(transform.position, target.position);

        if ((((hearingRadius - range) + thrustAmount + enemyIntelligence) * (thrustAmount / thrustAmount)) > hearingValue){
            return true;
        }
        
        return false;
    }
}
