using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour
{

    [SerializeField] private int iD; // Contains the item ID

    public int ID { get { return iD; } }

    // Use this for initialization
    void Start()
    {
        // Change the sprite to match the item ID
        GetComponent<SpriteRenderer>().sprite = GameObject.Find("Inventory").GetComponent<ItemDatabase>().FetchItemFromID(iD).Sprite;

        // TODO - Resize to appropriate size
    }
}
