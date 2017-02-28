using UnityEngine;
using System.Collections;

public class MinimapCameraBehaviour : MonoBehaviour {

    private Transform target; // The minimap's target

    // Use this for initialization                                                                     
    void Start () {
        // If the player object exists
        if(GameObject.Find("Player") != null)
            // Get the player's transform component
            target = GameObject.Find("Player").transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // If the target exists, follow its position
        if (target != null)
            transform.position = new Vector3(target.position.x, target.position.y, -10);
	}
}
