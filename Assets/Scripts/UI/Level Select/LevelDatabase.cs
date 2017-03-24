using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class LevelDatabase : MonoBehaviour {

    private List<LevelData> database = new List<LevelData>();   // The level database
    private JsonData levelData;                                 // The level .json data

    private GlobalGameControllerBehaviour globalGameController; // Reference to the global game controller

    public List<LevelData> Database { get { return database; } }

	// Use this for initialization
	void Start () {
        // Get the level data from the .json file
        levelData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/LevelData.json"));

        // If the global game controller exists
        if (GameObject.Find("Global Game Controller") != null)
            // Get the global game controller
            globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();

        // Create the level database
        CreateDatabase();
        Debug.Log(database.Count);
    }

    // Creates the level database (SHOULD ONLY BE RUN ONCE)
    void CreateDatabase()
    {
        // Check all level entries in the Json data
        for (int i = levelData.Count - 1; i >= 0; i--)
        {
            // If there is a global game controller
            if (globalGameController == null)
            {
                // Add a new level to the database, the level data is taken from the Json data
                database.Add(new LevelData((int)levelData[i]["iD"], (string)levelData[i]["levelName"], (string)levelData[i]["description"], (string)levelData[i]["scene"], false));
            }
            else
            {
                // Temporary bool used to specify if the levels completed requirements were met
                bool requirementsMet = false;

                // If there are no required levels to complete, the requirements are met
                if (levelData[i]["requiredLevelsCompleted"].Count == 0)
                    requirementsMet = true;
                
                // Check all of the level's requirements
                for (int j = 0; j < levelData[i]["requiredLevelsCompleted"].Count; j++)
                {
                    // Check all of the currently completed levels
                    for (int k = 0; k < globalGameController.CurrentCompletedLevels.Count; k++)
                    {
                        // If any of the currently completed levels matches the current checked level requirement
                        if ((int)levelData[i]["requiredLevelsCompleted"][j] == globalGameController.CurrentCompletedLevels[k])
                        {
                            // This requirement is met, stop checking this requirement
                            requirementsMet = true;
                            break;
                        }
                    }

                    // If the previous requirement was not met, stop checking the requirements
                    if (!requirementsMet)
                        break;
                }

                // If the requirements were met
                if (requirementsMet)
                {
                    // Temporary bool used to specify whether or not the current level is completed or not
                    bool completed = false;
                    
                    // Check all the currently completed levels
                    for (int j = 0; j < globalGameController.CurrentCompletedLevels.Count; j++)
                    {
                        // If any of them match the current level
                        if ((int)levelData[i]["iD"] == globalGameController.CurrentCompletedLevels[j])
                        {
                            // The current level has been completed, stop checking
                            completed = true;
                            break;
                        }
                    }

                    // Add a new level to the database, the level data is taken from the Json data
                    database.Add(new LevelData((int)levelData[i]["iD"], (string)levelData[i]["levelName"], (string)levelData[i]["description"], (string)levelData[i]["scene"], completed));
                } 
            }
        }
    }
}

public class LevelData
{
    private int iD;             // The level data's ID
    private string name;        // The level's name
    private string description; // The description of the level
    private string scene;       // The the level's scene-name
    private bool completed;

    public int ID { get { return iD; } }
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public string Scene { get { return scene; } }
    public bool Completed { get { return completed; } }

    // Constructor for creating level data
    public LevelData(int iD, string name, string description, string scene, bool completed)
    {
        this.iD = iD;
        this.name = name;
        this.description = description;
        this.scene = scene;
        this.completed = completed;
    }
}
