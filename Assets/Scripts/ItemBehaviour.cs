using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour
{

    [SerializeField] private int iD; // Contains the item ID

    public int ID { get { return iD; } }

    // Use this for initialization
    void Start()
    {
        // Change the sprite to match the item
        GetComponent<SpriteRenderer>().sprite = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>().FetchItemFromID(iD).Sprite;

        // Change the name of the gameobject to match the item
        gameObject.name = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>().FetchItemFromID(iD).Name + " (Item)";
    }
}
