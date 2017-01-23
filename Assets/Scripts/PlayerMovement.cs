using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D player;

    [SerializeField] private float speed = 1;
    [SerializeField] private float speedLimit = 3;
    [SerializeField] private float rotationSpeed  = 3;

	// Use this for initialization
	void Start () {
        player = GetComponent<Rigidbody2D>();
	}

    // Takes care of movement by the player
    void Update()
    {
        //Rotates left when the left arrow is pressed
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.Rotate(0, 0, rotationSpeed);
        }

        //Accelerates the player when the up arrow is pressed and stops acceleration if speedlimit is reached.
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.AddForce(player.transform.up * speed);
            player.velocity = Vector2.ClampMagnitude(player.velocity, speedLimit);
        }


        //Rotates right when the right arrow is pressed
        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Rotate(0, 0, -rotationSpeed);
        }


        //Accelerates the player backwards when the down arrow is pressed and stops acceleration if speedlimit is reached.
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.AddForce(player.transform.up * -speed);
            player.velocity = Vector2.ClampMagnitude(player.velocity, speedLimit);
        }
    }

    //Fixes a bug where the player continously spins after colliding with objects
    void OnCollisionExit2D(Collision2D coll)
    {
        player.angularVelocity = 0;
    }
}
