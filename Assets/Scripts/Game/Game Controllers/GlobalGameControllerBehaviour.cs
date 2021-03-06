﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class GlobalGameControllerBehaviour : MonoBehaviour {

    private readonly string SAVE_NAME = "saveData_";
    private readonly string AUDIO_NAME = "audioData";

    private AudioMaster audioMaster;

    private List<int> currentCompletedLevels = new List<int>();

    private Inventory currentInventory;
    private AudioData currentAudio;

    private static int currentSaveIndex = 0;
    private float startTime = 0;
    
    public List<int> CurrentCompletedLevels { get { return currentCompletedLevels; } set { currentCompletedLevels = value; } }
    public int CurrentSaveIndex { get { return currentSaveIndex; } }
    
    void Start()
    {
        audioMaster = GetComponent<AudioMaster>();
        // If this is the only global game controller
        if (GameObject.FindGameObjectsWithTag("Global Game Controller").Length == 1)
        {
            // Set this object to be persistent between scenes
            DontDestroyOnLoad(gameObject);
        }
        // If another one exists
        else
        {
            // Destroy this global game controller
            Destroy(gameObject);
        }

        LoadAudioData();

    }


    ///////////////////////////
    ///////SAVE/LOADGAME///////
    ///////////////////////////

    // Creates a new save on the specified index
    public void NewGame(int index)
    {
        // Update the current save index
        currentSaveIndex = index;

        // Instantiate a new binary formatter and a new filestream
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        // Create it
        fileStream = File.Create(Application.persistentDataPath + "/"+ SAVE_NAME + currentSaveIndex + ".dat");

        Debug.Log("Created new game on index: " + index);

        // Create a temporary save data representing the current save data
        SaveData saveData = new SaveData();

        // Serialize the save data and unload the file
        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();

        UpdateCurrentCompletedLevels();
        startTime = Time.time;
        SceneManager.LoadScene("Hub");

    }

    public void DeleteGame(int index)
    {
        // Remove game on the index slot
        File.Delete(Application.persistentDataPath + "/" + SAVE_NAME + index + ".dat");
        Debug.Log("Deleted game on index: " + index);
    }

    // Saves the game to the active index
    public void Save()
    {
        // Updates the current inventory
        UpdateInventory();
        
        // Instantiate a new binary formatter and a new filestream
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        // If there does not exist a file representing the current save index
        if (!File.Exists(Application.persistentDataPath + "/" + SAVE_NAME + currentSaveIndex + ".dat"))
        {
            // Create it
            fileStream = File.Create(Application.persistentDataPath + "/" + SAVE_NAME + currentSaveIndex + ".dat");
        }
        // If there already is a file representing the current save index
        else
        {
            // Load it
            fileStream = File.Open(Application.persistentDataPath + "/" + SAVE_NAME + currentSaveIndex + ".dat", FileMode.Open);
        }

        // Create a temporary save data representing the current save data
        SaveData saveData = new SaveData();
        UpdateSaveData(saveData, currentInventory);

        // Serialize the save data and unload the file
        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
    }

    // Loads the game
    public void Load(int index)
    {
        // Update the current save index
        currentSaveIndex = index;

        if (!File.Exists(Application.persistentDataPath + "/" + SAVE_NAME + currentSaveIndex + ".dat"))
        {
            NewGame(index);
        }
        else
        {

            UpdateCurrentCompletedLevels();
            startTime = Time.time;
            SceneManager.LoadScene("Hub");
        }
    }

    public void UpdateCurrentInventory()
    {
        SaveData saveData = GetSaveData();
        
        // Update the current inventory
        UpdateInventory();

        if (currentInventory != null)
        {
            // Go through the item data in the save file
            for (int i = 0; i < saveData.Items.Count; i++)
            {
                // Get the current inventory data
                InventoryData inventoryData = saveData.Items[i];

                // If the ID of the inventory data is not -1 (empty item)
                if (inventoryData.ID != -1)
                {
                    // Loop as many times as the amount variable in the inventory data
                    for (int j = 0; j < inventoryData.Amount; j++)
                    {
                        // Add an item with this ID to the inventory
                        currentInventory.AddItem(inventoryData.ID);
                    }
                }
            }
        }
        else
            Debug.LogError("Tried to load save data, but no inventory was found");

    }

    public void UpdateCurrentCompletedLevels()
    {

        SaveData saveData = GetSaveData();

        currentCompletedLevels.Clear();

        for (int i = 0; i < saveData.CompletedLevels.Count; i++)
        {
            currentCompletedLevels.Add(saveData.CompletedLevels[i]);
        }

    }

    // Updates the save data to the current save
    void UpdateSaveData(SaveData saveData, Inventory inventory)
    {
        // Clear all the item data from the save data
        saveData.Items.Clear();

        saveData.CompletedLevels.Clear();
        
        // Go through all of the items in the current inventory
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            // If the current item does not have the ID -1 (empty item)
            if(inventory.Items[i].ID != -1)
            {
                // Create a temporary inventorydata and add it to the save data
                InventoryData data = new InventoryData(inventory.Items[i].ID, currentInventory.CheckItemCount(inventory.Items[i].ID));
                saveData.Items.Add(data);
            }
        }
        saveData.TimePlayed = Time.time - startTime;
       
        saveData.CompletedLevels = currentCompletedLevels;
    }
    
    // Updates the current inventory
    void UpdateInventory()
    {
        // Finds the current inventory and sets it as the current one
        currentInventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
    }

    SaveData GetSaveData()
    {
        SaveData saveData;

        if(File.Exists(Application.persistentDataPath + "/" + SAVE_NAME + currentSaveIndex + ".dat"))
        {
            // Instantiate a new binaryformatter
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            // Load the current save file
            FileStream fileStream = File.Open(Application.persistentDataPath + "/" + SAVE_NAME + currentSaveIndex + ".dat", FileMode.Open);

            // Get the save data from the save file
            saveData = (SaveData)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
        }
        else
        {
            saveData = null;
        }

        return saveData;
    }


    //////////////////////
    ///////SAVEINFO///////
    //////////////////////

    public SaveInformation GetSaveInfo(int index)
    {
        SaveInformation saveInfo = new SaveInformation();
        currentSaveIndex = index;

        SaveData saveData = GetSaveData();

        if (saveData != null)
        {
            for (int i = 0; i < saveData.Items.Count; i++)
            {
                if(saveData.Items[i].ID < 100)
                {
                    saveInfo.AmountOfAudioLogs++;
                }
                
            }
            saveInfo.TimePlayed += saveData.TimePlayed;
            saveInfo.AmountOfLevelsCompleted = saveData.CompletedLevels.Count;
        }
        else
        {
            saveInfo = null;
        }
        return saveInfo;
    }

    ///////////////////
    ///////AUDIO///////
    ///////////////////

    void NewAudioData()
    {
        AudioData audioData = new AudioData();

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        fileStream = File.Create(Application.persistentDataPath + "/" + AUDIO_NAME + ".dat");

        audioData.MasterVolume = 1;
        audioData.MusicVolume = 1;
        audioData.SFXVolume = 1;

        binaryFormatter.Serialize(fileStream, audioData);
        fileStream.Close();
    }

    //Saves the currentaudio
    public void SaveAudioData()
    {        

        // If there does not exist a file representing the audiodata
        if (!File.Exists(Application.persistentDataPath + "/" + AUDIO_NAME + ".dat"))
        {
            Debug.LogError("No Audiodata found!");
        }
        // If there already is a file representing the audiodata
        else
        {
            // Create a temporary save data representing the audiodata
            AudioData audioData = new AudioData();

            // Instantiate a new binary formatter and a new filestream
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream;

            // Load the file
            fileStream = File.Open(Application.persistentDataPath + "/" + AUDIO_NAME + ".dat", FileMode.Open);

            audioData = UpdateAudioSaveData();

            // Serialize the audio data and unload the file
            binaryFormatter.Serialize(fileStream, audioData);
            fileStream.Close();
        }


    }

    //Loads the saved audio
    void LoadAudioData()
    {

        if (!File.Exists(Application.persistentDataPath + "/" + AUDIO_NAME + ".dat"))
        {
            Debug.LogWarning("No audiodata found, creating new");
            NewAudioData();
            LoadToCurrentAudio();
        }
        else
        {
            LoadToCurrentAudio();
        }
    }



    //Applies the saved audio to the currentaudio
    void LoadToCurrentAudio()
    {
        AudioData audioData = GetAudioData();
        currentAudio = audioData;
        ApplyAudioFromSave();
    }
    //Applies the current audio to the sliders
    void ApplyAudioFromSave()
    {
        if (currentAudio != null)
        {
            audioMaster.ChangeVolume(currentAudio.MasterVolume, "master");
            audioMaster.ChangeVolume(currentAudio.MusicVolume, "music");
            audioMaster.ChangeVolume(currentAudio.SFXVolume, "sfx");

            audioMaster.UpdateSliders();
        }
    }

    //Updates the savedata, only used by the SaveAudio function
    AudioData UpdateAudioSaveData()
    {
        UpdateCurrentAudio();
        return currentAudio;
    }

    //Updates the currentAudio with the current audiosettings
    void UpdateCurrentAudio()
    {
        currentAudio = audioMaster.CurrentValues;
    }

    //Function for getting the AudioData class from save
    AudioData GetAudioData()
    {
        AudioData audioData;
        if (File.Exists(Application.persistentDataPath + "/" + AUDIO_NAME + ".dat"))
        {
            // Instantiate a new binaryformatter
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            // Load the current save file
            FileStream fileStream = File.Open(Application.persistentDataPath + "/" + AUDIO_NAME + ".dat", FileMode.Open);

            // Get the save data from the save file
            audioData = (AudioData)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
        }
        else
        {
            audioData = null;
        }
        return audioData;
    }

}

