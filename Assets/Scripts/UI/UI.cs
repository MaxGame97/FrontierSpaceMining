using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    private Inventory inventory;        // The current inventory system
    private Crafting crafting;          // The current crafting system
    private LevelSelect levelSelect;    // The current level select system
    private PauseMenu pauseMenu;        // The current pause menu system

    [SerializeField] private string toggleInventoryInput;   // String representing the input used to toggle the inventory system
    [SerializeField] private string toggleCraftingInput;    // String representing the input used to toggle the crafting system
    [SerializeField] private string toggleLevelSelectInput; // String representing the input used to toggle the level select system
    [SerializeField] private string togglePauseMenuInput;   // String representing the input used to toggle the pause menu

    [SerializeField] private bool levelSelectEnabled = false;

    // Use this for initialization
    void Start () {
        // If the required UI exists, get the UI components
        if (GameObject.Find("Inventory Controller") != null && GameObject.Find("Crafting Controller") != null && GameObject.Find("Level Select Controller") != null && GameObject.Find("Pause Menu Controller") != null)
        {
            inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
            crafting = GameObject.Find("Crafting Controller").GetComponent<Crafting>();
            levelSelect = GameObject.Find("Level Select Controller").GetComponent<LevelSelect>();
            pauseMenu = GameObject.Find("Pause Menu Controller").GetComponent<PauseMenu>();
        }
        else
        {
            // Throw an error message to the debug log and destroy this script
            Debug.LogError("Some or all of the UI is missing, UI system disabled");
            Destroy(this);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
        // If the pause menu system is disabled
        if (!pauseMenu.PauseMenuEnabled)
        {
            // If the inventory button is pressed, toggle the inventory system
            if (Input.GetButtonDown(toggleInventoryInput))
                ToggleInventorySystem();

            // If the crafting button is pressed, toggle the crafting system
            if (Input.GetButtonDown(toggleCraftingInput))
                ToggleCraftingSystem();

            // If the level select button is pressed, toggle the level select system
            if (levelSelectEnabled)
                if (Input.GetButtonDown(toggleLevelSelectInput))
                    ToggleLevelSelectSystem();

            // If the pause menu button is pressed, toggle the pause menu system
            if (Input.GetButtonDown(togglePauseMenuInput))
                TogglePauseMenuSystem();
        }
        else
        // Else, if the pause button is pressed, toggle the pause menu system
            if (Input.GetButtonDown(togglePauseMenuInput))
            TogglePauseMenuSystem();
    }

    // Toggles the inventory system
    public void ToggleInventorySystem()
    {
        // Toggle the inventory panel
        inventory.ToggleInventoryPanel();

        // If the crafting panel is enabled, toggle it
        if (crafting.CraftingEnabled)
            crafting.ToggleCraftingPanel();

        // If the level select system is enabled, toggle it
        if (levelSelectEnabled)
            if (levelSelect.LevelSelectEnabled)
                levelSelect.ToggleLevelSelectPanel();

        // If the pause menu panel is enabled, toggle it
        if (pauseMenu.PauseMenuEnabled)
            pauseMenu.TogglePauseMenuPanel();
    }

    // Toggles the crafting system
    public void ToggleCraftingSystem()
    {
        // Toggle the crafting panel
        crafting.ToggleCraftingPanel();

        // If the inventory panel is enabled, toggle it
        if (inventory.InventoryEnabled)
            inventory.ToggleInventoryPanel(); ;

        // If the level select panel is enabled, toggle it
        if (levelSelectEnabled)
            if (levelSelect.LevelSelectEnabled)
                levelSelect.ToggleLevelSelectPanel();

        // If the pause menu panel is enabled, toggle it
        if (pauseMenu.PauseMenuEnabled)
            pauseMenu.TogglePauseMenuPanel();
    }

    // Toggles the level select system
    public void ToggleLevelSelectSystem()
    {
        // Toggle the level select panel
        levelSelect.ToggleLevelSelectPanel();

        // If the inventory panel is enabled, toggle it
        if (inventory.InventoryEnabled)
            inventory.ToggleInventoryPanel(); ;

        // If the crafting panel is enabled, toggle it
        if (crafting.CraftingEnabled)
            crafting.ToggleCraftingPanel();

        // If the pause menu panel is enabled, toggle it
        if (pauseMenu.PauseMenuEnabled)
            pauseMenu.TogglePauseMenuPanel();
    }

    // Toggles the pause menu system
    public void TogglePauseMenuSystem()
    {
        // Toggle the pause menu panel
        pauseMenu.TogglePauseMenuPanel();

        // If the inventory panel is enabled, toggle it
        if (inventory.InventoryEnabled)
            inventory.ToggleInventoryPanel(); ;

        // If the crafting panel is enabled, toggle it
        if (crafting.CraftingEnabled)
            crafting.ToggleCraftingPanel();

        // If the level select panel is enabled, toggle it
        if (levelSelectEnabled)
            if (levelSelect.LevelSelectEnabled)
                levelSelect.ToggleLevelSelectPanel();
    }
}
