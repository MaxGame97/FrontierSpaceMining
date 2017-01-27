using UnityEngine;
using System.Collections;

public class MiningLaser : MonoBehaviour
{
    // Range of the laser
    [SerializeField] [Range(1f, 15f)] private float laserRange = 5;

    // Delay between the itemspawns
    [SerializeField] [Range(0.1f, 5f)] private float spawnDelay;

    //Temporary variables until we get better solution for mininglaser
    [SerializeField] private Transform tempObject;
    [SerializeField] private Transform miningParticlesPosition;

    private Vector3 startPos;

    // Setting up the layermask, linerenderer and particlesystem
    private LayerMask mask;
    private LineRenderer line;
    private ParticleSystem miningParticles;

    void Start()
    {
        // Getting the lineRenderer from the gameObject and disabling it 
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;

        // Getting the particlesystem from the component
        miningParticles = miningParticlesPosition.GetComponent<ParticleSystem>();

        startPos = miningParticlesPosition.position;

        //Setting layermask variable to "environment"
        mask = LayerMask.GetMask("Environment");
    }
    void Update()
    {
        // If we press the fire button then we start the laser coroutine and when the fire button is released we stop the laser coroutine
        if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
    }
    IEnumerator FireLaser()
    {
        // Enables line so the lineRenderer is active while button is pressed
        line.enabled = true;
        // Enables the particlesystem so it is active while button is pressed
        miningParticles.Play();

        // As long as button is pressed we continously create the laser
        while (Input.GetButton("Fire1"))
        {
            // Initial setup for laser
            Ray2D ray = new Ray2D(transform.position, transform.up);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, transform.up, laserRange, mask);

            // If the laser would hit an object in the layer "Environment" then we create a laser 
            if (hit.collider != null)
            {
                // Setting start and end position of the laser
                line.SetPosition(0, ray.origin);
                line.SetPosition(1, hit.point);

                // Setting the position of the particlesystem
                miningParticlesPosition.position = hit.point;

                // If we hit an object with the tag "Mineable" then we do something
                if (hit.transform.tag == "Mineable")
                {
                    //TODO: Switch to minable script
                    //This is temp until better solution is created

                    // Spawndelay between the items while mining
                    spawnDelay -= Time.deltaTime;
                    if (spawnDelay <= 0)
                    {
                        Instantiate(tempObject, hit.point, Quaternion.identity);
                        spawnDelay = 1f;
                    }
                }
            }
            // Resetting laser and miningObjects position so it doesn't show
            else
            {

                //TODO fix this bug where I can't remove the particle effect
                //Currently i reset the particle effect so it's barely visible
                miningParticlesPosition.position = startPos;

                line.SetPosition(0, ray.origin);
                line.SetPosition(1, ray.origin);
            }
              yield return null;
        }

        // Removes the particle effect when button is released  
        miningParticles.Stop();
        // Removes line if button is released
        line.enabled = false;
    }
}
