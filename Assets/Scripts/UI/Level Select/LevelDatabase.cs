using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class LevelDatabase : MonoBehaviour {

    private List<LevelData> database = new List<LevelData>();
    private JsonData levelData;

    public List<LevelData> Database { get { return database; } }

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
        for (int i = 0; i < levelData.Count; i++)
        {
            // Add a new level to the database, the level data is taken from the Json data
            database.Add(
                new LevelData(
                    (int)levelData[i]["iD"],
                    (string)levelData[i]["levelName"],
                    (string)levelData[i]["description"],
                    (string)levelData[i]["scene"]
                )
            );
        }
    }
}

public class LevelData
{
    private int iD;             // The level data's ID
    private string name;        // The level's name
    private string description; // The description of the level
    private string scene;       // The the level's scene-name

    public int ID { get { return iD; } }
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public string Scene { get { return scene; } }

    // Constructor for creating level data
    public LevelData(int iD, string name, string description, string scene)
    {
        this.iD = iD;
        this.name = name;
        this.description = description;
        this.scene = scene;
    }
}
