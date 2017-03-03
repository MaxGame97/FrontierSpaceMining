using UnityEngine;
using System.Collections;

public class PlayerEngineSoundBehaviour : MonoBehaviour {
    
    private SoundFXBehaviour engine;        // Reference to the engine's sound behaviour

    private float currentThrustAmount = 0f; // The current thrust amount (between 0 and 1)
    private float playerThrustAmount = 0f;  // The player's current thrust amount (between 0 and 1)

    public float PlayerThrustAmount { get { return playerThrustAmount; } set { playerThrustAmount = value; } }

	// Use this for initialization
	void Start () {
        // Check all of the player's child objects
        for(int i = 0; i < transform.childCount; i++)
        {
            // Try to get a sound FX component
            SoundFXBehaviour tempSoundFXBehaviour = transform.GetChild(i).GetComponent<SoundFXBehaviour>();

            // If a sound FX component was found
            if(tempSoundFXBehaviour != null)
            {
                // Assign it to tis behaviour's sound FX reference
                engine = tempSoundFXBehaviour;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        // If the game is unpaused
        if(Time.timeScale != 0)
        {
            // Lerp the thrust amount between the current and desired value
            currentThrustAmount = Mathf.Lerp(currentThrustAmount, Mathf.Abs(playerThrustAmount), 0.2f);

            // Update the engine volume with the curren thrust value
            engine.SetVolume(currentThrustAmount);
        }
        // If the game is paused
        else
        {
            // Update the engine volume with 0 (mute)
            engine.SetVolume(0f);
        }
	}
}
