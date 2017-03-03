using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour {

    private Inventory inventory;                                                    // The player's inventory

    private List<HubItems> hubInventory = new List<HubItems>();

    public List<HubItems> HubInventory { get { return hubInventory; } }             // List of the new temporary inventory

    void Awake()
    {
        DontDestroyOnLoad(gameObject);                                              // Make sure we do not destroy this GameObject when we load a new scene
    }

    public void Start()
    {
        // Get the player's inventory
        inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();

        // For every item in the players inventory
        for (int i = 0; i < inventory.Items.Count; i++)                             
        {
            // If the item ID is not -1 (empty item)
            if (inventory.Items[i].ID != -1)
            {
                // Create a temporary hub item instance
                HubItems hubItem;

                // Set the hub item's ID and amount based on the inventory
                hubItem.iD = inventory.Items[i].ID;
                hubItem.amount = inventory.CheckItemCount(inventory.Items[i].ID);

                // Add the hub item to the hub item list
                hubInventory.Add(hubItem);
            }
        }
        
        // Load the hub scene
        SceneManager.LoadScene("Hub");

        // If the global game controller exists
        if(GameObject.Find("Global Game Controller") != null)
        {
            // Get the global game controller behaviour
            GlobalGameControllerBehaviour gameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();

            int levelID = GameObject.Find("Level Info(Clone)").GetComponent<LevelInfo>().LevelID;
            
            // Check the currently completed levels list
            for (int i = 0; i < gameController.CurrentCompletedLevels.Count; i++)
            {
                // If the current level ID is there, exit this function
                if (gameController.CurrentCompletedLevels[i] == levelID)
                {
                    Destroy(GameObject.Find("Level Info(Clone)"));
                    return;
                }
            }

            // If not, add the current level ID to the completed levels list
            gameController.CurrentCompletedLevels.Add(levelID);

            

            Destroy(GameObject.Find("Level Info(Clone)"));
        }
    }

    // Struct needed for the hub transition
    public struct HubItems
    {
        public int iD;
        public int amount;
    }
}
