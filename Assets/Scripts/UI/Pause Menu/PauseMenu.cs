using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField] bool saveAllowed = false;

    private GameControllerBehaviour gameController;

    private CanvasGroup canvas;

    private GameObject mainPanel;
    private GameObject optionsPanel;
    private GameObject audioOptionsPanel;
    private GameObject controlOptionsPanel;

    private GameObject currentPanel;

    private bool pauseMenuEnabled = true;

    public bool PauseMenuEnabled { get { return pauseMenuEnabled; } }

	// Use this for initialization
	void Start () {
        gameController = GameObject.Find("Game Controller").GetComponent<GameControllerBehaviour>();

        canvas = gameObject.GetComponentInParent<CanvasGroup>();

        mainPanel = GameObject.Find("Main Panel");
        optionsPanel = GameObject.Find("Options Panel");
        audioOptionsPanel = GameObject.Find("Audio Options Panel");
        controlOptionsPanel = GameObject.Find("Control Options Panel");

        optionsPanel.SetActive(false);
        audioOptionsPanel.SetActive(false);
        controlOptionsPanel.SetActive(false);

        TogglePauseMenuPanel();
        gameController.PauseToggle();
    }

    // Toggles the pause menu
    public void TogglePauseMenuPanel()
    {
        if (pauseMenuEnabled)
        {
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
            pauseMenuEnabled = false;

            gameController.PauseToggle();

            BackToMainPanel();
        }
        else
        {
            canvas.alpha = 1;
            canvas.blocksRaycasts = true;
            pauseMenuEnabled = true;
            
            gameController.PauseToggle();
        }
    }

    public void OpenOptionsPanel()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);

        currentPanel = optionsPanel;
    }

    public void OpenAudioOptionsPanel()
    {
        optionsPanel.SetActive(false);
        audioOptionsPanel.SetActive(true);

        currentPanel = audioOptionsPanel;
    }

    public void OpenControlOptionsPanel()
    {
        optionsPanel.SetActive(false);
        controlOptionsPanel.SetActive(true);

        currentPanel = controlOptionsPanel;
    }

    public void Back()
    {
        if(currentPanel != null)
        {
            if(currentPanel == mainPanel)
            {
                TogglePauseMenuPanel();
            }
            else if(currentPanel == optionsPanel)
            {
                optionsPanel.SetActive(false);
                mainPanel.SetActive(true);

                currentPanel = mainPanel;
            }
            else if(currentPanel == audioOptionsPanel)
            {
                audioOptionsPanel.SetActive(false);
                optionsPanel.SetActive(true);

                currentPanel = optionsPanel;
            }
            else if(currentPanel == controlOptionsPanel)
            {
                controlOptionsPanel.SetActive(false);
                optionsPanel.SetActive(true);

                currentPanel = optionsPanel;
            }
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

        Application.Quit();
    }

    void BackToMainPanel()
    {
        optionsPanel.SetActive(false);
        audioOptionsPanel.SetActive(false);
        controlOptionsPanel.SetActive(false);

        mainPanel.SetActive(true);

        currentPanel = mainPanel;
    }
}
