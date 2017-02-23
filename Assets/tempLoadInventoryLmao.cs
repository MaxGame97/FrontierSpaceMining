using UnityEngine;
using System.Collections;

public class tempLoadInventoryLmao : MonoBehaviour {

	// Use this for initialization
	void Start() {
        GameObject.FindGameObjectWithTag("Global Game Controller").GetComponent<SaveLoadGame>().LoadCurrentSaveIndex();
        GameObject.FindGameObjectWithTag("Global Game Controller").GetComponent<SaveLoadGame>().Save();

        Destroy(gameObject);
	}
}
