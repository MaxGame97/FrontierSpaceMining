using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    [SerializeField] private GameObject inventorySlotPrefab;    // The slot prefab
    [SerializeField] private GameObject inventoryItemPrefab;    // The item prefab
    [SerializeField] private int inventorySlotCount = 20;       // The amount of slots in the inventory

    private ItemDatabase itemDatabase;                          // The item database

    private GameObject inventoryPanel;                          // The inventory UI panel (unneccesary?)
    private GameObject slotPanel;                               // The slot panel

    public List<Item> items = new List<Item>();                 // List containing all the items in the inventory
    public List<GameObject> slots = new List<GameObject>();     // List containing all the slots in the inventory

	// Use this for initialization
	void Start () {
        itemDatabase = GetComponent<ItemDatabase>();            // Get the item database component

        inventoryPanel = GameObject.Find("Inventory Panel");    // Find the inventory panel (unneccesary?)
        slotPanel = GameObject.Find("Slot Panel");              // Find the slot panel

        for(int i = 0; i < inventorySlotCount; i++)             // Loops as many times as there are slots in the inventory
        {
            items.Add(new Item());                              // Add an empty item

            slots.Add(Instantiate(inventorySlotPrefab));        // Instantiate a slot prefab
            slots[i].transform.SetParent(slotPanel.transform);  // Parent the slot panel to the slot
            slots[i].GetComponent<Slot>().SlotID = i;           // Assign the slot an ID
        }

        AddItem(0);
        AddItem(2);
    }

    // Adds an item to the inventory
    public void AddItem(int iD)
    {
        // Fetches the item by its ID and places it in a temporary item variable
        Item tempItem = itemDatabase.FetchItemFromID(iD);

        // If the item to be added is stackable, and if it exists in the inventory
        if (tempItem.Stackable && IfItemExistsInInventory(tempItem))
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
        // Else, if either the item to be added isn't stackable, or if the item doesn't exist in the inventory
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

                    // If the item is stackable
                    if (tempItem.Stackable)
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
    public bool RemoveItem(int iD, int amount)
    {
        // Fetches the item by its ID and places it in a temporary item variable
        Item tempItem = itemDatabase.FetchItemFromID(iD);

        // If the item exists in the inventory
        if (IfItemExistsInInventory(tempItem))
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
                        // If the item is stackable and there will be more than one of them left after being removed
                        if (tempItem.Stackable && itemData.Count - amount > 0)
                        {
                            // Decrease the item count by the specified amount
                            itemData.Count -= amount;
                            // Update the item count text
                            itemData.transform.GetChild(0).GetComponent<Text>().text = itemData.Count.ToString();

                            // Return true, removing the items succeeded
                            return true;
                        }
                        else
                        {
                            // Change the item to be an empty item
                            items[i] = new Item();

                            // Destroy the item object
                            Destroy(slots[i].transform.GetChild(0).gameObject);

                            // Return true, removing the item(s) succeeded
                            return true;
                        }
                    }
                }
            }
        }

        // If no item was removed, return false, removing the item(s) did not succeed
        return false;
    }

    // Returns true if a specified item already exists in the inventory
    bool IfItemExistsInInventory(Item item)
    {
        // Loops through the item list
        for (int i = 0; i < items.Count; i++)
        {
            // If an item with the same ID exists
            if (items[i].ID == item.ID)
            {
                // Return true, there is an item with the same ID in the inventory
                return true;
            }
        }

        // If no item was found, return false
        return false;
    }
}
