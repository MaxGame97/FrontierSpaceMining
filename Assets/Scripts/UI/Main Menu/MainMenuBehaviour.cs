using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;



public class MainMenuBehaviour : MonoBehaviour {
    private readonly int AMOUNT_OF_SAVES = 3;


    private AudioMaster audioScript;
    private GlobalGameControllerBehaviour globalBehaviour;

    private List<GameObject> savesList = new List<GameObject>();

    void Start () {
        Time.timeScale = 1.0f;

        audioScript = GameObject.Find("Global Game Controller").GetComponent<AudioMaster>();
        globalBehaviour = GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>();

        savesList.Capacity = AMOUNT_OF_SAVES;
        for(int i = 0; i < AMOUNT_OF_SAVES; i++)
        {
            string save = "Save " + (i + 1);
            savesList.Add(GameObject.Find(save));
        }
        UpdateSavesInMenu();
        audioScript.UpdateSliders();
    }

    // Triggers the game to load a specific save index
	public void Play(int index)
    {
        audioScript.SaveAudioSettings();

        // Calls the load function, this is a workaround for an issue where calling the function directly from the button
        // caused the changed variables to revert, this messed up the loading system, this workaround fixes this issue
        globalBehaviour.Load(index);
    }

    public void Delete(int index)
    {
        globalBehaviour.DeleteGame(index);
        UpdateSavesInMenu();
    }

    // Exits the game to the desktop (or the editor)
    public void ExitToDesktop()
    {
        audioScript.SaveAudioSettings();


        //If inside the unity editor
        #if UNITY_EDITOR
        //stop the playing instance
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                    //Quit the game
                    Application.Quit();
        #endif
    }

    void UpdateSavesInMenu()
    {
        for(int i = 0; i < AMOUNT_OF_SAVES; i++)
        {
            SaveInformation saveInfo = new SaveInformation();
            saveInfo = globalBehaviour.GetSaveInfo(i);


            if (saveInfo != null)
            {
                int itemAmount = 0;
                int timePlayed = 0;
                int completedLevels = 0;

                itemAmount = saveInfo.AmountOfItems;
                timePlayed = (int)saveInfo.TimePlayed;
                completedLevels = saveInfo.AmountOfLevelsCompleted;

                int minutesPlayed = timePlayed / 60;
                int hoursPlayed = timePlayed / 3600;
            
                savesList[i].GetComponentInChildren<Text>().text = "Completed levels: " + completedLevels + "\n" + "Items: " + itemAmount + "\n" + "Time played: " + hoursPlayed + " hours, " + minutesPlayed + " minutes";
            }
            else
            {
                savesList[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "New Game";
                savesList[i].GetComponentInChildren<Text>().text = "";
            }

        }

    }

    //The function the Masterslider calls to change the mastervolume of the game
    public void ChangeMasterVolume(float volume)
    {
        audioScript.ChangeVolume(volume, "master");
    }
    //The function the musicslider calls to change the musicvolume of the game
    public void ChangeMusicVolume(float volume)
    {
        audioScript.ChangeVolume(volume, "music");
    }
    //The function the SFXslider calls to change the SFXvolume of the game
    public void ChangeSFXVolume(float volume)
    {
        audioScript.ChangeVolume(volume, "sfx");
    }
}
