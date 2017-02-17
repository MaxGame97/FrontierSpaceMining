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
    [SerializeField] [Range(5f, 25f)] private float hearingDistance = 15f;    // The AI's hearing radius
    [SerializeField] [Range(10f, 180f)] private float fieldOfView;          // The enemy's field of view
    [SerializeField] [Range(5f, 25f)] private float viewDistance;           // The enemy's view distance
    [SerializeField] GameObject viewCone;                                   // The prefab object used for the view cone
    [SerializeField] GameObject bullet;                                     // The prefab object used for the bullet

    [Space(2f)]
    
    [Header("Enemy behaviour values")]

    [SerializeField] private Behaviour startingBehaviour;

    [SerializeField] private Transform[] patrolNodes;

    [SerializeField] private float roamingRadius;

    [Space(2f)]

    [Header("Enemy misc values")]

    [SerializeField] GameObject soundFXPrefab;                              // The sound FX prefab

    [SerializeField] AudioClip alertSoundClip;                              // The alert sound clip
    [SerializeField] AudioClip bulletSoundClip;                             // The bullet sound clip
    
    private Rigidbody2D enemyRigidbody;                                     // The enemy's rigidbody
    
    private SpriteRenderer viewConeSprite;                                  // The spriterenderer component of the view cone
    private Transform viewConeTransform;                                    // The transform component of the view cone

    private LayerMask environmentLayerMask;                                 // A layermask containing the environment layer
    private LayerMask playerLayerMask;                                      // A layermask containing the player layer

    private MusicControllerBehaviour musicController;

    private State currentState;                                             // The enemy's current state

    /*
    private class IdleState : State
    {
        EnemyBehaviour enemy;

        public IdleState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        //public override void OnTriggerEnter2D(Collision2D trigger)
        //{

        //}

        public override void Exit(State exitState) {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }
    }
    */

    private class IdleState : State
    {
        EnemyBehaviour enemy;

        Transform[] targets;

        public IdleState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            enemy.viewConeSprite.color = Color.cyan;
        }

        public override void Update()
        {
            targets = enemy.GetTargetObjects();

            if (enemy.CanSeeTarget(target))
                Exit(enemy.AlertState);
            if (enemy.CanHearTarget(target))
                Exit(enemy.AlertState);
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        //public override void OnTriggerEnter2D(Collision2D trigger)
        //{

        //}

        public override void Exit(State exitState) {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }
    }

    void MoveTowards(Transform target)
    {
        // Get the distance to the target
        float range = Vector2.Distance(transform.position, target.position);

        // If the enemy is too far away from the target
        if (range > minDistance)
        {
            // Add force in the direction of travel
            enemyRigidbody.AddForce(transform.up * acceleration);

            // If the enemy is moving faster than the max speed
            if (enemyRigidbody.velocity.magnitude > maxSpeed) {
                // Calculate how much force is neccesary to counter (neutralize) the thrust force
                float counterForce = Mathf.Abs(acceleration) - (maxSpeed / enemyRigidbody.velocity.magnitude);

                // Add the counter force in the opposite direction of travel
                enemyRigidbody.AddForce(counterForce * -enemyRigidbody.velocity.normalized);
            }

            // Get the rotation needed to point towards the target
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - target.position, Vector3.forward);

            targetRotation.x = 0f;
            targetRotation.y = 0f;

            // Rotate the enemy linearly towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
        }
    }

    void Shoot()
    {
        // Instantiate a bullet object on the enemy's position and rotation
        GameObject bulletObject = (GameObject)Instantiate(bullet, transform.position, transform.rotation);

        // Set the bullet to self destruct after a defined period of time
        Destroy(bulletObject, bulletAliveTime);

        // Instatniate a sound FX object at the enemy's position and get its audio source component
        GameObject bulletFX = (GameObject)Instantiate(soundFXPrefab, transform.position, new Quaternion());
        AudioSource audioSource = bulletFX.GetComponent<AudioSource>();

        // Set the sound FX's audio source clip to the bullet audio clip
        audioSource.clip = bulletSoundClip;
        
        // Trigger battle music
        musicController.TriggerBattleMusic();
    }

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();                                                   // Get the enemy's rigidbody component

        environmentLayerMask = LayerMask.GetMask("Environment");                                        // Get the environment layer
        playerLayerMask = LayerMask.GetMask("Player");                                                  // Get the player layer

        musicController = GameObject.Find("Music Controller").GetComponent<MusicControllerBehaviour>(); // Get the music controller component

        GameObject tempCone = Instantiate(viewCone);                                                    // Create a view cone
        viewConeTransform = tempCone.transform;                                                         // Get the view cone's transform component
        viewConeSprite = tempCone.GetComponent<SpriteRenderer>();                                       // Get the view cone's sprite renderer component

        viewConeTransform.position = transform.position;                                                // Move the view cone to the enemy's position
        viewConeTransform.localRotation = transform.localRotation;                                      // Rotate the view cone to have the same rotation as the enemy
        viewConeTransform.Translate(0f, viewConeSprite.bounds.extents.y, 5f);                           // Translate the view cone to border the enemy's front side
        viewConeTransform.parent = transform;                                                           // Parent the enemy to the view cone, this will make the view cone follow the enemy                                          // Run the current state's entry function
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        currentState.Update();
    }

    // OnCollisionEnter2D is called when a collision occurs
    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(collision);
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        //currentState.OnTriggerEnter2D(trigger)
    }

    Transform[] GetTargetObjects()
    {
        return GameObject.Find("Player").transform;
    }

    // Checks whether or not the enemy can see the target
    bool CanSeeTarget(Transform target)
    {
        Vector3 targetDirection = target.position - transform.position; // Get the direction to the target
        float angle = Vector3.Angle(targetDirection, transform.up);     // Get the angle to the target

        // If the target is within the enemy's field of view
        if (Mathf.Abs(angle) <= fieldOfView / 2)
        {
            // If the target is within the enemy's view distance
            if (Vector3.Distance(transform.position, target.position) <= viewDistance)
            {
                // Raycast for an environment object
                RaycastHit2D environmentRaycast = Physics2D.Raycast(transform.position, targetDirection, Mathf.Infinity, environmentLayerMask);
                // Raycast for the target object
                RaycastHit2D playerRaycast = Physics2D.Raycast(transform.position, targetDirection, Mathf.Infinity, playerLayerMask);

                // If either no environment object was found, or the target object is closer than the environment object
                if (playerRaycast.distance < environmentRaycast.distance || environmentRaycast.collider == null)
                {
                    // Return true, the enemy can see the target object
                    return true;
                }
            }
        }

        // If any of the above statements were false, return false, the enemy can't see the target object
        return false;
    }

    //Check whether or not the enemy can hear the target
    bool CanHearTarget(Transform target)
    {
        // Get the target's engine sound component
        PlayerEngineSoundBehaviour playerEngine = GameObject.Find("Player").GetComponent<PlayerEngineSoundBehaviour>();

        // Get the thrust amount from the target's engine sound component
        float thrustAmount = playerEngine.GetSpeedSound();

        // Get the distance to the target
        float distance = Vector2.Distance(transform.position, target.position);

        // Calculate the hearing value of the target
        float targetHearingValue = Mathf.Clamp(hearingDistance - distance, 0f, Mathf.Infinity) * thrustAmount * enemyIntelligence;

        // If the hearing value of the target is large enough, return true
        if (targetHearingValue > hearingValue)
            return true;
        else
            return false;
    }
}
