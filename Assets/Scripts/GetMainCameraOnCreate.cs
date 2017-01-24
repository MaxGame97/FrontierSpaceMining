using UnityEngine;
using System.Collections;

public class GetMainCameraOnCreate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Finds the main camera and makes it follow this object's transform
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>().ChangeTrackingPointTransform(transform);
    }
}
