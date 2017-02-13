using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class CraftingRecipeDatabase : MonoBehaviour {

    private List<CraftingRecipe> database = new List<CraftingRecipe>(); // Database of all crafting recipes
    private JsonData craftingRecipeData;                                // Json data of all crafting recipes

    public List<CraftingRecipe> Database { get { return database; } }

    // Use this for initialization
    void Start()
    {
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
            // Add a new crafting recipe, the recipe data is taken from the Json data
            database.Add(
                new CraftingRecipe(
                    (int)craftingRecipeData[i]["iD"],
                    (string)craftingRecipeData[i]["name"],
                    (int)craftingRecipeData[i]["itemID"],
                    (string)craftingRecipeData[i]["slug"]
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
    private int iD;             // The recipe's unique ID
    private string name;        // The recipe's name
    private int itemID;         // The recipe's result, or its contained item
    private string slug;        // The recipe's slug name (used for finding the recipe's sprite)
    private Sprite sprite;      // The recipe's sprite

    public int ID { get { return iD; } set { iD = value; } }
    public string Name { get { return name; } set { name = value; } }
    public int ItemID { get { return itemID; } set { itemID = value; } }
    public string Slug { get { return slug; } set { slug = value; } }
    public Sprite Sprite { get { return sprite; } set { sprite = value; } }

    // Constructor for a crafting recipe
    public CraftingRecipe(int iD, string name, int itemID, string slug)
    {
        this.iD = iD;
        this.name = name;
        this.itemID = itemID;
        this.slug = slug;

        // Gets the recipe's sprite from its slug name
        sprite = Resources.Load<Sprite>("UI/Crafting/Sprites/" + slug);

        // If the sprite is null (missing), load a debug image instead
        if (sprite == null)
            sprite = Resources.Load<Sprite>("UI/Crafting/Sprites/missing_image");
    }
}
