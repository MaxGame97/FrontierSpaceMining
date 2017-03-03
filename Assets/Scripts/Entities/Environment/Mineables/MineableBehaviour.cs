using UnityEngine;
using System.Collections;

public class MineableBehaviour : MonoBehaviour {

    [SerializeField] private GameObject objectPrefab;                           // The object to spawn when the object is mined
    [SerializeField] [Range(1, 25)] private int amount;                         // The amount of objects to spawn
    [SerializeField] [Range(0.25f, 5f)] private float maxMiningInterval = 0.3f; // The max interval between spawned objects (in seconds)

    private float currentMiningInterval; // The current inverval between spawned objects (in seconds)

    void Start()
    {
        // Set the current mining inverval to be the max value
        currentMiningInterval = maxMiningInterval;
    }

    // Called when the mineable object is being mined
    public void Mine(Quaternion miningDirection, Vector2 miningPoint)
    {
        // Decrease the current mining interval
        currentMiningInterval -= Time.deltaTime;

        // If the current mining inverval has expired
        if(currentMiningInterval < 0)
        {
            // Instantiate a mined item on the mining point, with the same rotation as the mining laser
            GameObject minedItem = (GameObject)Instantiate(objectPrefab, miningPoint, miningDirection);

            // Rotate the mined item randomly within 50 degrees
            minedItem.transform.Rotate(0f, 0f, Random.Range(-25f, 25f));
            // Add a force in the backwards direction (makes the mined item start moving away from the mining point)
            minedItem.GetComponent<Rigidbody2D>().AddForce(10f * -minedItem.transform.up);
            // Increase the mined item's rotation speed (makes it spin when mined)
            minedItem.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-5f, 5f));
            // Randomly rotate the item within 360 degrees
            minedItem.transform.Rotate(0f, 0f, Random.Range(0f, 360f));

            // Decrease the amount of mineable items
            amount--;

            // Reset the current mining interval to the max interval
            currentMiningInterval = maxMiningInterval;

            // If all items have been mined, destroy this gameobject
            if (amount == 0)
                Destroy(gameObject);
        }
    }
}
