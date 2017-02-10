using UnityEngine;
using System.Collections;

public class TEMP_RANDOMMUSICMIXER : MonoBehaviour {

    [SerializeField] private AudioClip[] musicClips;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = musicClips[Random.Range(0, musicClips.Length)];
        audioSource.Play();
	}
}
