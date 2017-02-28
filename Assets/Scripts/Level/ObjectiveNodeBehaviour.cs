using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveNodeBehaviour : MonoBehaviour {

    [SerializeField] private List<GameObject> gameObjectsToSpawn = new List<GameObject>();

    [SerializeField] private List<GameObject> gameObjectsToInstantiate = new List<GameObject>();

    [SerializeField] private bool destroyOnTrigger = false;

    private LevelController levelController;
    
	void Awake () {
        // If a level controller exists
        if(GameObject.Find("Level Controller") != null)
        {
            // Set the level controller instance
            levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();
        }
        else
        {
            // Throw an error message, destroy this gameObject and quit this function
            Debug.LogError("No level controller object found, deactivating objective nodes");
            Destroy(gameObject);
            return;
        }

        // Go through the GameObject list
	    for(int i = 0; i < gameObjectsToSpawn.Count; i++)
        {
            // Deactivate all GameObjects in the list
            gameObjectsToSpawn[i].SetActive(false);
        }
	}
	
    // Is called when the GameObject is collides with a trigger collider
	void OnTriggerEnter2D(Collider2D collision)
    {
        // If the objective is triggered by the player, and is allowed to be destroyed on trigger
        if(collision.gameObject.tag == "Player" && destroyOnTrigger)
        {
            // Destroy this GameObject
            Destroy(gameObject);
        }
    }

    // Is called when the GameObject is destroyed
    void OnDestroy()
    {
        // Activate all stored GameObjects
        ActivateGameObjects();
        // Activate the next objective
        levelController.NextObjective();
    }

    // Activates all GameObjects in the list
    void ActivateGameObjects()
    {
        // Go through the to spawn GameObject list
        for (int i = 0; i < gameObjectsToSpawn.Count; i++)
        {
            // Activate all GameObjects in the list
            gameObjectsToSpawn[i].SetActive(true);
        }

        // Go through the to instantiate GameObject list
        for (int i = 0; i < gameObjectsToInstantiate.Count; i++)
        {
            // Instantiate all GameObjects in the list
            Instantiate(gameObjectsToInstantiate[i]);
        }
    }
}
