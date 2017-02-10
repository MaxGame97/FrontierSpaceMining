using UnityEngine;
using System.Collections;

public class ItemPickupBehaviour : MonoBehaviour {

    private Inventory inventory;    // The player's inventory

    private HUD hUD;                // The HUD

    [SerializeField] private bool isEnabled = true;

    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }

    // Use this for initialization
    void Start()
    {
        // If the inventory exists
        if (GameObject.Find("Inventory Controller") != null)
        {
            // Get the player's inventory
            inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The inventory is missing, item pickup disabled");
            // If the inventory is missing, delete the item pickup behaviour and exit this function
            Destroy(this);
            return;
        }

        // If the inventory exists
        if (GameObject.Find("HUD Controller") != null)
        {
            // Get the player's inventory
            hUD = GameObject.Find("HUD Controller").GetComponent<HUD>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("The HUD is missing, item pickup disabled");
            // If the HUD is missing, delete the item pickup behaviour and exit this function
            Destroy(this);
            return;
        }
    }

        void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player is colliding with an item
        if (collision.gameObject.tag == "Item" && isEnabled)
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

        // Send a message to the HUD
        hUD.AddNotificationString("'" + item.Name + "' Picked up");
    }
}
