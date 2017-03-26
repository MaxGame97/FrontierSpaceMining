using UnityEngine;
using System.Collections;

public class StationaryTurretBehaviour : MonoBehaviour {

    private enum Behaviour { independent, connected };

    [SerializeField] private AudioClip bulletSoundClip;                             // The bullet sound clip
    [SerializeField] private GameObject soundFXPrefab;                             // The sound fx prefab


    [Header("Turret movement values")]

    [SerializeField] private bool hasLimits;                                    // remove?
    [SerializeField] [Range(0.01f, 0.1f)] private float rotationSpeed = 0.06f;  // The turrets rotation speed

    [Header("Turret stealth values")]
    
    [SerializeField] [Range(0f, 360f)] private float fieldOfView = 120;         // The turrets field of view
    [SerializeField] [Range(5f, 45f)] private float viewDistance = 30;          // The turrets view distance
    
    [Header("Turret weapon values")]

    [SerializeField] private GameObject projectile;                             // The turrets peojectile
    [SerializeField] [Range(0f, 10f)] private float weaponCooldown = 2f;        // The cooldown of the turrets weapon

    [Header("Turrent behaviour values")]

    [SerializeField] private Behaviour startingBehaviour;                       // The turrets start behaviour

    [Space(6f)]

    [SerializeField] [Range(0.5f, 2f)]private float maxAttentionTimer = 1f;

    [Header("Turret informant values")]

    [SerializeField] private GameObject[] informEnemies;
    [SerializeField] private GameObject[] informTurrets;

    private LayerMask environmentLayerMask;                                     // A layermask containing the environment layer
    private LayerMask playerLayerMask;                                          // A layermask containing the player layer

    private Transform target;                                                   // A transform containing the player
    
    private State currentState;                                                 // Different states the turret contains
    private State defaultState;
    private IndependentState independentState;
    private ConnectedState connectedState;
    private ShootingState shootingState;
    private AlertState alertState;

    private Vector3 startRotation;

    private class IndependentState : State
    {
        StationaryTurretBehaviour turret;

        public IndependentState(StationaryTurretBehaviour turret)
        {
            this.turret = turret;
        }
        public override void Update()
        {
            Transform target = GameObject.Find("Player").transform;

            if (turret.CanSeeTarget(target))
            {
                turret.target = target;

                Exit(turret.shootingState);
            }
            else
            {
                turret.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(turret.transform.localEulerAngles), Quaternion.Euler(turret.startRotation), turret.rotationSpeed);
            }
        }

