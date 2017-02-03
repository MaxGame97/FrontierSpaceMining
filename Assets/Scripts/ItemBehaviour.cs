using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour
{

    [SerializeField] private int iD; // Contains the item ID

    private string name;

    public int ID { get { return iD; } }
    public string Name { get { return name; } }

    // Use this for initialization
    void Start()
    {
        // Change the sprite to match the item ID
        GetComponent<SpriteRenderer>().sprite = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>().FetchItemFromID(iD).Sprite;

        name = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>().FetchItemFromID(iD).Name;

        // TODO - Resize to appropriate size
    }
}
