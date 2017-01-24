using UnityEngine;
using System.Collections;

public class GameControllerBehaviour : MonoBehaviour {

    [SerializeField] private bool isPaused = false;

    public bool IsPaused { get { return isPaused; } }
    public bool IsRunning { get { return !isPaused; } }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                Time.timeScale = 1f;

                isPaused = !isPaused;
            }
            else
            {
                Time.timeScale = 0f;

                isPaused = !isPaused;
            }
        }
	}
}
