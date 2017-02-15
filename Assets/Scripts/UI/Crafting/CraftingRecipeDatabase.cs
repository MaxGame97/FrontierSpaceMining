using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class CraftingRecipeDatabase : MonoBehaviour {

    private ItemDatabase itemDatabase;

    private List<CraftingRecipe> database = new List<CraftingRecipe>(); // Database of all crafting recipes
    private JsonData craftingRecipeData;                                // Json data of all crafting recipes

    public List<CraftingRecipe> Database { get { return database; } }

    // Use this for initialization
    void Start()
    {
        itemDatabase = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>();

        // Get the crafting recipe data from the .json file
        craftingRecipeData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/CraftingRecipes.json"));
        // Create the crafting recipe database
        CreateDatabase();
    }

    // Creates the crafting recipe database (SHOULD ONLY BE RUN ONCE)
    void CreateDatabase()
    {
        // Check all crafting recipe entries in the Json data
        for (int i = 0; i < craftingRecipeData.Count; i++)
        {
            List<CraftingIngredient> craftingIngredients = new List<CraftingIngredient>();

            for (int j = 0; j < craftingRecipeData[i]["recipe"].Count; j++)
            {
                craftingIngredients.Add(new CraftingIngredient((int)craftingRecipeData[i]["recipe"][j]["iD"], (int)craftingRecipeData[i]["recipe"][j]["amount"]));
            }

            // Add a new crafting recipe, the recipe data is taken from the Json data
            database.Add(
                new CraftingRecipe(
                    itemDatabase,
                    (int)craftingRecipeData[i]["iD"],
                    (int)craftingRecipeData[i]["itemID"],
                    craftingIngredients.ToArray()
                )
            );
        }
    }

    // Fetches a crafting recipe from the database, from its ID
    public CraftingRecipe FetchCraftingRecipeFromID(int iD)
    {
        // Loop through the recipe database
        for (int i = 0; i < database.Count; i++)
        {
            // If the current recipe ID matches the given ID
            if (database[i].ID == iD)
            {
                // Return the current recipe
                return database[i];
            }
        }

        // If no recipe is found, return null
        return null;
    }
}

public class CraftingRecipe
{
    private int iD;                                     // The recipe's unique ID
    private Item resultItem;                            // The item that the recipe is creating
    private CraftingIngredient[] craftingIngredients;   // An array containing the recipe's ingredients
    private string slug;                                // The recipe's slug name (used for finding the recipe's sprite)
    private Sprite sprite;                              // The recipe's sprite

    public int ID { get { return iD; } }
    public Item ResultItem { get { return resultItem; } }
    public CraftingIngredient[] CraftingIngredients { get { return craftingIngredients; } }
    public Sprite Sprite { get { return sprite; } }

    // Constructor for a crafting recipe
    public CraftingRecipe(ItemDatabase itemDatabase, int iD, int itemID, CraftingIngredient[] craftingIngredients)
    {
        this.iD = iD;
        this.resultItem = itemDatabase.FetchItemFromID(itemID);
        this.craftingIngredients = craftingIngredients;

        // Gets the recipe's sprite from its slug name
        sprite = resultItem.Sprite;

        // If the sprite is null (missing), load a debug image instead
        if (sprite == null)
            sprite = Resources.Load<Sprite>("Items/Sprites/missing_image");
    }
}

public struct CraftingIngredient
{
    public int iD;      // The ID of the required item
    public int amount;  // The amount of the required item

    public CraftingIngredient(int iD, int amount)
    {
        this.iD = iD;
        this.amount = amount;
    }
}
