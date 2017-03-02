using UnityEngine;
using System.Collections;

public class LoadSavedInventory : MonoBehaviour {

	// Use this for initialization
	void Start() {
        if (GameObject.Find("Global Game Controller") != null)
        {
            GlobalGameControllerBehaviour globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();
            
            globalGameController.UpdateCurrentInventory();
        }

        Destroy(gameObject);
	}
}
