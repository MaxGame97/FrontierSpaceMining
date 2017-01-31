using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D player;                                         // The player's rigidbody

    [SerializeField] [Range(1f, 15f)] private float acceleration = 5;   // The player's acceleration speed
    [SerializeField] [Range(1f, 20f)] private float maxSpeed = 10;      // The player's max speed
    [SerializeField] [Range(1f, 20f)] private float rotationSpeed = 5;  // The player's rotation speed
    [SerializeField] [Range(1f, 20f)] private float speedDamageLimit = 5;  // The player's required speed to take damage from collisions


    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ScrollingTextBehaviour ScrollText;

    private float currentSpeed;
    private float tempDamage;
    private int damageToTake;

    public Text speedtext;

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

        currentSpeed = player.velocity.magnitude;
    }

    void Update()
    {
        speedtext.text = currentSpeed.ToString();


        if (Input.GetButtonDown("Fire2"))
        {
            string temp = "Resources found: ";
            temp += Random.Range(100, 500) + " iron, ";
            temp += Random.Range(100, 500) + " zinc, ";
            temp += Random.Range(100, 500) + " nickel, ";
            temp += Random.Range(100, 50000) + " ice";
            ScrollText.setText(temp);
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag != "Item" && other.gameObject.layer == 8)
        {
            if(currentSpeed > speedDamageLimit)
            {
                tempDamage = currentSpeed - speedDamageLimit;
                tempDamage = Mathf.Pow(tempDamage, 1.3f);

                playerHealth.TakeDamage(tempDamage);
            }
        }
    }

}
