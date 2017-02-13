using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public CraftingIngredient[] craftingIngredients;

    CraftingTooltip tooltip;

	// Use this for initialization
	void Start () {
        tooltip = GameObject.Find("Crafting Controller").GetComponent<CraftingTooltip>();
	}

    // When the mouse has entered the item bounds
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate an update the tooltip to show this item's data
        tooltip.Activate(craftingIngredients);
    }

    // When the mouse has exited the item bounds
    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate the tooltip
        tooltip.Deactivate();
    }
}
