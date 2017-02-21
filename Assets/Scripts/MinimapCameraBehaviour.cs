using UnityEngine;
using System.Collections;

public class MinimapCameraBehaviour : MonoBehaviour {

    private Transform target;                                               // The enemy's target (the player)
                                                                            // Use this for initialization
    void Start () {

        target = GameObject.FindGameObjectWithTag("Player").transform;          // Find the player's transform component
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(target.position.x, target.position.y, -10);
	}
}
