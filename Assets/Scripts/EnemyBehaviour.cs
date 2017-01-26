using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    private Transform target;
    [SerializeField] [Range(1f, 15f)] private float acceleration = 2;       // The AI's acceleration speed
    [SerializeField] [Range(1f, 20f)]  private float maxSpeed = 2;          // The AI's max speed
    [SerializeField] [Range(0.01f, 0.1f)] private float rotationSpeed = 2;  // The AI's rotation speed
    private float minDistance = 0.1f;                                          // The AI's minimum distance to the player
    private float range;                                                    // The AI's range towards the player
    private Quaternion newRotation;                                         // The AI's rotation angle

    private Rigidbody2D AI;

    // ----------------------------------------------------------------------------------------------------------------

    [SerializeField] [Range(10f, 180f)] private float fieldOfView;
    [SerializeField] [Range(5f, 25f)] private float viewDistance;
    [SerializeField] GameObject viewCone;

    private SpriteRenderer viewConeSprite;
    private Transform viewConeTransform;

    private LayerMask environmentLayerMask;
    private LayerMask playerLayerMask;

    private State movementState;
    private IdleState idleState;
    private AlertState alertState;
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
            enemy.viewConeSprite.color = new Color(0f / 255f, 120f / 255f, 255f / 255f);
        }

        public override void Update()
        {
            if (enemy.CanSeePlayer())
                Exit(enemy.alertState);
        }

        public override void Exit(State exitState)
        {
            enemy.movementState = exitState;
            exitState.Entry();
        }
    }

    private class AlertState : State
    {
        EnemyBehaviour enemy;

        public AlertState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            enemy.viewConeSprite.color = new Color(255f / 255f, 40f / 255f, 0f / 255f);
        }

        public override void Update()
        {
            enemy.range = Vector2.Distance(enemy.transform.position, enemy.target.position);

            if (enemy.range > enemy.minDistance)
            {

                //Move AI towards player
                enemy.AI.AddForce(enemy.transform.up * enemy.acceleration);

                // Clamp the player's velocity to the max speed
                enemy.AI.velocity = Vector2.ClampMagnitude(enemy.AI.velocity, enemy.maxSpeed);

                //Find what to rotate towards and reset x and y value since we dont want to ratate those axis
                enemy.newRotation = Quaternion.LookRotation(enemy.transform.position - enemy.target.position, Vector3.forward);
                enemy.newRotation.x = 0.0f;
                enemy.newRotation.y = 0.0f;

                //rotate AI towards player
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.newRotation, enemy.rotationSpeed);
            }

            if(!enemy.CanSeePlayer())
                Exit(enemy.searchingState);
        }

        public override void Exit(State exitState)
        {
            enemy.movementState = exitState;
            exitState.Entry();
        }
    }

    private class SearchingState : State
    {
        EnemyBehaviour enemy;
        Vector3 lastSeenPosition;

        public SearchingState(EnemyBehaviour enemy)
        {
            this.enemy = enemy;
        }

        public override void Entry()
        {
            lastSeenPosition = enemy.target.position;

            enemy.viewConeSprite.color = new Color(255f / 255f, 255f / 255f, 60f / 255f);
        }

        public override void Update()
        {
            enemy.range = Vector2.Distance(enemy.transform.position, lastSeenPosition);

            if (enemy.range > enemy.minDistance)
            {

                //Move AI towards player
                enemy.AI.AddForce(enemy.transform.up * enemy.acceleration);

                // Clamp the player's velocity to the max speed
                enemy.AI.velocity = Vector2.ClampMagnitude(enemy.AI.velocity, enemy.maxSpeed / 2);

                //Find what to rotate towards and reset x and y value since we dont want to ratate those axis
                enemy.newRotation = Quaternion.LookRotation(enemy.transform.position - lastSeenPosition, Vector3.forward);
                enemy.newRotation.x = 0.0f;
                enemy.newRotation.y = 0.0f;

                //rotate AI towards player
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.newRotation, enemy.rotationSpeed);
            }
            else
            {
                Exit(enemy.idleState);
            }

            if(enemy.CanSeePlayer())
                Exit(enemy.alertState);
        }

        public override void Exit(State exitState)
        {
            enemy.movementState = exitState;
            exitState.Entry();
        }
    }

    void Awake()
    {
        idleState = new IdleState(this);
        alertState = new AlertState(this);
        searchingState = new SearchingState(this);
    }

    // Use this for initialization
    void Start()
    {
        // Get the player's rigidbody
        AI = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        environmentLayerMask = LayerMask.GetMask("Environment");
        playerLayerMask = LayerMask.GetMask("Player");

        GameObject tempCone = Instantiate(viewCone);
        viewConeTransform = tempCone.transform;
        viewConeSprite = tempCone.GetComponent<SpriteRenderer>();

        movementState = idleState;
        movementState.Entry();

        viewConeTransform.position = transform.position;
        viewConeTransform.localRotation = transform.localRotation;
        viewConeTransform.Translate(0f, viewConeSprite.bounds.extents.y, 5f);
        viewConeTransform.parent = transform;
    }

    void FixedUpdate()
    {
        movementState.Update();
    }

    bool CanSeePlayer()
    {
        Vector3 targetDirection = target.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.up);

        if (Mathf.Abs(angle) <= fieldOfView / 2)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= viewDistance)
            {
                RaycastHit2D environmentRaycast = Physics2D.Raycast(transform.position, targetDirection, Mathf.Infinity, environmentLayerMask);
                RaycastHit2D playerRaycast = Physics2D.Raycast(transform.position, targetDirection, Mathf.Infinity, playerLayerMask);

                if (playerRaycast.distance < environmentRaycast.distance || environmentRaycast.collider == null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
