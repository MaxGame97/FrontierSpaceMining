using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class GlobalGameControllerBehaviour : MonoBehaviour {

    private List<int> currentCompletedLevels = new List<int>();

    private Inventory currentInventory;

    private static int currentSaveIndex = 0;
    
    public List<int> CurrentCompletedLevels { get { return currentCompletedLevels; } set { currentCompletedLevels = value; } }
    public int CurrentSaveIndex { get { return currentSaveIndex; } }
    
    void Start()
    {
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
    }

    // Creates a new save on the specified index
    public void NewGame(int index)
    {
        // Update the current save index
        currentSaveIndex = index;

        // Instantiate a new binary formatter and a new filestream
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream;

        // If there does not exist a file representing this save index
        if (!File.Exists(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat"))
        {
            // Create it
            fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat");
        }
        // If there already is a file representing this save index
        else
        {
            // Remove it and create a new one
            File.Delete(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat");
            fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat");
        }

        // Create a temporary save data representing the current save data
        SaveData saveData = new SaveData();

        // Serialize the save data and unload the file
        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();

        SceneManager.LoadScene("Hub");

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
        if (!File.Exists(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat"))
        {
            // Create it
            fileStream = File.Create(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat");
        }
        // If there already is a file representing the current save index
        else
        {
            // Load it
            fileStream = File.Open(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat", FileMode.Open);
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

        if (!File.Exists(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat"))
        {
            Debug.LogError("No savegame found on index: " + currentSaveIndex);
        }
        else
        {
            UpdateCurrentCompletedLevels();

            SceneManager.LoadScene("Hub");
        }
    }

    public void UpdateCurrentInventory()
    {
        // Instantiate a new binaryformatter
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        // Load the current save file
        FileStream fileStream = File.Open(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat", FileMode.Open);

        // Get the save data from the save file
        SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
        
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

        // Close the file stream
        fileStream.Close();
    }

    public void UpdateCurrentCompletedLevels()
    {
        // Instantiate a new binaryformatter
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        // Load the current save file
        FileStream fileStream = File.Open(Application.persistentDataPath + "/playerInfo" + currentSaveIndex + ".dat", FileMode.Open);

        // Get the save data from the save file
        SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);

        currentCompletedLevels.Clear();

        for (int i = 0; i < saveData.CompletedLevels.Count; i++)
        {
            currentCompletedLevels.Add(saveData.CompletedLevels[i]);
        }

        // Close the file stream
        fileStream.Close();
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

        saveData.CompletedLevels = currentCompletedLevels;
    }
    
    // Updates the current inventory
    void UpdateInventory()
    {
        // Finds the current inventory and sets it as the current one
        currentInventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
    }
}

[Serializable]
class SaveData
{
    private List<InventoryData> items = new List<InventoryData>();
    private List<int> completedLevels = new List<int>();

    public List<InventoryData> Items { get { return items; } set { items = value; } }
    public List<int> CompletedLevels { get { return completedLevels; } set { completedLevels = value; } }
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