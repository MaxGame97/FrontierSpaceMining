using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class AudioMaster : MonoBehaviour {

    public AudioMixer masterMixer;

    private Slider masterVol;
    private Slider musicVol;
    private Slider sfxVol;

    private float currentMasterVolume = 1;
    private float currentMusicVolume = 1;
    private float currentSFXVolume = 1;

    private float currentMasterValue = 1;
    private float currentMusicValue = 1;
    private float currentSFXValue = 1;

    private int soundValue = 40;

    // Use this for initialization
    void Start () {

        Debug.Log(masterMixer);

	}
	
    void Update()
    {

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
        }
    }

}
