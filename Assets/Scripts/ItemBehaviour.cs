using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour
{

    [SerializeField] private int iD; // Contains the item ID

    private string itemName;

    public int ID { get { return iD; } }
    public string Name { get { return itemName; } }

    // Use this for initialization
    void Start()
    {
        // Change the sprite to match the item ID
        GetComponent<SpriteRenderer>().sprite = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>().FetchItemFromID(iD).Sprite;

        itemName = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>().FetchItemFromID(iD).Name;

        // TODO - Resize to appropriate size
    }
}
