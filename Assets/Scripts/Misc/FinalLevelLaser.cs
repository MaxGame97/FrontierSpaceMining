using UnityEngine;
using System.Collections;

public class FinalLevelLaser : MonoBehaviour {
    
    [SerializeField] [Range(0f, 25f)] private float duration = 2;   // Duration of the laser
    [SerializeField] [Range(1f, 50f)] private float intervall = 10; // Time between each laser shot
    
    private MeshRenderer BigLaser;                                  // The MeshRenderer of the mining laser

    [SerializeField] private bool isEnabled = true;

    private float timer;

    void Start()
    {
        BigLaser = gameObject.GetComponent<MeshRenderer>();      // Get the MeshRenderer component
        BigLaser.enabled = false;                                // Disable the MeshRenderer
    }
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        // If the timer runs out, the laser coroutine is started
        if (timer > intervall)
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
        if(timer > intervall + duration)
        {
            StopCoroutine("FireLaser");
            timer = 0;
        }
    }

    IEnumerator FireLaser()
    {
        // Enables line so the lineRenderer is active while the timer has not run out
        BigLaser.enabled = true;

        // As long as the timer has not run out
        while (timer < intervall + duration && isEnabled)
        {
            //Do nothing while laser is shooting
            yield return null;
        }

        // Removes line if timer runs out
        BigLaser.enabled = false;
    }
}
