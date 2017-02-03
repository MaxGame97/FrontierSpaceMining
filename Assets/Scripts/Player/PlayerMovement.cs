using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D player;                                         // The player's rigidbody

    [SerializeField] [Range(1f, 15f)] private float acceleration = 5;   // The player's acceleration speed
    [SerializeField] [Range(1f, 20f)] private float maxSpeed = 10;      // The player's max speed
    [SerializeField] [Range(1f, 20f)] private float rotationSpeed = 5;  // The player's rotation speed

    // Use this for initialization
    void Start () {
        // Get the player's rigidbody
        player = GetComponent<Rigidbody2D>();
        
    }

    // Takes care of movement by the player
    void FixedUpdate()
    {
        // In order for the player object to work properly, it is important
        // that the player's rigidbody has a reasonably large value in
        // "Angular Drag", otherwise the player will not stop spinning by
        // itself after a collision or rotation

        // Increase the torque based on the virtual horizontal axis input
        player.AddTorque(-Input.GetAxis("Horizontal") * rotationSpeed);

        // Increase velocity in the forward direction based on the virtual vertical axis input
        player.AddForce(Input.GetAxis("Vertical") * transform.up * acceleration);

        // Clamp the player's velocity to the max speed
        player.velocity = Vector2.ClampMagnitude(player.velocity, maxSpeed);
    }
}
