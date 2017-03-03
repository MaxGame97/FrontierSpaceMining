using UnityEngine;
using System.Collections;

public class MainMenuBehaviour : MonoBehaviour {

    private AudioMaster audioScript;

    void Start () {
        Time.timeScale = 1.0f;

        audioScript = GameObject.Find("Global Game Controller").GetComponent<AudioMaster>();

        audioScript.UpdateSliders();
    }

    // Triggers the game to load a specific save index
	public void Load(int index)
    {
        // Calls the load function, this is a workaround for an issue where calling the function directly from the button
        // caused the changed variables to revert, this messed up the loading system, this workaround fixes this issue
        GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>().Load(index);
    }

    // Exits the game to the desktop (or the editor)
    public void ExitToDesktop()
    {
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
        audioScript.ChangeVolume(volume, "master");
    }

    public void ChangeMusicVolume(float volume)
    {
        audioScript.ChangeVolume(volume, "music");
    }

    public void ChangeSFXVolume(float volume)
    {
        audioScript.ChangeVolume(volume, "sfx");
    }
}
