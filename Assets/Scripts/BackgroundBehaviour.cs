using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
        
        Vector3 originalSize = GetComponent<SpriteRenderer>().bounds.extents;

        float scaleX = mainCamera.GetComponent<CameraBehaviour>().ViewportWidth / originalSize.x;
        float scaleY = mainCamera.GetComponent<CameraBehaviour>().ViewportHeight / originalSize.y;

        if (scaleX > scaleY)
            transform.localScale = new Vector3(scaleX, scaleX, 1f);
        else
            transform.localScale = new Vector3(scaleY, scaleY, 1f);
    }
	
	void Update () {
        Vector3 newPosition = transform.position;

        newPosition.x = mainCamera.transform.position.x;
        newPosition.y = mainCamera.transform.position.y;

        transform.position = newPosition;
	}
}
