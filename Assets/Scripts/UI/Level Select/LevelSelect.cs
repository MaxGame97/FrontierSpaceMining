using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    [SerializeField] private GameObject levelSlotPrefab;        // Prefab for the level slot object
    [SerializeField] private GameObject levelTransitionPrefab;  // Prefab for the level transition object

    private LevelDatabase levelDatabase;                        // The level database

    private CanvasGroup canvasGroup;                            // The level select system's canvas group
    private GameObject levelSlotPanel;                          // The level slot panel

    private LevelData currentSelectedLevel = null;              // The currently selected level

    [SerializeField] private bool levelSelectEnabled = true;                     // Specifies whether or not the level select system is enabled

    public bool LevelSelectEnabled { get { return levelSelectEnabled; } }
    public LevelData CurrentSelectedLevel { get { return currentSelectedLevel; } }


	// Use this for initialization
	void Start () {
        // If all level select UI exists
        if(GameObject.Find("Level Select System") !=  null && GameObject.Find("Level Slot Panel") != null)
        {
            canvasGroup = GameObject.Find("Level Select System").GetComponent<CanvasGroup>();   // Get the level select system's canvas group
            levelSlotPanel = GameObject.Find("Level Slot Panel");                               // Get the level slot panel object
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the level select UI is missing, level select system disabled");
            Destroy(this);
            return;
        }

        // Gets the level database component
        levelDatabase = GetComponent<LevelDatabase>();

        // Add each level from the level database
        for(int i = 0; i < levelDatabase.Database.Count; i++)
        {
            AddLevelSlot(levelDatabase.Database[i]);
        }

        // If there is at least one entry in the level database, set the first one as the currently selected level
        if(levelDatabase.Database.Count > 0)
            UpdateCurrentSelectedLevel(levelDatabase.Database[0]);

        // Toggle (hide) the level select panel as default
        ToggleLevelSelectPanel();
	}

    // Adds a new level slot
    void AddLevelSlot(LevelData levelData)
    {
        // Instantiates a level slot object and parents it to the level slot panel
        GameObject levelSlotObject = (GameObject)Instantiate(levelSlotPrefab, levelSlotPanel.transform);

        // Get the level slot component and assign the correct level to it
        LevelSlot levelSlot = levelSlotObject.GetComponent<LevelSlot>();
        levelSlot.Level = levelData;

        if(levelData.Completed)
            levelSlotObject.GetComponent<Image>().color = new Color(0.5f, 1f, 0.5f, 1f);

        // Updates the level slot object's text to show the level name
        levelSlotObject.transform.GetChild(0).GetComponent<Text>().text = levelData.Name;
    }

    // Updates the currently selected level
    public void UpdateCurrentSelectedLevel(LevelData levelData)
    {
        // Changes the currently selected level variable
        currentSelectedLevel = levelData;

        // Update the level info text to show the level name and description
        GameObject.Find("Level Info Text").GetComponent<Text>().text = "<size=15>" + levelData.Name + "</size>\n" + levelData.Description;
    }

    // Loads the currently selected level
    public void LoadCurrentSelectedLevel()
    {
        // If a level is currently selected
        if (currentSelectedLevel != null)
        {
            // If there is a global game controller
            if (GameObject.Find("Global Game Controller") != null)
            {
                // Get the global game controller behaviour
                GlobalGameControllerBehaviour globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();
                
                // Save the game
                globalGameController.Save();
            }

            // Instantiate the level transition object
            GameObject levelTransition = (GameObject)Instantiate(levelTransitionPrefab);

            // Set the level transition object's level ID to the currently selected level's ID
            levelTransition.GetComponent<LevelInfo>().LevelID = currentSelectedLevel.ID;
            
            // Load the currently selected level
            SceneManager.LoadScene(currentSelectedLevel.Scene);
        }
    }

    // Toggles the level select panel
    public void ToggleLevelSelectPanel()
    {
        if (levelSelectEnabled)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            levelSelectEnabled = false;
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            levelSelectEnabled = true;
        }
    }
}
