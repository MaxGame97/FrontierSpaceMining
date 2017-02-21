using UnityEngine;
using System.Collections;

public class GameControllerBehaviour : MonoBehaviour
{

    [SerializeField]
    private bool isPaused = false;

    private MenuBehaviour pauseMenu;
    private Inventory invScript;
    private SaveLoadGame saveLoadScript;


    public bool IsPaused { get { return isPaused; } }
    public bool IsRunning { get { return !isPaused; } }


    void Start()
    {

        if (GameObject.Find("Inventory Controller") != null && GameObject.Find("Menu Controller") != null && GameObject.Find("GlobalGameController") != null)
        {
            pauseMenu = GameObject.Find("Menu Controller").GetComponent<MenuBehaviour>();
            invScript = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
            saveLoadScript = GameObject.Find("GlobalGameController").GetComponent<SaveLoadGame>();
        }
        pauseMenu.CloseMenu();
        UnPause();

        if (saveLoadScript.isLoadingGame)
            saveLoadScript.Load(saveLoadScript.currentSaveID);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            invScript.ToggleInventoryPanel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }

    }

    //Toggle pause on and off
    public void PauseToggle()
    {   //if the game IS paused now...
        if (isPaused)
        {   //..."Start" the timer again...
            Time.timeScale = 1f;
            //...close the menu...
            pauseMenu.CloseMenu();
            //...and change the state of the "paused" bool to false
            isPaused = !isPaused;
        }//if the game is NOT paused now...
        else
        {   //...stop the game(Timer)...
            Time.timeScale = 0f;
            //...show the paused screen GUI...
            pauseMenu.ShowMenu();
            //...and change the state of the "paused" bool to true
            isPaused = !isPaused;
        }
    }

    //Temporary solution to bug
    void UnPause()
    {
        Time.timeScale = 1f;
    }

    //Function that quits the game
    public void ExitGame()
    {   //Can only quit the game if the game is currently paused --- Might change this
        if (isPaused)
        {   //Quit the game
            Application.Quit();
        }
    }

}
