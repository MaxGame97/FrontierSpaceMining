using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    private Transform trackingPointTransform;                                       // The current transform tha the camera is tracking
    private Rigidbody2D trackingPointRigidbody = null;                              // The tracked transform's eventual rigidbody
    private Vector3 trackingPointPosition = new Vector3();                          // The latest position that the camera was tracking

    [SerializeField] [Range(0.01f, 0.5f)] private float maxSpeed;                   // Max speed that the camera can move at
    [SerializeField] [Range (0f, 2f)] private float predictMovementFactor = 0.75f;

    private float viewportHeight;                                                   // Vertical width (in units) that the camera is able to view
    private float viewportWidth;                                                    // Horizontal width (in units) that the camera is able to view

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
        {
            // If the tracked transform has a rigidbody
            if(trackingPointRigidbody != null)
            {
                // Update the current tracking position, shift it in the direction of travel
                trackingPointPosition = trackingPointTransform.position + (Vector3)(trackingPointRigidbody.velocity * predictMovementFactor);
            }
            // If the tracked transform does not have a rigidbody
            else
            {
                // Update the current tracking position
                trackingPointPosition = trackingPointTransform.position;
            }
        }
        
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
        // Update the tracked transform
        trackingPointTransform = newTrackingPointTransform;

        // If the transform followed has a rigidbody, update it
        if (newTrackingPointTransform.GetComponent<Rigidbody2D>() != null)
            trackingPointRigidbody = newTrackingPointTransform.GetComponent<Rigidbody2D>();
        // If not, set it to null
        else
            trackingPointRigidbody = null;
    }
}
