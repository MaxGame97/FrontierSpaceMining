using UnityEngine;
using System.Collections;

public class ItemPickupBehaviour : MonoBehaviour {

    void TempItemPickupFunction(GameObject item)
    {
        // TODO - Add logic here when inventory system exists
    }

	void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player is colliding with an item
        if (collision.gameObject.tag == "Item")
        {
            // Send the item to the inventory
            TempItemPickupFunction(collision.gameObject);

            // Destory the item instance
            Destroy(collision.gameObject);
        }
    }
}
