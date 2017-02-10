using UnityEngine;
using System.Collections;

public class SoundFXBehaviour : MonoBehaviour {

    private AudioSource audioSource;                        // The audiosource component

    [SerializeField] private bool isLooping = false;        // Whether or not the audio source should loop
    [SerializeField] private bool randomStartTime = false;  // Specifies if the audio should start at a random point

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();  // Get the audiosource component

        audioSource.loop = isLooping;

        audioSource.Play();

        if (randomStartTime)
            audioSource.time = Random.Range(0, audioSource.clip.length);
	}
	
	// Update is called once per frame
	void Update () {
        // If the audiosource has finished playing
        if (!audioSource.isPlaying && !audioSource.loop)
            // Destroy the soundFX object
            Destroy(gameObject);
	}

    // Sets the audiosource's volume
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
