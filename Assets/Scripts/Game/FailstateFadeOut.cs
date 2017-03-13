using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FailstateFadeOut : MonoBehaviour {

    [SerializeField] [Range(1f, 10f)] private float timer = 3f;             // The time it takes until the image starts to fade
    [SerializeField] [Range(0.01f, 1f)] private float fadeSpeed = 0.05f;    // The fade speed

    private MeshRenderer meshRenderer;                                      // The image's mesh renderer component
	
    void Start()
    {
        // Get the spriterenderer component
        meshRenderer = GetComponent<MeshRenderer>();

        // Set the mesh renderer's material color to black
        meshRenderer.material.color = new Color(0f, 0f, 0f, 0f);

        // Reset the object's scale
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Get the original object size
        Vector3 originalSize = meshRenderer.bounds.extents;

        Vector2 backgroundScale = new Vector2();

        // Calculate the scale neccesary to fit the viewport
        backgroundScale.x = Camera.main.GetComponent<CameraBehaviour>().ViewportWidth / originalSize.x;
        backgroundScale.y = Camera.main.GetComponent<CameraBehaviour>().ViewportHeight / originalSize.y;

        if(backgroundScale.x > backgroundScale.y)
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
        if(timer < 0f)
        {
            if (meshRenderer.material.color.a >= 0.99f)
                SceneManager.LoadScene("Hub");
            else
                meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, new Color(0f, 0f, 0f, 1f), fadeSpeed * Time.timeScale);
        }
        else
            timer -= Time.deltaTime;
	}
}
