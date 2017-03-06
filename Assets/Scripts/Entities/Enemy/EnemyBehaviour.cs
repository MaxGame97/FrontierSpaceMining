using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour {

    private enum Behaviour { idle, sentry, patrol, roaming };

    [Header("Enemy movement values")]

    [SerializeField] [Range(1f, 60f)] private float acceleration = 2;       // The AI's acceleration speed
    [SerializeField] [Range(1f, 40f)]  private float maxSpeed = 2;          // The AI's max speed
    [SerializeField] [Range(0.01f, 0.1f)] private float rotationSpeed = 2;  // The AI's rotation speed
    [SerializeField] [Range(1.5f, 5f)] private float minDistance = 2f;      // The AI's minimum distance to the player

    [Header("Enemy stealth values")]

    [SerializeField] [Range(5f, 25f)] private float hearingValue = 10f;     // The AI's hearing value, the lower it is the faster the AI will detect the player 
    [SerializeField] [Range(0f, 5f)] private float enemyIntelligence = 0;   // The AI's intelligence, the higher this value is the faster the AI will detect the player
    [SerializeField] [Range(5f, 25f)] private float hearingDistance = 15f;  // The AI's hearing radius
    [SerializeField] [Range(10f, 180f)] private float fieldOfView;          // The enemy's field of view
    [SerializeField] [Range(5f, 45f)] private float viewDistance;           // The enemy's view distance
    [SerializeField] GameObject viewCone;                                   // The prefab object used for the view cone

    [Header("Enemy weapon values")]

    [SerializeField] GameObject weaponPrefab;                               // The prefab object used for the bullet

    [SerializeField] [Range(1f, 5)] private float bulletAliveTime = 5f;     // The Ai's speed of their bullets 
    [SerializeField] [Range(0.1f, 2f)] private float weaponCooldown = 1f;
    
    [Header("Enemy behaviour values")]

    [SerializeField] private Behaviour startingBehaviour;

    [Space(6f)]

    [SerializeField] private bool patrolLoops = true;
    [SerializeField] private Transform[] patrolNodes;

    [Space(6f)]

    [SerializeField] [Range(1f, 35f)] private float roamingRadius = 10f;

    [Space(6f)]

    [SerializeField] [Range(1f, 15f)] private float searchingRadius = 2.5f;
    [SerializeField] [Range(10f, 30f)] private float searchingTime = 20f;

    [Header("Enemy misc values")]

    [SerializeField] private GameObject soundFXPrefab;                              // The sound FX prefab

    [SerializeField] private AudioClip alertSoundClip;                              // The alert sound clip
    [SerializeField] private AudioClip bulletSoundClip;                             // The bullet sound clip

    [Header("Enemy informant values")]

    [SerializeField] private GameObject[] informEnemies;
    [SerializeField] private GameObject[] informTurrets;

    private Rigidbody2D enemyRigidbody;                                     // The enemy's rigidbody

    private Transform currentTarget;

    private SpriteRenderer viewConeSprite;                                  // The spriterenderer component of the view cone
    private Transform viewConeTransform;                                    // The transform component of the view cone

    private LayerMask environmentLayerMask;                                 // A layermask containing the environment layer
    private LayerMask playerLayerMask;                                      // A layermask containing the player layer

    private MusicControllerBehaviour musicController;

    private State currentState;                                             // The enemy's current state
    private State defaultState;
    private IdleState idleState;
    private SentryState sentryState;
    private PatrolState patrolState;
    private RoamingState roamingState;
    private AlertState alertState;
    private InvestigatingState investigatingState;
    private SearchingState searchingState;

    private class IdleState : State
    {
        EnemyBehaviour enemy;

        public IdleState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Set the view cone color
            enemy.viewConeSprite.color = new Color(0f, 0.47f, 1f);
        }

        public override void Update()
        {
            // Get the active targets
            Transform[] targets = enemy.GetTargetTransforms();

            // Check all the active targets
            for(int i = 0; i < targets.Length; i++)
            {
                // Get the current target
                Transform target = targets[i];

                // If the enemy can see the target, enter the alert state
                if (enemy.CanSeeTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;
                    
                    Exit(enemy.alertState);
                }
                  
                // If the enemy can hear the target, enter the investigating state
                else if (enemy.CanHearTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;
                    
                    Exit(enemy.investigatingState);
                }
            }
        }

        public override void Exit(State exitState) {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }
    }

    private class SentryState : State
    {
        EnemyBehaviour enemy;

        Vector3 lookPosition;

        float lookPause;

        public SentryState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Set the view cone color
            enemy.viewConeSprite.color = new Color(0f, 0.47f, 1f);

            // Randomize a look position
            GenerateLookPosition();

            // Randomize a look pause
            lookPause = Random.Range(1f, 10f);
        }

        public override void Update()
        {
            // Rotate towards the current target
            enemy.RotateTowards(lookPosition);

            // If the look pause time has expired
            if (lookPause <= 0)
            {
                // Randomize a look position
                GenerateLookPosition();

                // Randomize a look pause
                lookPause = Random.Range(1f, 10f);
            }
            // If not, decrease the look pause time
            else
                lookPause -= Time.deltaTime;

            // Get the active targets
            Transform[] targets = enemy.GetTargetTransforms();

            // Check all the active targets
            for (int i = 0; i < targets.Length; i++)
            {
                // Get the current target
                Transform target = targets[i];

                // If the enemy can see the target, enter the alert state
                if (enemy.CanSeeTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.alertState);
                }

                // If the enemy can hear the target, enter the investigating state
                else if (enemy.CanHearTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.investigatingState);
                }
            }
        }

        public override void Exit(State exitState)
        {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }

        void GenerateLookPosition()
        {
            Vector2 enemyPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y);

            lookPosition = enemyPosition + (Random.insideUnitCircle * 10f);
        }
    }

    private class PatrolState : State
    {
        EnemyBehaviour enemy;

        int patrolIndex;

        bool positiveIndex;

        public PatrolState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;

            // Set the patrol index to 0
            patrolIndex = 0;

            positiveIndex = true;
        }

        public override void Entry()
        {
            // Set the view cone color
            enemy.viewConeSprite.color = new Color(0f, 0.47f, 1f);

            // If no patrol nodes are assigned
            if (enemy.patrolNodes.Length == 0)
            {
                // Throw an error message
                Debug.LogWarning("There are no patrol nodes assigned to '" + enemy.name + "' - switching to idle state");

                // Set the default state to the idle state
                enemy.defaultState = enemy.idleState;
                // Exit to the default state
                Exit(enemy.defaultState);
            }
            else
                // Set the current target to the current patrol node
                enemy.currentTarget = enemy.patrolNodes[patrolIndex];
        }

        public override void Update()
        {
            // Move towards the current patrol node
            enemy.MoveTowards(enemy.currentTarget.position, 0.3f);

            // If the enemy has reached the current patrol node
            if (Vector3.Distance(enemy.transform.position, enemy.currentTarget.position) < enemy.minDistance)
            {
                if (enemy.patrolLoops)
                {
                    // If the curren patrol index is the last, reset it
                    if (patrolIndex + 1 == enemy.patrolNodes.Length)
                        patrolIndex = 0;
                    // If not, increase it
                    else
                        patrolIndex++;
                }
                else
                {
                    if (positiveIndex)
                    {
                        // If the curren patrol index is the last, invert the patrol order
                        if (patrolIndex + 1 == enemy.patrolNodes.Length)
                        {
                            patrolIndex--;
                            positiveIndex = false;
                        }
                        // If not, increase it
                        else
                            patrolIndex++;
                    }
                    else
                    {
                        // If the curren patrol index is the first, invert the patrol order
                        if (patrolIndex == 0)
                        {
                            patrolIndex++;
                            positiveIndex = true;
                        }
                        // If not, decrease it
                        else
                            patrolIndex--;
                    }
                }

                // Set the current target to the current patrol node
                enemy.currentTarget = enemy.patrolNodes[patrolIndex];
            }

            // Get the active targets
            Transform[] targets = enemy.GetTargetTransforms();

            // Check all the active targets
            for (int i = 0; i < targets.Length; i++)
            {
                // Get the current target
                Transform target = targets[i];

                // If the enemy can see the target, enter the alert state
                if (enemy.CanSeeTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.alertState);
                }

                // If the enemy can hear the target, enter the investigating state
                else if (enemy.CanHearTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.investigatingState);
                }
            }
        }

        public override void Exit(State exitState)
        {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }
    }

    private class RoamingState : State
    {
        EnemyBehaviour enemy;

        Vector3 roamingPosition;

        public RoamingState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Set the view cone color
            enemy.viewConeSprite.color = new Color(0f, 0.47f, 1f);

            // Randomize a roaming position
            GenerateRoamingPosition();
        }

        public override void Update()
        {
            // Move towards the roaming position
            enemy.MoveTowards(roamingPosition, 0.3f);

            // If the enemy has reached the roaming position, randomize it to a new position
            if (Vector3.Distance(enemy.transform.position, roamingPosition) < enemy.minDistance)
                GenerateRoamingPosition();

            // Get the active targets
            Transform[] targets = enemy.GetTargetTransforms();

            // Check all the active targets
            for (int i = 0; i < targets.Length; i++)
            {
                // Get the current target
                Transform target = targets[i];

                // If the enemy can see the target, enter the alert state
                if (enemy.CanSeeTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.alertState);
                }

                // If the enemy can hear the target, enter the investigating state
                else if (enemy.CanHearTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.investigatingState);
                }
            }
        }

        public override void Exit(State exitState)
        {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }

        void GenerateRoamingPosition()
        {
            Vector2 enemyPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y);

            roamingPosition = enemyPosition + (Random.insideUnitCircle * enemy.roamingRadius);
        }
    }

    private class AlertState : State
    {
        EnemyBehaviour enemy;

        float weaponCooldown;

        public AlertState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Set the view cone color
            enemy.viewConeSprite.color = new Color(1f, 0.16f, 0f);

            // Reset the weapon cooldown time
            weaponCooldown = enemy.weaponCooldown;

            // Create an alert sound FX object
            GameObject alertFX = (GameObject)Instantiate(enemy.soundFXPrefab, enemy.transform.position, new Quaternion());
            alertFX.GetComponent<AudioSource>().clip = enemy.alertSoundClip;
            alertFX.GetComponent<AudioSource>().volume = 0.6f;
        }

        public override void Update()
        {
            // Informs all assigned enemies of the current target's position
            for (int i = 0; i < enemy.informEnemies.Length; i++)
            {
                enemy.informEnemies[i].GetComponent<EnemyBehaviour>().Inform(enemy.currentTarget);
            }

            /*
            // Informs all assigned turrets of the current target's position
            for (int i = 0; i < enemy.informEnemies.Length; i++)
            {
                enemy.informTurrets[i].GetComponent<StationaryTurretBehaviour>().Inform(enemy.currentTarget);
            }
            */

            // If the weapon cooldown time has expired
            if (weaponCooldown <= 0)
            {
                // Shoot a bullet
                enemy.FireWeapon();

                // Reset the weapon cooldown time
                weaponCooldown = enemy.weaponCooldown;
            }
            // If not, decrease the weapon cooldown time
            else
                weaponCooldown -= Time.deltaTime;

            // If the current target exists
            if (enemy.currentTarget != null)
                // Move towards the current target
                enemy.MoveTowards(enemy.currentTarget.position, 1f);
            // If not, exit to the default state
            else
                Exit(enemy.defaultState);

            // If the enemy has lost sight of the target
            if (!enemy.CanSeeTarget(enemy.currentTarget))
                Exit(enemy.investigatingState);
        }

        public override void Exit(State exitState)
        {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }
    }
    
    private class InvestigatingState : State
    {
        EnemyBehaviour enemy;

        Vector3 lastPosition;

        public InvestigatingState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;

            lastPosition = new Vector3();
        }

        public override void Entry()
        {
            // Set the view cone color
            enemy.viewConeSprite.color = new Color(1f, 1f, 0.24f);
            
            // Update the last spotted position to where the current target is
            lastPosition = enemy.currentTarget.position;
        }

        public override void Update()
        {
            // Move towards the last spotted position
            enemy.MoveTowards(lastPosition, 0.8f);

            // If the enemy has reached the last spotted position, exit to the searching state
            if (Vector3.Distance(enemy.transform.position, lastPosition) < enemy.minDistance)
                Exit(enemy.searchingState);

            // Get the active targets
            Transform[] targets = enemy.GetTargetTransforms();

            // Check all the active targets
            for (int i = 0; i < targets.Length; i++)
            {
                // Get the current target
                Transform target = targets[i];

                // If the enemy can see the target, enter the alert state
                if (enemy.CanSeeTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.alertState);
                }

                // If the enemy can hear the target, run the entry logic again
                else if (enemy.CanHearTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Entry();
                }
            }
        }

        public override void Exit(State exitState) {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }
    }

    private class SearchingState : State
    {
        EnemyBehaviour enemy;

        Vector3 searchingPosition;

        float searchingTime;

        public SearchingState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            // Set the view cone color
            enemy.viewConeSprite.color = new Color(1f, 1f, 0.24f);

            // Randomize a searching position, around the player's current position (makes the enemies act smarter, though a little cheating)
            Vector2 playerPosition = new Vector2(enemy.currentTarget.position.x, enemy.currentTarget.position.y);

            searchingPosition = playerPosition + (Random.insideUnitCircle * enemy.searchingRadius);

            // Set the searching time
            searchingTime = enemy.searchingTime;
        }

        public override void Update()
        {
            // If the searching time has expired, exit to the default state
            if (searchingTime <= 0)
                Exit(enemy.defaultState);
            // If not, decrease the searching time
            else
                searchingTime -= Time.deltaTime;

            // Move towards the searching position
            enemy.MoveTowards(searchingPosition, 0.3f);

            // If the enemy has reached the searching position, randomize it to a new position
            if (Vector3.Distance(enemy.transform.position, searchingPosition) < enemy.minDistance)
                GenerateSearchingPosition();

            // Get the active targets
            Transform[] targets = enemy.GetTargetTransforms();

            // Check all the active targets
            for (int i = 0; i < targets.Length; i++)
            {
                // Get the current target
                Transform target = targets[i];

                // If the enemy can see the target, enter the alert state
                if (enemy.CanSeeTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.alertState);
                }

                // If the enemy can hear the target, enter the investigating state
                else if (enemy.CanHearTarget(target))
                {
                    // Update the current target
                    enemy.currentTarget = target;

                    Exit(enemy.investigatingState);
                }
            }
        }

        public override void Exit(State exitState)
        {
            enemy.currentState = exitState;
            enemy.currentState.Entry();
        }

        void GenerateSearchingPosition()
        {
            Vector2 enemyPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y);

            searchingPosition = enemyPosition + (Random.insideUnitCircle * enemy.searchingRadius);
        }
    }

    // Awake is called before Start
    void Awake()
    {
        // Instantiate all states
        idleState = new IdleState(this);
        sentryState = new SentryState(this);
        patrolState = new PatrolState(this);
        roamingState = new RoamingState(this); 
        alertState = new AlertState(this);
        investigatingState = new InvestigatingState(this);
        searchingState = new SearchingState(this);
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

        // Set the starting state to the defined starting behaviour
        if (startingBehaviour == Behaviour.idle)
            currentState = idleState;
        else if (startingBehaviour == Behaviour.sentry)
            currentState = sentryState;
        else if (startingBehaviour == Behaviour.patrol)
            currentState = patrolState;
        else if (startingBehaviour == Behaviour.roaming)
            currentState = roamingState;

        // Set the default state to the starting state
        defaultState = currentState;

        // Run the starting state's entry logic
        currentState.Entry();
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

    // OnTriggerEnter2D is called when a trigger occurs
    void OnTriggerEnter2D(Collider2D trigger)
    {
        currentState.OnTriggerEnter2D(trigger);
    }

    // Rotates the enemy towards the target, DON'T USE THIS WITH MoveTowards()
    void RotateTowards(Vector3 target)
    {
        // Get the rotation needed to point towards the target
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - target, Vector3.forward);

        targetRotation.x = 0f;
        targetRotation.y = 0f;

        // Rotate the enemy linearly towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
    }

    // Moves and rotates the enemy towards the target, DONT USE THIS WITH RotateTowards()
    void MoveTowards(Vector3 target, float amount)
    {
        // Get the distance to the target
        float range = Vector2.Distance(transform.position, target);

        // If the enemy is too far away from the target
        if (range > minDistance)
        {

            // Get the rotation needed to point towards the target
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - target, Vector3.forward);

            targetRotation.x = 0f;
            targetRotation.y = 0f;

            // Rotate the enemy linearly towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);

            // Correctional thrust vector, used for more precise steering as well as avoiding obstacles
            Vector3 correctedThrust = transform.up;

            // If there is an obstacle in front of the enemy
            if (Physics2D.Raycast(transform.position, transform.up, 6f, environmentLayerMask))
            {
                float correctionalAngle = 0;

                // Check if the path is clear to the left
                if (!Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 35f) * transform.up, 4f, environmentLayerMask))
                    // If so, set the correctional angle to go left and slightly backwards
                    correctionalAngle = 100;
                else
                    // If not, set the correctional angle to go right and slightly backwards
                    correctionalAngle = -100;

                // Update the correctional vector
                correctedThrust = Quaternion.Euler(0, 0, correctionalAngle) * transform.up;
            }
            else
            {
                // If the enemy has a velocity
                if(enemyRigidbody.velocity.magnitude > 0f)
                {
                    // If the angle between the enemy's direction and its normalized velocity is smaller than 90 degrees (180 degrees covered)
                    if(Vector3.Angle(transform.up, enemyRigidbody.velocity.normalized) < 90f)
                        // Set the correctional thrust to the normalized velocity, reflected along the right axis
                        correctedThrust = Vector3.Reflect(enemyRigidbody.velocity.normalized, transform.right);
                    else
                    {
                        correctedThrust = -enemyRigidbody.velocity.normalized;
                    }
                }
            }

            float thrustByMass = (acceleration * amount) * enemyRigidbody.mass;

            // Add force in the direction of travel, this is corrected by the correctional angle
            enemyRigidbody.AddForce(correctedThrust * thrustByMass);

            // If the enemy is moving faster than the max speed
            if (enemyRigidbody.velocity.magnitude > maxSpeed)
            {
                // Calculate how much force is neccesary to counter (neutralize) the thrust force
                float counterForce = Mathf.Abs(thrustByMass) - (maxSpeed / enemyRigidbody.velocity.magnitude);

                // Add the counter force in the opposite direction of travel
                enemyRigidbody.AddForce(counterForce * -enemyRigidbody.velocity.normalized);
            }
        }
    }

    // Fires the enemy's weapon
    void FireWeapon()
    {
        // Instantiate a bullet object on the enemy's position and rotation
        GameObject bulletObject = (GameObject)Instantiate(weaponPrefab, transform.position, transform.rotation);

        // Set the bullet to self destruct after a defined period of time
        Destroy(bulletObject, bulletAliveTime);

        // Create a bullet sound FX object
        GameObject bulletFX = (GameObject)Instantiate(soundFXPrefab, transform.position, new Quaternion());
        bulletFX.GetComponent<AudioSource>().clip = bulletSoundClip;
        bulletFX.GetComponent<AudioSource>().volume = 0.7f;

        // Trigger battle music
        musicController.TriggerBattleMusic();
    }

    // Returns all target transforms
    Transform[] GetTargetTransforms()
    {
        // Create a list of transforms
        List<Transform> targetObjects = new List<Transform>();

        // Add the player object to the list
        targetObjects.Add(GameObject.Find("Player").transform);

        // Return the list as an array
        return targetObjects.ToArray();
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

    // Check whether or not the enemy can hear the target
    bool CanHearTarget(Transform target)
    {
        // Get the target's engine sound component
        PlayerEngineSoundBehaviour playerEngine = GameObject.Find("Player").GetComponent<PlayerEngineSoundBehaviour>();

        // Get the thrust amount from the target's engine sound component
        float thrustAmount = Mathf.Clamp(playerEngine.PlayerThrustAmount, 0.35f, 1f);

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

    // Function called to inform the enemy of a target's position
    public void Inform(Transform target)
    {
        // If the enemy is not already alerted
        if(currentState != alertState)
        {
            // Update the enemy's current target
            currentTarget = target;

            // Exit to the investigating state
            currentState.Exit(investigatingState);
        }
    }
}
