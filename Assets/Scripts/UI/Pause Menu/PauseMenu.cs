using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField] private bool saveAllowed = false;

    private GameControllerBehaviour gameController;

    private CanvasGroup canvas;
    
    private AudioMaster audioScript;
    private PauseMenuAnimator pauseAnimator;

    private bool pauseMenuEnabled = true;

    public bool PauseMenuEnabled { get { return pauseMenuEnabled; } }

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Pause Menu System").GetComponent<CanvasGroup>();

        gameController = GameObject.Find("Game Controller").GetComponent<GameControllerBehaviour>();
        pauseAnimator = GetComponent<PauseMenuAnimator>();

        if(GameObject.Find("Global Game Controller") != null)
            audioScript = GameObject.Find("Global Game Controller").GetComponent<AudioMaster>();

        if (audioScript != null)
            audioScript.UpdateSliders();

        TogglePauseMenuPanel();
        pauseAnimator.TogglePausePanel();

        gameController.PauseToggle();
    }

    public void TogglePauseMenuPanel()
    {
        if (pauseMenuEnabled)
        {
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
            pauseMenuEnabled = false;
            pauseAnimator.TogglePausePanel();
            gameController.PauseToggle();
        }
        else
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            pauseMenuEnabled = true;
            pauseAnimator.TogglePausePanel();
            gameController.PauseToggle();
        }
    }

    public void ExitToMainMenu()
    {
        if (GameObject.Find("Global Game Controller") != null && saveAllowed)
        {
            GlobalGameControllerBehaviour globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();

            globalGameController.Save();
        }

        SceneManager.LoadScene("Main Menu");
    }

    public void ExitToDesktop()
    {
        if (GameObject.Find("Global Game Controller") != null && saveAllowed)
        {
            GlobalGameControllerBehaviour globalGameController = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();

            globalGameController.Save();
        }

        //If inside the unity editor
        #if UNITY_EDITOR
            //stop the playing instance
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //Quit the game
            Application.Quit();
        #endif
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
