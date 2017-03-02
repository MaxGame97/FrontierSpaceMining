using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour {

    [SerializeField] private bool saveAllowed = false;

    private GameControllerBehaviour gameController;

    private CanvasGroup canvas;
    
    private AudioMaster audioScript;

    private bool pauseMenuEnabled = true;

    public bool PauseMenuEnabled { get { return pauseMenuEnabled; } }

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Pause Menu System").GetComponent<CanvasGroup>();

        gameController = GameObject.Find("Game Controller").GetComponent<GameControllerBehaviour>();

        if(GameObject.Find("Global Game Controller") != null)
            audioScript = GameObject.Find("Global Game Controller").GetComponent<AudioMaster>();

        if (audioScript != null)
            audioScript.UpdateSliders();

        TogglePauseMenuPanel();
        gameController.PauseToggle();
    }

    public void TogglePauseMenuPanel()
    {
        if (pauseMenuEnabled)
        {
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
            pauseMenuEnabled = false;

            gameController.PauseToggle();
        }
        else
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            pauseMenuEnabled = true;

            gameController.PauseToggle();
        }
    }

    public void ExitToMainMenu()
    {
        if (saveAllowed)
        {
            GlobalGameControllerBehaviour globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();

            globalGameController.Save();
        }

        SceneManager.LoadScene("Main Menu");
    }

    public void ExitToDesktop()
    {
        if (saveAllowed)
        {
            GlobalGameControllerBehaviour globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();

            globalGameController.Save();
        }

        Application.Quit();
    }

    public void ChangeMasterVolume(float volume)
    {
        if (audioScript != null)
            audioScript.ChangeVolume(volume, "master");   
    }
    public void ChangeMusicVolume(float volume)
    {
        if (audioScript != null)
            audioScript.ChangeVolume(volume, "music");
    }
    public void ChangeSFXVolume(float volume)
    {
        if (audioScript != null)
            audioScript.ChangeVolume(volume, "sfx");
    }

}
