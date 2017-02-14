using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler {

    private Inventory inventory;    // The player's iventory

    private int slotID;             // The slot's ID number

    public int SlotID { get { return slotID; } set { slotID = value; } }

    // Use this for initialization
    void Start()
    {
        // Get the player's inventory
        inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
    }

    // When an item is dropped on the slot
    public void OnDrop(PointerEventData eventData)
    {
        // Get the item data from the dropped item
        ItemData itemData = eventData.pointerDrag.GetComponent<ItemData>();

        // If the item is dropped on an empty slot
        if (inventory.Items[slotID].ID == -1)
        {
            inventory.Items[itemData.SlotID] = new Item();  // Clear the item from the previous slot
            inventory.Items[SlotID] = itemData.Item;        // Add the dropped item to this slot

            itemData.SlotID = slotID;                       // Update the item's slot ID
        }
        // Else, if the item is dropped on an occupied slot
        else if (transform.childCount > 0)
        {
            // This part of the code is a little confusing, essentially what it does
            // it switches the position of two items, when one is dropped on the other

            Transform itemTransform = transform.GetChild(0);                                // Get the item's transform from the slot dropped on

            itemTransform.GetComponent<ItemData>().SlotID = itemData.SlotID;                // Update the occupied item's slot ID to the previous slot
            itemTransform.SetParent(inventory.Slots[itemData.SlotID].transform);            // Update the occupied item's parent to the previous slot
            itemTransform.position = inventory.Slots[itemData.SlotID].transform.position;   // Resets the occupied item's position to relative zero

            itemData.SlotID = SlotID;                                                       // Update the dropped item's slot ID to the occupied slot
            itemData.transform.SetParent(transform);                                        // Update the dropped item's parent to the occupied slot
            itemData.transform.position = transform.position;                               // Resets the drópped item's position to relative zero

            // Switch the item's position in the item list
            inventory.Items[itemData.SlotID] = itemTransform.GetComponent<ItemData>().Item;
            inventory.Items[SlotID] = itemData.Item;
        }
    }
}
