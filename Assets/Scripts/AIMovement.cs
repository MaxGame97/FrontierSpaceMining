using UnityEngine;
using System.Collections;

public class AIMovement : MonoBehaviour {

    private Transform target;
    [SerializeField] [Range(1f, 15f)] private float acceleration = 2;   // The AI's acceleration speed
    [SerializeField] [Range(1f, 20f)] private float maxSpeed = 2;       // The AI's max speed
    [SerializeField] [Range(0.01f, 0.1f)] private float rotationSpeed = 2;  // The AI's rotation speed
    private float minDistance = 1;                                      // The AI's minimum distance to the player
    private float range;                                                // The AI's range towards the player
    private Quaternion newRotation;                                     // The AI's rotation angle

    private Rigidbody2D AI;


    // Use this for initialization
    void Start()
    {
        // Get the player's rigidbody
        AI = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        range = Vector2.Distance(transform.position, target.position);

        if (range > minDistance)
        {

            //Move AI towards player
            AI.AddForce(transform.up * acceleration);

            // Clamp the player's velocity to the max speed
            AI.velocity = Vector2.ClampMagnitude(AI.velocity, maxSpeed);

            //Find what to rotate towards and reset x and y value since we dont want to ratate those axis
            newRotation = Quaternion.LookRotation(transform.position - target.position, Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;

            //rotate AI towards player
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed);
        }
    }

}