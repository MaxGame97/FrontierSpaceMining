using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour {
    [SerializeField] private string sceneToLoad;                                    // Scene which we intend to load into, specifically for this scipt the Hub is currently the main transition

    private Inventory inventory;                                                    // The player's inventory
    private GameObject hubTransition;                                               // Gameobject to instantiate
    private List<HubItems> hubInventory = new List<HubItems>();

    public List<HubItems> HubInventory { get { return hubInventory; } }             // List of the new temporary inventory

    void Awake()
    {
        hubTransition = gameObject;                                                 // Find the hub transition GameObject
        DontDestroyOnLoad(hubTransition);                                           // Make sure we do not destroy this GameObject when we load a new scene
    }

    void Start ()
    {
        GameObject go = GameObject.Find("Inventory Controller");                    // Find the inventoryController component
        inventory = go.GetComponent<Inventory>();                                   // Make an instance of the Inventory

        for (int i = 0; i < inventory.Items.Count; i++)                             // For every item in the players inventory
        {
            if (inventory.Items[i].ID != -1)                                        // Make sure it exists in the database
            {
                HubItems hubItem;                                                   // Instance of the new struct

                hubItem.iD = inventory.Items[i].ID;                                 // Add the players inventory to the hubInventory
                hubItem.amount = inventory.CheckItemCount(inventory.Items[i].ID);

                hubInventory.Add(hubItem);
            }
        }

        SceneManager.LoadScene(sceneToLoad);                                        // Load the new scene
    }

    // Struct needed for the hub transition
    public struct HubItems
    {
        public int iD;
        public int amount;
    }
}
