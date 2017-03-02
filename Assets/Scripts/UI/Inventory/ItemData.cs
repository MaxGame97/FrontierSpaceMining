using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    private Inventory inventory;        // The player's inventory
    private InventoryTooltip tooltip;   // The tooltip component

    private Item item;                  // The item stored in the item data
    private int count;                  // The amount of the stored item

    private int slotID;                 // Which slot the item is on

    private Vector2 dragOffset;         // The drag offset when the item is being dragged

	public Item Item { get { return item; } set { item = value; } }
    public int Count { get { return count; } set { count = value; } }
    public int SlotID { get { return slotID; } set { slotID = value; } }

    // Use this for initialization
    void Start()
    {   
        inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();      // Get the player's inventory
        tooltip = GameObject.Find("Inventory Controller").GetComponent<InventoryTooltip>(); // Get the tooltip component
    }

    // When the item is starting to be dragged
    public void OnBeginDrag(PointerEventData eventData)
    {
        // DRAGGING CURRENTLY DISABLED
        return;

        // If the item is not an empty item
        if(Item != null && inventory.InventoryEnabled)
        {
            // Get the drag offset based on the mouse's current position realtive to the item's position
            dragOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);

            transform.SetParent(transform.parent.parent);           // Move the item up to the slot panel instead of being parented to a slot
            transform.position = eventData.position - dragOffset;   // Update the position of the mouse's position, minus the drag offset

            GetComponent<CanvasGroup>().blocksRaycasts = false;     // Allows raycasts to go through the item when dragged
        }
    }

    // When the item is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        // DRAGGING CURRENTLY DISABLED
        return;

        // If the item is not an empty item
        if (Item != null)
        {
            // Update the position of the mouse's position, minus the drag offset
            transform.position = eventData.position - dragOffset;
        }
    }

    // When the item is let go
    public void OnEndDrag(PointerEventData eventData)
    {
        // DRAGGING CURRENTLY DISABLED
        return;

        transform.SetParent(inventory.Slots[slotID].transform);             // Update the item's parent to the slot it's currently in

        transform.position = inventory.Slots[slotID].transform.position;    // Update the item's position to be relative zero

        GetComponent<CanvasGroup>().blocksRaycasts = true;                  // Allows raycasts to detect the item again
    }

    // When the mouse has entered the item bounds
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate an update the tooltip to show this item's data
        tooltip.Activate(item);
    }

    // When the mouse has exited the item bounds
    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate the tooltip
        tooltip.Deactivate();
    }
}
