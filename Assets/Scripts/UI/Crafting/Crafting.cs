using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crafting : MonoBehaviour {

    [SerializeField] private GameObject craftingSlotPrefab; // The crafting slot prefab object

    private CraftingRecipeDatabase craftingRecipeDatabase;  // The crafting recipe database

    private CanvasGroup canvasGroup;                        // The crafting system's canvas group
    private GameObject craftingSlotPanel;                   // The crafting slot panel object

    private bool craftingEnabled = true;                    // Specifies whether or not the crafting system is enabled

    public bool CraftingEnabled { get { return craftingEnabled; } }

	// Use this for initialization
	void Start () {
        // If all crafting UI exists
        if (GameObject.Find("Crafting System") != null && GameObject.Find("Crafting Slot Panel") != null)
        {
            canvasGroup = GameObject.Find("Crafting System").GetComponent<CanvasGroup>();   // Get the canvas's canvas group
            craftingSlotPanel = GameObject.Find("Crafting Slot Panel");                     // Get the crafting slot panel
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the crafting UI is missing, crafting system disabled");
            Destroy(this);
            return;
        }

        // Get the crafting recipe database component
        craftingRecipeDatabase = GetComponent<CraftingRecipeDatabase>();

        // Add each of the crafting recipes from the crafting recipe database
        for(int i = 0; i < craftingRecipeDatabase.Database.Count; i++)
        {
            AddCraftingRecipe(i);
        }

        // Toggle (hide) the crafting panel as default
        ToggleCraftingPanel();
    }

    // Add a crafting recipe to the crafting slot panel, by its ID
    public void AddCraftingRecipe(int iD)
    {
        // Fetch the crafting recipe from the ID
        CraftingRecipe recipe = craftingRecipeDatabase.FetchCraftingRecipeFromID(iD);
        
        GameObject craftingSlot = (GameObject)Instantiate(craftingSlotPrefab, craftingSlotPanel.transform); // Instantiate a crafting slot object, parent it to the crafting slot panel
        craftingSlot.GetComponent<CraftingSlot>().CraftingIngredients = recipe.CraftingIngredients;         // Set the crafting slot's crafting ingredients value to the recipe's
        craftingSlot.GetComponent<CraftingSlot>().ResultItem = recipe.ResultItem;                           // Set the crafting slot's resulting item to the recipe's
        craftingSlot.transform.GetChild(0).GetComponent<Image>().sprite = recipe.Sprite;                    // Set the sprite of the crafting slot to the resulting item's
        craftingSlot.transform.GetChild(1).GetComponent<Text>().text = recipe.ResultItem.Name;              // Set the text UI of the crafting slot to the resulting item's

        // Set the crafting slot's local position to zero
        craftingSlot.transform.localPosition = Vector2.zero;
    }

    // Toggles the crafting panel
    public void ToggleCraftingPanel()
    {
        if (craftingEnabled)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            craftingEnabled = false;
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            craftingEnabled = true;
        }
    }
}
