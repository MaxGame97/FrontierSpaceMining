using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    [SerializeField] private GameObject inventorySlotPrefab;    // The slot prefab
    [SerializeField] private GameObject inventoryItemPrefab;    // The item prefab
    [SerializeField] private int inventorySlotCount = 20;       // The amount of slots in the inventory

    private ItemDatabase itemDatabase;                          // The item database

    private CanvasGroup canvas;                                 // Contains the inventory canvas's CanvasGroup
    private GameObject slotPanel;                               // The slot panel

    private bool inventoryEnabled = true;                       // Specifies whether or not the inventory is enabled

    private List<Item> items = new List<Item>();                // List containing all the items in the inventory
    private List<GameObject> slots = new List<GameObject>();    // List containing all the slots in the inventory

    public List<Item> Items { get { return items; } set { items = value; } }
    public List<GameObject> Slots { get { return slots; } set { slots = value; } }

    public bool InventoryEnabled { get { return inventoryEnabled; } }

	// Use this for initialization
	void Start () {
        // If all inventory UI exists
        if (GameObject.Find("Inventory System") != null && GameObject.Find("Slot Panel") != null)
        {
            canvas = GameObject.Find("Inventory System").GetComponent<CanvasGroup>();   // Get the canvas's CanvasGroup
            slotPanel = GameObject.Find("Slot Panel");                                  // Find the slot panel
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the inventory UI is missing, inventory system disabled");
            Destroy(this);
            return;
        }

        itemDatabase = GetComponent<ItemDatabase>();            // Get the item database component

        for (int i = 0; i < inventorySlotCount; i++)            // Loops as many times as there are slots in the inventory
        {
            items.Add(new Item());                              // Add an empty item
            
            slots.Add(Instantiate(inventorySlotPrefab));        // Instantiate a slot prefab
            slots[i].transform.SetParent(slotPanel.transform);  // Parent the slot panel to the slot
            slots[i].GetComponent<Slot>().SlotID = i;           // Assign the slot an ID
        }

        GameObject hubTransition = GameObject.FindGameObjectWithTag("Hub Transition");

        // If a transition into the Hub exist, then we save the inventory
        if (hubTransition != null)
        {
            LevelTransition existingInventory;                                      // Get the leveltransition component

            existingInventory = hubTransition.GetComponent<LevelTransition>();      // Get the existing HubInventory 

            for (int i = 0; i < existingInventory.HubInventory.Count; i++)          // For every item in the hubInventory, we...
            {
                for (int j = 0; j < existingInventory.HubInventory[i].amount; j++)  // For every unit of every item in the hubInventory, we...
                {
                    AddItem(existingInventory.HubInventory[i].iD);                  // We add to the new player inventory
                }
            }

            Destroy(hubTransition);                                                 // Destroy the hubTransition prefab
        }

        // Hide the inventory panel as default
        ToggleInventoryPanel();
    }

    // Adds an item to the inventory
    public void AddItem(int iD)
    {
        // Fetches the item by its ID and places it in a temporary item variable
        Item tempItem = itemDatabase.FetchItemFromID(iD);

        // If the item exists in the inventory
        if (CheckItemCount(tempItem.ID) > 0)
        {
            // Loop through the item list
            for (int i = 0; i < items.Count; i++)
            {
                // If the item with the same ID is found
                if (items[i].ID == tempItem.ID)
                {
                    // Get the item data from the found item
                    ItemData itemData = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    
                    itemData.Count++;                                                                       // Increase the item count by one
                    itemData.transform.GetChild(0).GetComponent<Text>().text = itemData.Count.ToString();   // Update the text component (displays the current count of this item)

                    // Break the loop, there is no need to keep looping
                    break;
                }
            }
        }
        // If it doesn't
        else
        {
            // Loop through the item list
            for (int i = 0; i < items.Count; i++)
            {
                // If an empty item is found
                if (items[i].ID == -1)
                {
                    // Changes the empty item to the added item
                    items[i] = tempItem;

                    GameObject tempItemObject = Instantiate(inventoryItemPrefab);               // Create an item object prefab
                    tempItemObject.GetComponent<Image>().sprite = tempItem.Sprite;              // Change the item object's sprite to match the item
                    tempItemObject.transform.SetParent(slots[i].transform);                     // Parent the slot to the item object
                    tempItemObject.GetComponent<RectTransform>().localPosition = Vector3.zero;  // Zero the item object's position relative to the slot

                    // Get the item data from the item object
                    ItemData itemData = tempItemObject.GetComponent<ItemData>();

                    itemData.Item = tempItem;   // Assign the item to the item data
                    itemData.SlotID = i;        // Assign the slot ID to the item data
                    itemData.Count++;           // Increase the item count in the item data
                    
                    // Update and show the item count text
                    itemData.transform.GetChild(0).GetComponent<Text>().text = itemData.Count.ToString();

                    // Change the item object's name in the inspector
                    tempItemObject.name = tempItem.Name + " (Item)";

                    // Break the loop, there is no need to keep looping
                    break;
                }
            }
        }
    }

    // Removes a specified amount of an item from the inventory
    public void RemoveItem(int iD, int amount)
    {
        // Fetches the item by its ID and places it in a temporary item variable
        Item tempItem = itemDatabase.FetchItemFromID(iD);

        // If the item exists in the inventory
        if (CheckItemCount(tempItem.ID) > 0)
        {
            // Loop through the item list
            for (int i = 0; i < items.Count; i++)
            {
                // If the item with the same ID is found
                if (items[i].ID == tempItem.ID)
                {
                    // Get the item data from the item object
                    ItemData itemData = slots[i].transform.GetChild(0).GetComponent<ItemData>();

                    // If there is enough of the specified item to be removed
                    if (itemData.Count >= amount)
                    {
                        // If there will be more than one of them left after being removed
                        if (itemData.Count - amount > 0)
                        {
                            // Decrease the item count by the specified amount
                            itemData.Count -= amount;
                            // Update the item count text
                            itemData.transform.GetChild(0).GetComponent<Text>().text = itemData.Count.ToString();

                            // Exit the loop
                            break;
                        }
                        else
                        {
                            // Change the item to be an empty item
                            items[i] = new Item();

                            // Destroy the item object
                            Destroy(slots[i].transform.GetChild(0).gameObject);

                            // Exit the loop
                            break;
                        }
                    }
                }
            }
        }
    }

    // Toggles the inventory panel
    public void ToggleInventoryPanel()
    {
        if (inventoryEnabled)
        {
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
            inventoryEnabled = false;
        }
        else
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            inventoryEnabled = true;
        }
    }

    // Returns the amount of items in the inventory
    public int CheckItemCount(int iD)
    {
        // Loops through the item list
        for (int i = 0; i < items.Count; i++)
        {
            // If an item with the same ID exists
            if (items[i].ID == iD)
            {
                // Get the item data from the item object
                ItemData itemData = slots[i].transform.GetChild(0).GetComponent<ItemData>();

                // Return the amount of items
                return itemData.Count;
            }
        }

        // If no item was found, return zero
        return 0;
    }

    // Returns true if there is an empty slot in the inventory
    public bool CheckIfEmptySlot()
    {
        // Loops through the item list
        for (int i = 0; i < items.Count; i++)
        {
            // If the item is an empty item (ID = -1)
            if (items[i].ID == -1)
            {
                // There is an empty slot in the inventory
                return true;
            }
        }

        // There are no empty slots in the inventory
        return false;
    }
    // Struct needed for the transition into the Hub
    public struct HubItems
    {
        public int iD;
        public int amount;
    }
}
