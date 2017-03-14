using UnityEngine;
using System.Collections;

public class AutoDespawnBehaviour : MonoBehaviour {

    [SerializeField] private float despawnTimer;        // The time in seconds until the gameObject despawns


	void Update ()
    {
        // Reduce the value of despawnTimer by time.deltaTime
        despawnTimer -= Time.deltaTime;
        // If the value of despawnTimer is zero or less
        if (despawnTimer <= 0)
        {
            // Destroy this gameObject
            Destroy(gameObject);
        }
	}
}
