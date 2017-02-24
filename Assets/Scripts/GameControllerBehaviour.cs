using UnityEngine;
using System.Collections;

public class GameControllerBehaviour : MonoBehaviour
{

    [SerializeField] private bool isPaused = false;

    public bool IsPaused { get { return isPaused; } }
    public bool IsRunning { get { return !isPaused; } }

    //Toggle pause on and off
    public void PauseToggle()
    {   //if the game IS paused now...
        if (isPaused)
        {   //..."Start" the timer again...
            Time.timeScale = 1f;
            //...and change the state of the "paused" bool to false
            isPaused = false;
        }//if the game is NOT paused now...
        else
        {   //...stop the game(Timer)...
            Time.timeScale = 0f;
            //...and change the state of the "paused" bool to true
            isPaused = true;
        }
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
