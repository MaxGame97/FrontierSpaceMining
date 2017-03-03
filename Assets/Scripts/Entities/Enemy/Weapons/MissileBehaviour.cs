using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

    [SerializeField] [Range(0f, 1f)] private float acceleration = 0.2f;         // The Missile's acceleration speed
    [SerializeField] [Range(0f, 3f)] private float missileSpeed = 0.2f;         // The Missile's max speed
    [SerializeField] [Range(0.01f, 0.1f)] private float rotationSpeed = 2;      // The Missile's rotation speed
    [SerializeField] [Range(5f, 45f)] private float aliveTime = 30;             // Time until the missile is destroyed

    private Transform target;
    private Rigidbody2D missileRigidbody;

    // Use this for initialization
    void Start () {
        // Increase the speed of the bullet
        missileRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        MoveTowards(target.position, 1f);
        aliveTime -= Time.deltaTime;
        if(aliveTime <= 0) {
            Destroy(gameObject);
        }
	}

    // OnCollisionEnter2D is called on collision with another collider
    void OnCollisionEnter2D()
    {
        // Destroy the missile on collision
        Destroy(gameObject);
    }

    // Moves and rotates the enemy towards the target
    void MoveTowards(Vector3 target, float amount)
    {
        // Get the rotation needed to point towards the target
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - target, Vector3.forward);

        targetRotation.x = 0f;
        targetRotation.y = 0f;

        // Rotate the enemy linearly towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);

        // Correctional thrust vector, used for more precise steering as well as avoiding obstacles
        Vector3 correctedThrust = transform.up;

        if (missileRigidbody.velocity.magnitude > 0f)
        {
            if (Vector3.Angle(transform.up, missileRigidbody.velocity.normalized) < 90f)
                correctedThrust = Vector3.Reflect(missileRigidbody.velocity.normalized, transform.right);
            else
                correctedThrust = -missileRigidbody.velocity.normalized;
        }

        // Add force in the direction of travel, this is corrected by the correctional angle
        missileRigidbody.AddForce(correctedThrust * (acceleration * amount));

        // If the enemy is moving faster than the max speed
        if (missileRigidbody.velocity.magnitude > missileSpeed)
        {
            // Calculate how much force is neccesary to counter (neutralize) the thrust force
            float counterForce = Mathf.Abs(acceleration * amount) - (missileSpeed / missileRigidbody.velocity.magnitude);

            // Add the counter force in the opposite direction of travel
            missileRigidbody.AddForce(counterForce * -missileRigidbody.velocity.normalized);

        }
    }
}
