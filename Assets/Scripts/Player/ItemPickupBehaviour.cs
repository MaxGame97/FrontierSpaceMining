using UnityEngine;
using System.Collections;

public class ItemPickupBehaviour : MonoBehaviour {

    private Inventory inventory; // The player's inventory

    // Use this for initialization
    void Start()
    {
        // Get the player's inventory
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

        void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player is colliding with an item
        if (collision.gameObject.tag == "Item")
        {
            // Send the item to the inventory
            ItemPickup(collision.gameObject);

            // Destory the item instance
            Destroy(collision.gameObject);
        }
    }

    void ItemPickup(GameObject itemObject)
    {
        // Get the item behaviour from the collided item
        ItemBehaviour item = itemObject.GetComponent<ItemBehaviour>();

        // Add the item to the inventory, by its ID
        inventory.AddItem(item.ID);
    }
}
