using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    private CraftingIngredient[] craftingIngredients;   // The required ingredients in the hovered crafting recipe

    private Item resultItem;                            // The resulting item in the hovered crafting recipe

    private CraftingTooltip tooltip;                    // The tooltip panel's tooltip component

    private Inventory inventory;                        // The player's inventory

    private HUD hUD;                                    // The HUD controller

    public CraftingIngredient[] CraftingIngredients { get { return craftingIngredients; } set { craftingIngredients = value; } }
    public Item ResultItem { get { return resultItem; } set { resultItem = value; } }

	// Use this for initialization
	void Start () {
        // If the crafting system exists
        if (GameObject.Find("Crafting Controller") != null)
        {
            // Get the tooltip component from the crafting system
            tooltip = GameObject.Find("Crafting Controller").GetComponent<CraftingTooltip>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The crafting system is missing, crafting slot disabled");
            Destroy(this);
            return;
        }

        // If the inventory system exists
        if (GameObject.Find("Inventory Controller") != null)
        {
            // Get the inventory component from the inventory system
            inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The inventory system is missing, crafting slot disabled");
            Destroy(this);
            return;
        }

        // If the HUD system exists
        if (GameObject.Find("HUD Controller") != null)
        {
            // Get the HUD component from the HUD system
            hUD = GameObject.Find("HUD Controller").GetComponent<HUD>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The HUD system is missing, crafting slot disabled");
            Destroy(this);
            return;
        }
	}

    // When the mouse has entered the item bounds
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate an update the tooltip to show this item's data
        tooltip.Activate(craftingIngredients, inventory, resultItem);
    }

    // When the mouse has exited the item bounds
    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate the tooltip
        tooltip.Deactivate();
    }

    // When the mouse is pressed on the crafting slot
    public void OnPointerDown(PointerEventData eventData)
    {
        // If the mouse button pressed was the left mouse button
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // If there is no more room in the inventory
            if (!inventory.CheckIfEmptySlot() && inventory.CheckItemCount(resultItem.ID) == 0)
            {
                // Add a notification string showing that the inventory is full, and exit this function
                hUD.AddNotificationString("Inventory is full");

                return;
            }

            // If there is room in the inventory

            // Check all the crafting ingredients
            for (int i = 0; i < craftingIngredients.Length; i++)
            {
                // If there are not enough items in the inventory to meet the crafting requirements
                if (inventory.CheckItemCount(craftingIngredients[i].iD) < craftingIngredients[i].amount)
                {
                    // Add a notification string showing that the recipe was unable to be crafted, and exit this function
                    hUD.AddNotificationString("Unable to craft '" + resultItem.Name + "'");

                    return;
                }
            }

            // If the crafting requirements are met

            // Remove the amount of items
            for (int i = 0; i < craftingIngredients.Length; i++)
            {
                inventory.RemoveItem(craftingIngredients[i].iD, craftingIngredients[i].amount);
            }

            // Add the crafted item to the inventory
            inventory.AddItem(resultItem.ID);

            // Update the tooltip text to match the current inventory status
            tooltip.UpdateText(craftingIngredients, inventory, resultItem);

            // Add a notification string showing that the item was crafted
            hUD.AddNotificationString("'" + resultItem.Name + "' Crafted");
        }
    }
}
