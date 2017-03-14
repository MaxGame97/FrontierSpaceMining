using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class AudioMaster : MonoBehaviour {

    public AudioMixer masterMixer;

    private Slider masterVol;
    private Slider musicVol;
    private Slider sfxVol;

    private float currentMasterVolume;
    private float currentMusicVolume;
    private float currentSFXVolume;

    private float currentMasterValue;
    private float currentMusicValue;
    private float currentSFXValue;

    private int soundValue = 40;

    // Use this for initialization
    void Start () {
        // Update the current volume values based on the audio mixer's values
        masterMixer.GetFloat("MasterVolume", out currentMasterVolume);
        masterMixer.GetFloat("MusicVolume", out currentMusicVolume);
        masterMixer.GetFloat("SFXVolume", out currentSFXVolume);

        // Convert the current volume values to a linear scale between 0 and 1, this is used to reset the volume slider values
        currentMasterValue = Mathf.Pow(10f, currentMasterVolume / 80f);
        currentMusicValue = Mathf.Pow(10f, currentMusicVolume / 80f);
        currentSFXValue = Mathf.Pow(10f, currentSFXVolume / 80f);

        // Important, these values need to be saved somewhere, while this works fine in the game, it does not carry over between sessions
        // In other words, the sound values need to be saved and restored, this should be made when the audio mixer is instantiated
        // For example, in this script or in the global game controller's script
	}


    public void ChangeVolume(float volume, string volumeType)
    {
        float newValue;

        newValue = GetVolumeValue(volume);

        if(volumeType == "master")
        {
            masterMixer.SetFloat("MasterVolume", newValue);
            currentMasterVolume = newValue;
            currentMasterValue = volume;
        }   
        else if(volumeType == "music")
        {
            masterMixer.SetFloat("MusicVolume", newValue);
            currentMusicVolume = newValue;
            currentMusicValue = volume;
        }
        else if(volumeType == "sfx")
        {
            masterMixer.SetFloat("SFXVolume", newValue);
            currentSFXVolume = newValue;
            currentSFXValue = volume;
        }
    }


    float GetVolumeValue(float entryVolume)
    {
        float finalVolume;
        finalVolume = (entryVolume * soundValue) - soundValue;
        return finalVolume;
    }

    public void UpdateSliders()
    {
        CheckSliders();
        /*masterMixer.SetFloat("MasterVolume", currentMasterVolume);
        masterMixer.SetFloat("MusicVolume", currentMusicVolume);
        masterMixer.SetFloat("SFXVolume", currentSFXVolume);*/

        masterVol.value = currentMasterValue;
        musicVol.value = currentMusicValue;
        sfxVol.value = currentSFXValue;
    }

    void CheckSliders()
    {
        if (masterVol == null && musicVol == null && sfxVol == null)
        {
            masterVol = GameObject.Find("MasterVolSlider").GetComponent<Slider>();
            musicVol = GameObject.Find("MusicVolSlider").GetComponent<Slider>();
            sfxVol = GameObject.Find("SFXVolSlider").GetComponent<Slider>();
            Debug.Log("Found all sliders");
        }
    }

}
