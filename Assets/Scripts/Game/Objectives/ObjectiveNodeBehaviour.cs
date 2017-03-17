using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveNodeBehaviour : MonoBehaviour {

    [SerializeField] private List<GameObject> gameObjectsToActivate = new List<GameObject>();
    [SerializeField] private List<GameObject> gameObjectsToDeactivate = new List<GameObject>();
    [SerializeField] private List<GameObject> gameObjectsToInstantiate = new List<GameObject>();

    [SerializeField] private GameObject soundFXPrefab;

    [SerializeField] private AudioClip audioClip;

    [Space(6f)]

    [SerializeField] private bool startObjectsActive = false;
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

        // If the assigned objects to activate shouldn't be kept active
        if (!startObjectsActive)
        {
            // Go through the GameObject list
            for (int i = 0; i < gameObjectsToActivate.Count; i++)
            {
                // Deactivate all GameObjects in the list
                gameObjectsToActivate[i].SetActive(false);
            }
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
        // If the GameObject has been disabled
        if (!enabled)
        {
            // Activate all stored GameObjects
            ActivateGameObjects();
            // Activate the next objective
            levelController.NextObjective();
        }
    }

    // Activates all GameObjects in the list
    void ActivateGameObjects()
    {
        // Go through the to spawn GameObject list
        for (int i = 0; i < gameObjectsToActivate.Count; i++)
        {
            if(gameObjectsToActivate[i] != null)
                // Activate all GameObjects in the list
                gameObjectsToActivate[i].SetActive(true);
        }

        // Go through the to spawn GameObject list
        for (int i = 0; i < gameObjectsToDeactivate.Count; i++)
        {
            if(gameObjectsToDeactivate[i] != null)
                // Deactivate all GameObjects in the list
                gameObjectsToDeactivate[i].SetActive(false);
        }

        // Go through the to instantiate GameObject list
        for (int i = 0; i < gameObjectsToInstantiate.Count; i++)
        {
            // Instantiate all GameObjects in the list
            Instantiate(gameObjectsToInstantiate[i]);
        }

        if(audioClip != null)
        {
            GameObject soundFX = (GameObject)Instantiate(soundFXPrefab, transform.position, new Quaternion());

            soundFX.GetComponent<AudioSource>().clip = audioClip;
        }
    }
}