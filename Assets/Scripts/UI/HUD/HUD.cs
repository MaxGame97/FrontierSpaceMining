using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

    [SerializeField] private GameObject notificationTextPrefab; // The notification text prefab object

    [Space(6f)]

    [SerializeField] private bool forceAlternativeStyle = false;

    [SerializeField] private float healthNeedleRange = 82f;

    List<string> notificationTextQueue = new List<string>();    // List of strings (a queue of strings) that will be displayed as notifications

    private float maxQueueTime;                                 // The max queue time (in seconds)
    private float currentQueueTime = 0f;                        // The current queue time (in seconds)

    //private Transform hUDTransform;                             // The transform of the HUD canvas
    private Transform notificationTextPanelTransform;           // The transform of the notification text panel

    private GameObject leftCorner;
    private GameObject leftCornerAlt;

    private GameObject healthDisplay;
    private RectTransform healthNeedle;                         // The health needle

    private Text velocityText;                                  // The velocity text UI

    private Rigidbody2D playerRigidbody;                        // The player's rigidbody

    private PlayerBehaviour playerBehaviour;                    // The player's health component

	// Use this for initialization
	void Start () {
        // Get the HUD transform
        //hUDTransform = GameObject.Find("HUD System").transform;
        // Get the notification text panel transform
        notificationTextPanelTransform = GameObject.Find("Notification Text Panel").transform;

        leftCorner = GameObject.Find("Left Corner");
        leftCornerAlt = GameObject.Find("Left Corner Alt");

        // Get the health display
        healthDisplay = GameObject.Find("Health Display");

        // Get the health needle
        healthNeedle = GameObject.Find("Health Needle").GetComponent<RectTransform>();

        // Get the velocity text
        velocityText = GameObject.Find("Velocity Text").GetComponent<Text>();

        // If the player object exists
        if (GameObject.Find("Player") != null)
        {
            // Get the player's health component
            playerBehaviour = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
            // Get the player's rigidbody
            playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();

            leftCornerAlt.SetActive(false);
        }
        // If the player does not exist
        else
        {
            // Disable the HUD elements bound to the player

            leftCorner.SetActive(false);

            healthDisplay.SetActive(false);
            velocityText.gameObject.SetActive(false);

            GameObject.Find("Minimap Camera").SetActive(false);
            GameObject.Find("Minimap").SetActive(false);
            GameObject.Find("Bounds Warning Panel").SetActive(false);
        }

        if (forceAlternativeStyle)
        {
            leftCorner.SetActive(false);
            leftCornerAlt.SetActive(true);
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
        // If the health slider exists
        if(healthNeedle != null)
        {
            // Update the slider value based on the player's current health
            if (playerBehaviour != null)
                healthNeedle.localRotation = Quaternion.Lerp(healthNeedle.localRotation, Quaternion.Euler(0f, 0f, -((playerBehaviour.CurrentHealth / playerBehaviour.MaxHealth) - 0.5f) * healthNeedleRange), 0.1f);
        }

        // If the velocity text exists
        if(velocityText != null)
        {
            // Update the velocity text based on the player's current velocity
            if (playerRigidbody != null)
                velocityText.text = Mathf.Round(playerRigidbody.velocity.magnitude).ToString();
            else
                velocityText.text = "0";
        }

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
        notificationText.transform.SetParent(notificationTextPanelTransform, false);
    }
}
