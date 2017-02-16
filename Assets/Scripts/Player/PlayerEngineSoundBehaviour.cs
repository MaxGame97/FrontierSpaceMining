using UnityEngine;
using System.Collections;

public class PlayerEngineSoundBehaviour : MonoBehaviour {
    
    private SoundFXBehaviour engine;

    private float currentThrustAmount = 0f;
    private float playerThrustAmount = 0f;

	// Use this for initialization
	void Start () {
        engine = transform.GetChild(0).GetComponent<SoundFXBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        currentThrustAmount = Mathf.Lerp(currentThrustAmount, Mathf.Abs(playerThrustAmount), 0.2f);

        engine.SetVolume(currentThrustAmount);
	}

    // Sets the thrust amount
    public void SetThrustAmount(float thrustAmount)
    {
        playerThrustAmount = thrustAmount;
    }

    public float GetSpeedSound()
    {
        return playerThrustAmount;
    }
}
