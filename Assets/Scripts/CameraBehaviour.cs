using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    private Transform trackingPointTransform;                       // The current transform tha the camera is tracking
    private Vector3 trackingPointPosition = new Vector3();          // The latest position that the camera was tracking

    [SerializeField] [Range(0.01f, 0.5f)] private float maxSpeed;   // Max speed that the camera can move at

    private float viewportHeight;                                   // Vertical width (in units) that the camera is able to view
    private float viewportWidth;                                    // Horizontal width (in units) that the camera is able to view

    public float ViewportWidth { get { return viewportWidth; } }
    public float ViewportHeight { get { return viewportHeight; } }

	// Use this for initialization
	void Awake () {
        Camera camera = GetComponent<Camera>();         // Get the camera component

        viewportHeight = camera.orthographicSize;       // Get the camera's viewport height (in units)
        viewportWidth = viewportHeight * camera.aspect; // Get the camera's viewport width (in units), using the camera's aspect ratio
	}
	
	// FixedUpdate is called once per physics tick
	void FixedUpdate () {
        // If the transform that the camera is tracking exists
        if (trackingPointTransform != null)
            // Update the current tracking position
            trackingPointPosition = trackingPointTransform.position;
        
        // Track the current tracking position
        TrackPosition(trackingPointPosition);
    }

    void TrackPosition(Vector3 targetPosition)
    {
        // Create a temporary Vector3
        Vector3 newPosition = transform.position;

        // Interpolate between the current position and the tracking position
        newPosition.x = Mathf.Lerp(newPosition.x, targetPosition.x, maxSpeed);
        newPosition.y = Mathf.Lerp(newPosition.y, targetPosition.y, maxSpeed);

        // Update the camera's position
        transform.position = newPosition;
    }

    // Public function that changes the transform that the camera is tracking
    // Use this to change the camera's "focus"
    public void ChangeTrackingPointTransform(Transform newTrackingPointTransform)
    {
        trackingPointTransform = newTrackingPointTransform;
    }
}
