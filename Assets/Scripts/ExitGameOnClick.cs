using UnityEngine;
using System.Collections;

public class ExitGameOnClick : MonoBehaviour {

    public void ExitGame()
    {
#if UNITY_EDITOR    //If inside the unity editor
        //stop the playing instance
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //Quit the game
        Application.Quit();
#endif
    }

}
