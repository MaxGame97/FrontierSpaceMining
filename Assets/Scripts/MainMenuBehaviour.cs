using UnityEngine;
using System.Collections;

public class MainMenuBehaviour : MonoBehaviour {

    private AudioMaster audioScript;

    void Start () {
        Time.timeScale = 1.0f;

        audioScript = GameObject.Find("Global Game Controller").GetComponent<AudioMaster>();

        audioScript.UpdateSliders();
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
