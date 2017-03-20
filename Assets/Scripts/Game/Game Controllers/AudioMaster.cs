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

    private AudioData currentValue = new AudioData();

    private int soundValue = 80;

    private GlobalGameControllerBehaviour globalController;

    public AudioData CurrentValues { get { return currentValue; } }


    void Start () {
        globalController = GetComponent<GlobalGameControllerBehaviour>();
        // Update the current volume values based on the audio mixer's values
        masterMixer.GetFloat("MasterVolume", out currentMasterVolume);
        masterMixer.GetFloat("MusicVolume", out currentMusicVolume);
        masterMixer.GetFloat("SFXVolume", out currentSFXVolume);

        // Convert the current volume values to a linear scale between 0 and 1, this is used to reset the volume slider values
        currentValue.MasterVolume = Mathf.Pow(10f, currentMasterVolume / 80f);
        currentValue.MusicVolume = Mathf.Pow(10f, currentMusicVolume / 80f);
        currentValue.SFXVolume = Mathf.Pow(10f, currentSFXVolume / 80f);


        // Important, these values need to be saved somewhere, while this works fine in the game, it does not carry over between sessions
        // In other words, the sound values need to be saved and restored, this should be made when the audio mixer is instantiated
        // For example, in this script or in the global game controller's script
	}

    public void SaveAudioSettings()
    {
        globalController.SaveAudioData();
    }

    //The function the sliders indirectly call to change the volume of the game. 
    public void ChangeVolume(float volume, string volumeType)
    {
        float newValue;

        newValue = GetVolumeValue(volume);

        if(volumeType == "master")
        {
            masterMixer.SetFloat("MasterVolume", newValue);
            currentMasterVolume = newValue;
            currentValue.MasterVolume = volume;
        }   
        else if(volumeType == "music")
        {
            masterMixer.SetFloat("MusicVolume", newValue);
            currentMusicVolume = newValue;
            currentValue.MusicVolume = volume;
        }
        else if(volumeType == "sfx")
        {
            masterMixer.SetFloat("SFXVolume", newValue);
            currentSFXVolume = newValue;
            currentValue.SFXVolume = volume;
        }

    }


    float GetVolumeValue(float entryVolume)
    {
        float finalVolume;
        finalVolume = (soundValue * Mathf.Sin(entryVolume * Mathf.PI / 2f)) - soundValue;
        return finalVolume;
    }

    //Update the sliders with the current volume of the game, usually used after scenechange
    public void UpdateSliders()
    {
        CheckSliders();

        masterVol.value = currentValue.MasterVolume;
        musicVol.value = currentValue.MusicVolume;
        sfxVol.value = currentValue.SFXVolume;

    }

    //Checks to see if the sliders are referenced. If not, reference them
    void CheckSliders()
    {
        if (masterVol == null && musicVol == null && sfxVol == null)
        {
            masterVol = GameObject.Find("MasterVolSlider").GetComponent<Slider>();
            musicVol = GameObject.Find("MusicVolSlider").GetComponent<Slider>();
            sfxVol = GameObject.Find("SFXVolSlider").GetComponent<Slider>();
        }
    }

}