///////////////////
//////CLASSES//////
///////////////////
public class SaveInformation
{
    private float timePlayed = 0;
    private int amountOfAudioLogs = 0;
    private int amountOfLevelsCompleted = 0;
    private int maxAudioLogs = 0;
    private int maxLevels = 0;

    public float TimePlayed { get { return timePlayed; } set { timePlayed = value; } }
    public int AmountOfAudioLogs { get { return amountOfAudioLogs; } set { amountOfAudioLogs = value; } }
    public int AmountOfLevelsCompleted { get { return amountOfLevelsCompleted; } set { amountOfLevelsCompleted = value; } }
    public int MaxAudioLogs { get { return maxAudioLogs; } set { maxAudioLogs = value; } }
    public int MaxLevels { get { return maxLevels; } set { maxLevels = value; } }

}

[Serializable]
class SaveData
{
    private List<InventoryData> items = new List<InventoryData>();
    private List<int> completedLevels = new List<int>();
    private float timePlayed = 0;

    public List<InventoryData> Items { get { return items; } set { items = value; } }
    public List<int> CompletedLevels { get { return completedLevels; } set { completedLevels = value; } }
    public float TimePlayed { get { return timePlayed; } set { timePlayed = value; } }
}

[Serializable]
struct InventoryData
{
    private int iD;     // The item ID
    private int amount; // The amount of items

    public int ID { get { return iD; } }
    public int Amount { get { return amount; } }

    // Contructs a new inventory data
    public InventoryData(int iD, int amount)
    {
        this.iD = iD;
        this.amount = amount;
    }
}

[Serializable]
public class AudioData
{
    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    public float MasterVolume { get { return masterVolume; } set { masterVolume = value; } }
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
    public float SFXVolume { get { return sfxVolume; } set { sfxVolume = value; } }

}