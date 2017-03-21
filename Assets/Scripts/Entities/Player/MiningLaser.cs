using UnityEngine;
using System.Collections;

public class MiningLaser : MonoBehaviour
{
    [SerializeField] private GameObject miningParticles;            // Mining particle system
    [SerializeField] [Range(1f, 15f)] private float maxRange = 5;   // Max range of the mining laser
    [SerializeField] [Range(30f, 90f)] private float maxAngle = 70f;
    [SerializeField] private Vector2 rayOffset;

    private LayerMask environmentLayerMask;                         // A layermask containing the environment layer
    private LineRenderer miningLaser;                               // The linerenderer of the mining laser

    private Camera mainCamera;

    [SerializeField] private bool isEnabled = true;

    public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; } }

    void Start()
    {
        miningLaser = gameObject.GetComponent<LineRenderer>();      // Get the linerenderer component
        miningLaser.enabled = false;                                // Disable the linerenderer

        mainCamera = Camera.main;
        
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
    }

    IEnumerator FireLaser()
    {
        // Enables line so the lineRenderer is active while button is pressed
        miningLaser.enabled = true;

        // As long as button is pressed we continously create the laser
        while (Input.GetButton("Fire1") && isEnabled)
        {
            Vector2 mouseAngle = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - (transform.position + (Quaternion.Euler(0f, 0f, transform.eulerAngles.z) * rayOffset))).normalized;

            if(Vector2.Angle(transform.up, mouseAngle) < maxAngle)
            {
                // Initial setup for laser
                Ray2D ray = new Ray2D(transform.position + (Quaternion.Euler(0f, 0f, transform.eulerAngles.z) * rayOffset), transform.up);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, mouseAngle, maxRange, environmentLayerMask);

                // If the raycast hits an "Environment" object with the "Mineable" tag, start the mining laser
                if (hit.collider != null && hit.collider.tag == "Mineable")
                {
                    Vector3[] laserPosition = { new Vector3(ray.origin.x, ray.origin.y, transform.position.z + 5), new Vector3(hit.point.x, hit.point.y, transform.position.z + 5) };

                    // Setting start and end position of the laser
                    miningLaser.SetPosition(0, laserPosition[0]);
                    miningLaser.SetPosition(1, laserPosition[1]);

                    // Cause the mineable object to be mined
                    hit.collider.gameObject.GetComponent<MineableBehaviour>().Mine(transform.localRotation, hit.point);

                    GameObject tempParticles = Instantiate(miningParticles);    // Instantiate the mining particles prefab
                    tempParticles.transform.position = hit.point;               // Move the mining particles to the mining point
                }
                // Resetting laser and miningObjects position so it doesn't show
                else
                {
                    miningLaser.enabled = false;
                }
            }
            else
            {
                miningLaser.enabled = false;
            }

            yield return null;
        }
        
        // Removes line if button is released
        miningLaser.enabled = false;
    }
}
