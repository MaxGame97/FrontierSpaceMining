using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D player;                                             // The player's rigidbody

    [SerializeField] [Range(1f, 40f)] private float acceleration = 30f;     // The player's acceleration speed
    [SerializeField] [Range(1f, 30f)] private float maxSpeed = 12;          // The player's max speed
    [SerializeField] [Range(1f, 150f)] private float rotationSpeed = 100f;  // The player's rotation speed

    private float thrustAmount;                                             // The player's current thrust amount

    [SerializeField] private bool isEnabled = true;

    public float ThrustAmount { get { return thrustAmount; } }
    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }

    // Use this for initialization
    void Start () {
        GetComponent<Rigidbody2D>().inertia = (1);
        // Get the player's rigidbody
        player = GetComponent<Rigidbody2D>();
        
    }

    // Takes care of movement by the player
    void FixedUpdate()
    {
        // If the playermovement is enabled
        // TODO - Add this to a movement behaviour in Playerbehaviour instead
        if (isEnabled)
        {
            // In order for the player object to work properly, it is important
            // that the player's rigidbody has a reasonably large value in
            // "Angular Drag", otherwise the player will not stop spinning by
            // itself after a collision or rotation

            // Increase the torque based on the virtual horizontal axis input
            player.AddTorque(-Input.GetAxis("Horizontal") * rotationSpeed);

            // Get the thrust force based on the player input and the player's acceleration
            float thrustForce = Input.GetAxis("Vertical") * acceleration;

            // Add the thrust force in the forward direction
            player.AddForce(thrustForce * transform.up);

            // If the player is moving faster than the max speed
            if (player.velocity.magnitude > maxSpeed)
            {
                // Calculate how much force is neccesary to counter (neutralize) the thrust force
                float counterForce = Mathf.Abs(thrustForce) - (maxSpeed / player.velocity.magnitude);

                // Add the counter force in the opposite direction of travel
                player.AddForce(counterForce * -player.velocity.normalized);
            }
            
            // Get the thrust amount based on player input
            thrustAmount = Mathf.Abs(Input.GetAxis("Vertical"));
        }
        else
        {
            thrustAmount = 0f;
        }
    }
}
