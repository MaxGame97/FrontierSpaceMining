using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class LevelDatabase : MonoBehaviour {

    private List<LevelData> database = new List<LevelData>();
    private JsonData levelData;

    private GlobalGameControllerBehaviour globalGameController;

    public List<LevelData> Database { get { return database; } }

    void Awake()
    {
        // If the global game controller exists
        if (GameObject.Find("Global Game Controller") != null)
            // Get the global game controller
            globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();
    }

	// Use this for initialization
	void Start () {
        // Get the level data from the .json file
        levelData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/LevelData.json"));
        
        // Create the level database
        CreateDatabase();
    }

    // Creates the level database (SHOULD ONLY BE RUN ONCE)
    void CreateDatabase()
    {
        // Check all level entries in the Json data
        for (int i = levelData.Count - 1; i >= 0; i--)
        {
            if (globalGameController == null)
            {
                // Add a new level to the database, the level data is taken from the Json data
                database.Add(new LevelData((int)levelData[i]["iD"], (string)levelData[i]["levelName"], (string)levelData[i]["description"], (string)levelData[i]["scene"], false));
            }
            else
            {
                bool requirementsMet = false;

                if (levelData[i]["requiredLevelsCompleted"].Count == 0)
                    requirementsMet = true;

                if (globalGameController.CurrentCompletedLevels.Count > 0)
                {
                    for (int j = 0; j < levelData[i]["requiredLevelsCompleted"].Count; j++)
                    {
                        for (int k = 0; k < globalGameController.CurrentCompletedLevels.Count; k++)
                        {
                            requirementsMet = false;

                            if ((int)levelData[i]["requiredLevelsCompleted"][j] == globalGameController.CurrentCompletedLevels[k])
                            {
                                requirementsMet = true;
                                break;
                            }
                        }
                    }
                }

                if (requirementsMet)
                {
                    bool completed = false;
                    
                    for (int j = 0; j < globalGameController.CurrentCompletedLevels.Count; j++)
                    {
                        if ((int)levelData[i]["iD"] == globalGameController.CurrentCompletedLevels[j])
                        {
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
