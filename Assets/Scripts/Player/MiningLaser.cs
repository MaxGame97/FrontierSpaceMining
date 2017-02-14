using UnityEngine;
using System.Collections;

public class MiningLaser : MonoBehaviour
{
    [SerializeField] private GameObject tempItem;                   // Temporary item object
    [SerializeField] private GameObject miningParticles;            // Mining particle system
    [SerializeField] [Range(1f, 15f)] private float maxRange = 5;   // Max range of the mining laser
    [SerializeField] [Range(0.1f, 5f)] private float maxSpawnDelay; // Max mining delay (in seconds)

    private float spawnDelay = 0f;                                  // Current mining delay (in seconds)

    private LayerMask environmentLayerMask;                         // A layermask containing the environment layer
    private LineRenderer miningLaser;                               // The linerenderer of the mining laser

    [SerializeField] private bool isEnabled = true;

    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }

    void Start()
    {
        miningLaser = gameObject.GetComponent<LineRenderer>();      // Get the linerenderer component
        miningLaser.enabled = false;                                // Disable the linerenderer
        
        environmentLayerMask = LayerMask.GetMask("Environment");    // Get the environment layer
    }
    void FixedUpdate()
    {
        // If the fire button is pressed, the laser coroutine is started
        if (Input.GetButton("Fire1"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }

        // Decrease the spawn delay if it is larger than 0
        if(spawnDelay > 0)
            spawnDelay -= Time.deltaTime;
    }

    IEnumerator FireLaser()
    {
        // Enables line so the lineRenderer is active while button is pressed
        miningLaser.enabled = true;

        // As long as button is pressed we continously create the laser
        while (Input.GetButton("Fire1") && isEnabled)
        {
            // Initial setup for laser
            Ray2D ray = new Ray2D(transform.position, transform.up);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, transform.up, maxRange, environmentLayerMask);

            // If the laser would hit an object in the layer "Environment" then we create a laser 
            if (hit.collider != null)
            {
                // Setting start and end position of the laser
                miningLaser.SetPosition(0, ray.origin);
                miningLaser.SetPosition(1, hit.point);

                // If we hit an object with the tag "Mineable" then we do something
                if (hit.transform.tag == "Mineable")
                {
                    //TODO: Switch to minable script
                    //This is temp until better solution is created
                    
                    // If the spawn delay has passed
                    if (spawnDelay <= 0)
                    {
                        Instantiate(tempItem, hit.point, Quaternion.identity);      // Instantiate the temp item prefab

                        GameObject tempParticles = Instantiate(miningParticles);    // Instantiate the mining particles prefab
                        tempParticles.transform.position = hit.point;               // Move the mining particles to the mining point

                        spawnDelay = maxSpawnDelay;                                 // Reset the spawn delay
                    }
                }
            }
            // Resetting laser and miningObjects position so it doesn't show
            else
            {
                miningLaser.SetPosition(0, ray.origin);
                miningLaser.SetPosition(1, ray.origin);
            }

            yield return null;
        }
        
        // Removes line if button is released
        miningLaser.enabled = false;
    }
}
