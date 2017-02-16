using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

    [SerializeField] private GameObject notificationTextPrefab; // The notification text prefab object

    List<string> notificationTextQueue = new List<string>();    // List of strings (a queue of strings) that will be displayed as notifications

    private float maxQueueTime;                                 // The max queue time (in seconds)
    private float currentQueueTime = 0f;                        // The current queue time (in seconds)

    private Transform hUDTransform;                             // The transform of the HUD canvas

    private Slider healthSlider;                                // The health slider UI
    private Text velocityText;                                  // The velocity text UI

    private Rigidbody2D playerRigidbody;                        // The player's rigidbody

    private PlayerBehaviour playerBehaviour;                    // The player's health component

	// Use this for initialization
	void Start () {
        // If the player object exists
        if (GameObject.Find("Player") != null)
        {
            // Get the player's health component
            playerBehaviour = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
            // Get the player's rigidbody
            playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The player object is missing, HUD disabled");
            // If the inventory is missing, delete the item pickup behaviour and exit this function
            Destroy(this);
            return;
        }

        // If all of the HUD UI exists
        if (GameObject.Find("HUD System") != null && GameObject.Find("Health Slider") != null && GameObject.Find("Velocity Text") != null)
        {
            // Get the HUD transform
            hUDTransform = GameObject.Find("HUD System").transform;
            // Get the health slider
            healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
            // Set the slider value to the player's max value
            healthSlider.maxValue = playerBehaviour.MaxHealth;

            // Get the velocity text
            velocityText = GameObject.Find("Velocity Text").GetComponent<Text>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The HUD UI is missing, HUD disabled");
            Destroy(this);
            return;
        }

        // If the notification object is assigned
        if(notificationTextPrefab != null)
        {
            // Get the notification text prefab height and speed
            float notificationTextHeight = notificationTextPrefab.GetComponent<RectTransform>().rect.size.y;
            float notificationTextSpeed = notificationTextPrefab.GetComponent<NotificationTextBehaviour>().ScrollSpeed;
            // Calculate the max time between notifications
            maxQueueTime = (notificationTextHeight / notificationTextSpeed);
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The notification prefab is not assigned, HUD disabled");
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the slider value based on the player's current health
        if (playerBehaviour != null)
            healthSlider.value = playerBehaviour.CurrentHealth;
        else
            healthSlider.value = 0f;

        // Update the velocity text based on the player's current velocity
        if (playerRigidbody != null)
            velocityText.text = Mathf.Round(playerRigidbody.velocity.magnitude).ToString();
        else
            velocityText.text = "0";

        // If the queue time has passed
        if (currentQueueTime <= 0)
        {
            // If there is something in the queue
            if (notificationTextQueue.Count > 0)
            {
                // Create a new notification text from the first entry in the queue
                InstantiateNotificationText(notificationTextQueue[0]);
                // Remove the first entry from the queue
                notificationTextQueue.RemoveAt(0);
                // Reset the queue time
                currentQueueTime = maxQueueTime;
            }
        }
        // Else, decrease the queue time
        else
            currentQueueTime -= Time.unscaledDeltaTime;
    }

    // Adds a notification string to the queue
    public void AddNotificationString(string notificationString)
    {
        notificationTextQueue.Add(notificationString);
    }

    // Instantiates a notification text from a string
    void InstantiateNotificationText(string notificationString)
    {
        // Instantiate a new notification text object
        GameObject notificationText = Instantiate(notificationTextPrefab);
        // Change the new notification text's text string
        notificationText.GetComponent<Text>().text = " - " + notificationString;
        // Parent the HUD transform to the new notification text
        notificationText.transform.SetParent(hUDTransform);
    }
}
