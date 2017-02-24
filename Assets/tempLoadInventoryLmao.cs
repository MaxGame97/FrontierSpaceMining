using UnityEngine;
using System.Collections;

public class tempLoadInventoryLmao : MonoBehaviour {

	// Use this for initialization
	void Start() {
        if (GameObject.Find("Global Game Controller") != null)
        {
            SaveLoadGame globalGameController = GameObject.Find("Global Game Controller").GetComponent<SaveLoadGame>();
            
            globalGameController.LoadCurrentSaveIndex();
        }

        Destroy(gameObject);
	}
}