        public override void Exit(State exitState)
        {
            turret.currentState = exitState;
            turret.currentState.Entry();
        }
    }

    private class ConnectedState : State
    {
        StationaryTurretBehaviour turret;

        public ConnectedState(StationaryTurretBehaviour turret)
        {
            this.turret = turret;
        }

        public override void Update()
        {
            Transform target = GameObject.Find("Player").transform;

            if (turret.CanSeeTarget(target))
            {
                turret.target = target;

                Exit(turret.shootingState);
            }
            else
            {
                turret.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(turret.transform.localEulerAngles), Quaternion.Euler(turret.startRotation), turret.rotationSpeed);
            }
        }

        public override void Exit(State exitState)
        {
            turret.currentState = exitState;
            turret.currentState.Entry();
        }
    }

    private class ShootingState : State
    {
        StationaryTurretBehaviour turret;

        float weaponCooldown;

        public ShootingState(StationaryTurretBehaviour turret)
        {
            this.turret = turret;
        }

        public override void Entry()
        {
            // Reset the weapon cooldown time
            weaponCooldown = turret.weaponCooldown;
        }

        public override void Update()
        {
            // Informs all assigned enemies of the current target's position
            for (int i = 0; i < turret.informEnemies.Length; i++)
            {
                turret.informEnemies[i].GetComponent<EnemyBehaviour>().Inform(turret.target);
            }

            // Informs all assigned turrets of the current target's position
            for (int i = 0; i < turret.informTurrets.Length; i++)
            {
                turret.informTurrets[i].GetComponent<StationaryTurretBehaviour>().Inform(turret.target);
            }

            // If the weapon cooldown time has expired
            if (weaponCooldown <= 0)
            {
                // Shoot a bullet
                turret.FireTurret();

                // Reset the weapon cooldown time
                weaponCooldown = turret.weaponCooldown;
            }
            // If not, decrease the weapon cooldown time
            else
                weaponCooldown -= Time.deltaTime;

            turret.RotateTowards(turret.target.position);

            if (!turret.CanSeeTarget(turret.target))
            {
                Exit(turret.alertState);
            }
        }

        public override void Exit(State exitState)
        {
            turret.currentState = exitState;
            turret.currentState.Entry();
        }
    }

    private class AlertState : State
    {
        StationaryTurretBehaviour turret;

        // The position where the target was last seen
        Vector3 lastPosition;

        // The attention timer before exiting to the default state
        float attentionTimer;

        public AlertState(StationaryTurretBehaviour turret)
        {
            this.turret = turret;
        }

        public override void Entry()
        {
            // Update the last seen position
            lastPosition = turret.target.position;

            // Reset the attention timer
            attentionTimer = turret.maxAttentionTimer;
        }

        public override void Update()
        {
            // Rotate towards the last seen position
            turret.RotateTowards(lastPosition);

            // If the turret can see the target, exit to the shooting state
            if (turret.CanSeeTarget(turret.target))
                Exit(turret.shootingState);

            // Decrease the attention timer
            attentionTimer -= Time.deltaTime;
            
            // If the attention timer has expired, exit to the default state
            if (attentionTimer <= 0f)
                Exit(turret.defaultState);
        }

        public override void Exit(State exitState)
        {
            turret.currentState = exitState;
            turret.currentState.Entry();
        }
    }

    public void FireTurret()
    {
        GameObject bulletObject = (GameObject)Instantiate(projectile, transform.position, transform.rotation);            
        // Create a bullet sound FX object
        GameObject bulletFX = (GameObject)Instantiate(soundFXPrefab, transform.position, new Quaternion());
        bulletFX.GetComponent<AudioSource>().clip = bulletSoundClip;
        bulletFX.GetComponent<AudioSource>().volume = 0.4f;

        Destroy(bulletObject, 5f);
    }

    // Awake is called before Start()
    void Awake()
    {
        // Instantiate all states
        independentState = new IndependentState(this);
        connectedState = new ConnectedState(this);
        shootingState = new ShootingState(this);
        alertState = new AlertState(this);
    }

    // Use this for initialization
    void Start () {
        environmentLayerMask = LayerMask.GetMask("Environment");                                        // Get the environment layer
        playerLayerMask = LayerMask.GetMask("Player");                                                  // Get the player layer

        startRotation = transform.localEulerAngles;

        // Set the starting state to the defined starting behaviour
        if (startingBehaviour == Behaviour.independent)
            currentState = independentState;
        else if (startingBehaviour == Behaviour.connected)
            currentState = connectedState;

        // Set the default state to the starting state
        defaultState = currentState;

        // Run the starting state's entry logic
        currentState.Entry();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        currentState.Update();
    }

    void RotateTowards(Vector3 target)
    {
        // Get the rotation needed to point towards the target
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - target, Vector3.forward);

        targetRotation.x = 0f;
        targetRotation.y = 0f;

        // Rotate the enemy linearly towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
    }

    // Checks whether or not the enemy can see the target
    bool CanSeeTarget(Transform targetPlayer)
    {
        Vector3 targetDirection = targetPlayer.position - transform.position; // Get the direction to the target
        float angle = Vector3.Angle(targetDirection, transform.up);     // Get the angle to the target

        // If the target is within the enemy's field of view
        if (Mathf.Abs(angle) <= fieldOfView / 2)
        {
            // If the target is within the enemy's view distance
            if (Vector3.Distance(transform.position, targetPlayer.position) <= viewDistance)
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

    public void Inform(Transform transform)
    {
        if(currentState != shootingState)
        {
            target = transform;

            currentState.Exit(alertState);
        }
    }
}
