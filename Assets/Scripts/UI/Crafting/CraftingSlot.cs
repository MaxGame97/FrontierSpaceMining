using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    private CraftingIngredient[] craftingIngredients;

    private Item resultItem;

    private CraftingTooltip tooltip;

    private Inventory inventory;

    private HUD hUD;

    public CraftingIngredient[] CraftingIngredients { get { return craftingIngredients; } set { craftingIngredients = value; } }
    public Item ResultItem { get { return resultItem; } set { resultItem = value; } }

	// Use this for initialization
	void Start () {
        tooltip = GameObject.Find("Crafting Controller").GetComponent<CraftingTooltip>();
        inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
        hUD = GameObject.Find("HUD Controller").GetComponent<HUD>();
	}

    // When the mouse has entered the item bounds
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate an update the tooltip to show this item's data
        tooltip.Activate(craftingIngredients, inventory);
    }

    // When the mouse has exited the item bounds
    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate the tooltip
        tooltip.Deactivate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!inventory.CheckIfEmptySlot())
        {
            hUD.AddNotificationString("Inventory is full");

            return;
        }

        for(int i = 0; i < craftingIngredients.Length; i++)
        {
            if (inventory.CheckItemCount(craftingIngredients[i].iD) < craftingIngredients[i].amount)
            {
                hUD.AddNotificationString("Unable to craft '" + resultItem.Name + "'");

                return;
            }
        }

        for (int i = 0; i < craftingIngredients.Length; i++)
        {
            inventory.RemoveItem(craftingIngredients[i].iD, craftingIngredients[i].amount);
        }

        inventory.AddItem(resultItem.ID);

        tooltip.UpdateText(craftingIngredients, inventory);

        hUD.AddNotificationString("'" + resultItem.Name + "' Crafted");

        if (false)
        {
            // Deactivate the tooltip
            tooltip.Deactivate();

            Destroy(gameObject);
        }
    }
}
