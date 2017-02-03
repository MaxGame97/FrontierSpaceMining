using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

    private Camera mainCamera;          // Contains the main camera instance

    private Vector2 backgroundScale;    // Vector2 defining the background scale

    public Vector2 BackgroundScale { get { return backgroundScale; } }

	// Use this for initialization
	void Start () {
        // Get the main camera
        mainCamera = Camera.main;

        // Reset the object's scale
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Get the original object size
        Vector3 originalSize = GetComponent<MeshRenderer>().bounds.extents;

        // Calculate the scale neccesary to fit the viewport
        backgroundScale.x = mainCamera.GetComponent<CameraBehaviour>().ViewportWidth / originalSize.x;
        backgroundScale.y = mainCamera.GetComponent<CameraBehaviour>().ViewportHeight / originalSize.y;
        
        // Scale the object to fit the viewport
        transform.localScale = new Vector3(backgroundScale.x, backgroundScale.y, 1f);
    }
	
	void Update () {
        Vector3 newPosition = transform.position;

        // Center the background to the camera's position
        newPosition.x = mainCamera.transform.position.x;
        newPosition.y = mainCamera.transform.position.y;

        // Move the background to the new position
        transform.position = newPosition;
	}
}
