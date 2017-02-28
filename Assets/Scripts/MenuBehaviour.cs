using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {

    public static Canvas control;

    private Canvas menuCanvas;
    private AudioMaster audioScript;

	// Use this for initialization
	void Start () {
        menuCanvas = gameObject.GetComponentInParent<Canvas>();
        audioScript = GameObject.Find("GlobalGameController").GetComponent<AudioMaster>();

        audioScript.UpdateSliders();
    }


    public void ShowMenu()
    {
        menuCanvas.enabled = true;
    }
    public void CloseMenu()
    {
        menuCanvas.enabled = false;
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
