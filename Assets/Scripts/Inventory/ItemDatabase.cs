using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    private List<Item> database = new List<Item>(); // Database of all items
    private JsonData itemData;                      // Json data of all items

	// Use this for initialization
	void Start () {
        // Get the item data from the .json file
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        // Create the item database
        CreateDatabase();
    }

    // Creates the item database (SHOULD ONLY BE RUN ONCE)
    void CreateDatabase()
    {
        // Check all item entries in the Json data
        for(int i = 0; i < itemData.Count; i++)
        {
            // Add a new item, the item data is taken from the Json data
            database.Add(
                new Item(
                    (int)itemData[i]["iD"],
                    (string)itemData[i]["name"],
                    (string)itemData[i]["description"],
                    (int)itemData[i]["value"],
                    (int)itemData[i]["weight"],
                    (bool)itemData[i]["stackable"],
                    (string)itemData[i]["slug"]
                )
            );
        }
    }

    // Fetches an item from th database, from its ID
    public Item FetchItemFromID(int iD)
    {
        // Loop through the item database
        for(int i = 0; i < database.Count; i++)
        {
            // If the current item ID matches the given ID
            if(database[i].ID == iD)
            {
                // Return the current item
                return database[i];
            }
        }

        // If no item is found, return an empty item
        return new Item();
    }
}

public class Item
{
    private int iD;             // The item's unique ID
    private string name;        // The item's name
    private string description; // The item's description
    private int value;          // The item's value
    private int weight;         // The item's weight
    private bool stackable;     // Specifies whether or not the item is stackable
    private string slug;        // The item's slug name (used for finding the item's sprite)
    private Sprite sprite;      // The item's sprite

    public int ID { get { return iD; } set { iD = value; } }
    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public int Value { get { return value; } set { this.value = value; } }
    public int Weight { get { return weight; } set { weight = value; } }
    public bool Stackable { get { return stackable; } set { stackable = value; } }
    public string Slug { get { return slug ; } set { slug = value; } }
    public Sprite Sprite { get { return sprite; } set { sprite = value; } }

    // Constructor for a complete item
    public Item(int iD, string name, string description, int value, int weight, bool stackable, string slug)
    {
        this.iD = iD;
        this.name = name;
        this.description = description;
        this.value = value;
        this.weight = weight;
        this.stackable = stackable;
        this.slug = slug;

        // Gets the item's sprite from its slug name
        sprite = Resources.Load<Sprite>("Inventory/Sprites/" + slug);
    }

    // Constructor for an empty item
    public Item()
    {
        // Sets the ID to -1 (this means there is no item data)
        iD = -1;
    }
}
