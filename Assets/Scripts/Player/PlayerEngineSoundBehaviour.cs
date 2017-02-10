using UnityEngine;
using System.Collections;

public class PlayerEngineSoundBehaviour : MonoBehaviour {

    private PlayerMovement player;
    private SoundFXBehaviour engine;

    private float thrustAmount;

	// Use this for initialization
	void Start () {
        player = GetComponent<PlayerMovement>();
        engine = transform.GetChild(0).GetComponent<SoundFXBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        thrustAmount = Mathf.Lerp(thrustAmount, Mathf.Abs(player.ThrustAmount), 0.2f);

        engine.SetVolume(thrustAmount);
	}
}
