using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour {

    // Instance of the particle system
    ParticleSystem particles;

	// Use this for initialization
	void Start () {
        particles = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        // If the particle system has finished playing, destroy it
        if (!particles.IsAlive())
            Destroy(gameObject);
	}
}
