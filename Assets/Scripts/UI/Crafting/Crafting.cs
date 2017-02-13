using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crafting : MonoBehaviour {

    [SerializeField] private GameObject craftingRecipePrefab;

    private CraftingRecipeDatabase craftingRecipeDatabase;

    private CanvasGroup canvas;
    private GameObject craftingRecipePanel;

    private bool craftingEnabled = true;

    //private List<CraftingRecipe> craftingRecipies = new List<CraftingRecipe>();

    public bool CraftingEnabled { get { return craftingEnabled; } }

	// Use this for initialization
	void Start () {
        // If all crafting UI exists
        if (GameObject.Find("Crafting System") != null && GameObject.Find("Crafting Recipe Panel") != null)
        {
            canvas = GameObject.Find("Crafting System").GetComponent<CanvasGroup>();    // Get the canvas's CanvasGroup
            craftingRecipePanel = GameObject.Find("Crafting Recipe Panel");             // Get the crafting recipe panel
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the crafting UI is missing, crafting system disabled");
            Destroy(this);
            return;
        }

        craftingRecipeDatabase = GetComponent<CraftingRecipeDatabase>();

        for(int i = 0; i < craftingRecipeDatabase.Database.Count; i++)
        {
            AddCraftingRecipe(i);
        }

        ToggleCraftingPanel();
    }

    public void AddCraftingRecipe(int iD)
    {
        CraftingRecipe recipe = craftingRecipeDatabase.FetchCraftingRecipeFromID(iD);

        GameObject craftingRecipe = (GameObject)Instantiate(craftingRecipePrefab, craftingRecipePanel.transform);
        craftingRecipe.transform.GetChild(0).GetComponent<Image>().sprite = recipe.Sprite;
        craftingRecipe.transform.GetChild(1).GetComponent<Text>().text = recipe.Name;
        craftingRecipe.transform.localPosition = Vector2.zero;
    }

    // Toggles the crafting panel
    public void ToggleCraftingPanel()
    {
        if (craftingEnabled)
        {
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
            craftingEnabled = false;
        }
        else
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            craftingEnabled = true;
        }
    }
}
