using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class _SplashImage : MonoBehaviour {

    [SerializeField] [Range(1f, 10f)] private float timer = 3f;             // The time it takes until the splash image starts to fade
    [SerializeField] [Range(0.01f, 1f)] private float fadeSpeed = 0.05f;    // The fade speed

    private SpriteRenderer spriteRenderer;                                  // The splash image's sprite renderer component
	
    void Start()
    {
        // Get the spriterenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Reset the object's scale
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Get the original object size
        Vector3 originalSize = spriteRenderer.bounds.extents;

        Vector2 backgroundScale = new Vector2();

        // Calculate the scale neccesary to fit the viewport
        backgroundScale.x = Camera.main.GetComponent<CameraBehaviour>().ViewportWidth / originalSize.x;
        backgroundScale.y = Camera.main.GetComponent<CameraBehaviour>().ViewportHeight / originalSize.y;

        if(backgroundScale.x < backgroundScale.y)
        {
            // Scale the object to fit the viewport
            transform.localScale = new Vector3(backgroundScale.x, backgroundScale.x, 1f);
        }
        else
        {
            // Scale the object to fit the viewport
            transform.localScale = new Vector3(backgroundScale.y, backgroundScale.y, 1f);
        }
    }

	// Update is called once per frame
	void Update () {
        if(timer < 0)
        {
            if (spriteRenderer.color.r < 0.01f)
                SceneManager.LoadScene("Main Menu");
            else
                spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(0f, 0f, 0f, 1f), fadeSpeed);
        }
        else
            timer -= Time.unscaledDeltaTime;
	}
}
