using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    private Vector3 trackingPoint;

    [SerializeField] [Range(1.0f, 10.0f)]
    private float maxSpeed;

    private float viewportWidth;
    private float viewportHeight;

    public float ViewportWidth { get { return viewportWidth; } }
    public float ViewportHeight { get { return viewportHeight; } }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(trackingPoint != null)
        {
            TrackPosition(trackingPoint);
        }
        else if (player != null)
        {
            TrackPosition(player.transform.position);
        }
	}

    void TrackPosition(Vector3 targetPosition)
    {
        Vector3 newPosition = transform.position;

        newPosition.x = Mathf.Lerp(newPosition.x, targetPosition.x, maxSpeed);
        newPosition.y = Mathf.Lerp(newPosition.y, targetPosition.y, maxSpeed);

        transform.position = newPosition;
    }
}
