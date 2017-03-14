using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationTextBehaviour : MonoBehaviour {

    [SerializeField] [Range(5f, 50f)] private float scrollSpeed = 15f;  // The scrolling speed of the notification text
    [SerializeField] [Range(0.1f, 1f)] private float decayRate = 0.5f;  // The decay rate of the notification text
    [SerializeField] [Range(0f, 5f)] private float lifeTime = 1.5f;     // The lifetime of the notification text, before it starts decaying

    private float alpha;                                                // The notification text's current alpha value

    private Text text;                                                  // The notification text's text UI component 

    public float ScrollSpeed { get { return scrollSpeed; } }

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();    // Get the notification text's text UI component

        alpha = text.color.a;           // Get the text UI's alpha
    }

    // Update is called once per frame
    void Update ()
    {
        // Translate the notification text upwards
        transform.Translate(0f, scrollSpeed * Time.unscaledDeltaTime, 0f);
        
        // Decrease the lifetime of the notification
        lifeTime -= Time.unscaledDeltaTime;

        // If the lifetime has run out
        if(lifeTime <= 0)
        {
            // Start decaying the notification by decreasing its alpha value
            alpha -= decayRate * Time.unscaledDeltaTime;

            // If the alpha value is above 0, update the text UI's color
            if (alpha > 0)
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            // If not, destroy the notification
            else
                Destroy(gameObject);
        }
	}
}
