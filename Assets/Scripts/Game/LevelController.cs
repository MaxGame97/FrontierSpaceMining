using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

    [SerializeField] private List<GameObject> objectiveGameObjects = new List<GameObject>();

    private int objectiveIndex = 0;

	// Use this for initialization
	void Start () {
        // If no objective game objects were assigned
        if (objectiveGameObjects.Count > 0)
        {
            for (int i = 1; i < objectiveGameObjects.Count; i++)
            {
                objectiveGameObjects[i].SetActive(false);
            }
        }
        else
        {
            // Throw an error message, destroy this gameObject and quit this function
            Debug.LogWarning("No objective GameObjects assigned");
            Destroy(gameObject);
            return;
        }
	}

    // Function that activates the next objective game object
    public void NextObjective()
    {
        // If the objective index is lower than the max
        if(objectiveIndex < objectiveGameObjects.Count - 1)
        {
            // Increase it
            objectiveIndex++;

            // Activate the objective game object on this index
            objectiveGameObjects[objectiveIndex].SetActive(true);
        }
    }
}
