using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    private List<Item> database = new List<Item>();
    private JsonData itemData;

	// Use this for initialization
	void Start () {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));

        CreateDatabase();
    }

    void CreateDatabase()
    {
        for(int i = 0; i < itemData.Count; i++)
        {
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

    public Item FetchItemFromID(int iD)
    {
        for(int i = 0; i < database.Count; i++)
        {
            if(database[i].ID == iD)
            {
                return database[i];
            }
        }
        return null;
    }
}

public class Item
{
    private int iD;
    private string name;
    private string description;
    private int value;
    private int weight;
    private bool stackable;
    private string slug;
    private Sprite sprite;

    public int ID { get { return iD; } set { iD = value; } }
    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public int Value { get { return value; } set { this.value = value; } }
    public int Weight { get { return weight; } set { weight = value; } }
    public bool Stackable { get { return stackable; } set { stackable = value; } }
    public string Slug { get { return slug ; } set { slug = value; } }
    public Sprite Sprite { get { return sprite; } set { sprite = value; } }

    public Item(int iD, string name, string description, int value, int weight, bool stackable, string slug)
    {
        this.iD = iD;
        this.name = name;
        this.description = description;
        this.value = value;
        this.weight = weight;
        this.stackable = stackable;
        this.slug = slug;

        sprite = Resources.Load<Sprite>("Inventory/Sprites/" + slug);
    }

    public Item()
    {
        iD = -1;
    }
}
