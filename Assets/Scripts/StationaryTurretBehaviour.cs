using UnityEngine;
using System.Collections;

public class StationaryTurretBehaviour : MonoBehaviour {

    private enum Behaviour { independent, connected };

    [SerializeField] private GameObject projectile;                             // The turrets peojectile
    [SerializeField] private bool hasLimits;                                    // remove?
    [SerializeField] [Range(0f, 360f)] private float fieldOfView = 120;         // The turrets field of view
    [SerializeField] [Range(5f, 45f)] private float viewDistance = 30;          // The turrets view distance
    [SerializeField] [Range(0.01f, 0.1f)] private float rotationSpeed = 0.06f;  // The turrets rotation speed

    private LayerMask environmentLayerMask;                                     // A layermask containing the environment layer
    private LayerMask playerLayerMask;                                          // A layermask containing the player layer
    private Transform target;                                                   // A transform containing the player

    [SerializeField] private Behaviour startingBehaviour;                       // The turrets start behaviour

    private State currentState;                                                 // Different states the turret contains
    private State defaultState;
    private IndependentState independentState;
    private ConnectedState connectedState;
    private ShootingState shootingState;

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
                turret.RotateTowards(target.position);
                Exit(turret.shootingState);
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

        public override void Entry()
        {
            
        }

        public override void Update()
        {

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

        public ShootingState(StationaryTurretBehaviour turret)
        {
            this.turret = turret;
        }

        public override void Entry()
        {

        }

        public override void Update()
        {
            Transform target = GameObject.Find("Player").transform;

            turret.RotateTowards(target.position);

            if (!turret.CanSeeTarget(target))
            {
                Exit(turret.defaultState);
            }
        }

        public override void Exit(State exitState)
        {
            turret.currentState = exitState;
            turret.currentState.Entry();
        }
    }

    // Awake is called before Start()
    void Awake()
    {
        // Instantiate all states
        independentState = new IndependentState(this);
        connectedState = new ConnectedState(this);
        shootingState = new ShootingState(this);
    }

    // Use this for initialization
    void Start () {
        environmentLayerMask = LayerMask.GetMask("Environment");                                        // Get the environment layer
        playerLayerMask = LayerMask.GetMask("Player");                                                  // Get the player layer

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
        Debug.Log(currentState);
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
}
