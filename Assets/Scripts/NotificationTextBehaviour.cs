using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationTextBehaviour : MonoBehaviour {

    [SerializeField] [Range(5f, 25f)] private float scrollSpeed = 15f;
    [SerializeField] [Range(0.1f, 1f)] private float decayRate = 0.5f;
    [SerializeField] [Range(1f, 5f)] private float lifeTime = 1.5f;

    private float alpha;

    private Text text;

    public float ScrollSpeed { get { return scrollSpeed; } }

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();

        alpha = text.color.a;
    }

    // Update is called once per frame
    void Update ()
    {
        transform.Translate(0f, scrollSpeed * Time.unscaledDeltaTime, 0f);
        
        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0)
        {
            alpha -= decayRate * Time.unscaledDeltaTime;

            if (alpha > 0)
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            else
                Destroy(gameObject);
        }
	}
}
