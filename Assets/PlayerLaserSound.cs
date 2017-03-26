using UnityEngine;
using System.Collections;

public class PlayerLaserSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void FixedUpdate()
    {
        transform.GetComponentInChildren<AudioSource>().enabled = transform.GetComponentInParent<LineRenderer>().enabled;
    }
}
