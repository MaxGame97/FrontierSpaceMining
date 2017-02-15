using UnityEngine;
using System.Collections;

public class MineableBehaviour : MonoBehaviour {

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] [Range(1, 25)] private int amount;

    // Called when the mineable object is being mined
    public void Mine(Quaternion miningDirection, Vector2 miningPoint)
    {
        // Instantiate a mined item on the mining point, with the same rotation as the mining laser
        GameObject minedItem = (GameObject)Instantiate(itemPrefab, miningPoint, miningDirection);

        // Rotate the mined item randomly within 50 degrees
        minedItem.transform.Rotate(0f, 0f, Random.Range(-25f, 25f));
        // Add a force in the backwards direction (makes the mined item start moving away from the mining point)
        minedItem.GetComponent<Rigidbody2D>().AddForce(10f * -minedItem.transform.up);
        // Randomly rotate the item within 360 degrees
        minedItem.transform.Rotate(0f, 0f, Random.Range(0f, 360f));

        // Decrease the amount of mineable items
        amount--;

        // If all items have been mined, destroy this gameobject
        if (amount == 0)
            Destroy(gameObject);
    }
}
