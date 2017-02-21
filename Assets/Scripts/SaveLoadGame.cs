using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class SaveLoadGame : MonoBehaviour {

    public static SaveLoadGame control;

    private Inventory inventoryScript;
    private PlayerBehaviour playerScript;

    private bool loadingGame = false;
    private PlayerData loadData;

    private static int saveIndex;

    public bool isLoadingGame { get { return loadingGame; } }
    public int currentSaveID { get { return saveIndex; } }

    void Awake()
    {

        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        if (GameObject.Find("Inventory Controller") != null && GameObject.Find("Player") != null)
        {
            inventoryScript = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
            playerScript = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        }

    }


    public void NewGame(int id)
    {
        saveIndex = id;
        if (!File.Exists(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat"))
        {
            File.Create(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat");
        }
        else
        {
            File.Delete(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat");
            File.Create(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat");
            Debug.Log("Last save on id: "+ saveIndex + " was overwritten.");
        }
        Load(saveIndex, true);
        //Save();
        
    }

    public void Save()
    {
        CheckScripts();

        if (playerScript.CurrentHealth > 0)
        {
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            if (!File.Exists(Application.persistentDataPath + "/playerInfo"+saveIndex+".dat"))
            {
                file = File.Create(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat");
            }
            else
            {
                file = File.Open(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat", FileMode.Open);
            }

            PlayerData data = new PlayerData();
            data.health = playerScript.CurrentHealth;
            copyToStruct(data, inventoryScript.Items);

            bf.Serialize(file, data);
            file.Close();

            Debug.Log("Saved game on ID: "+saveIndex);
        }
        else
        {
            Debug.Log("You can't save when you are dead!");
        }
    }


    public void Load(int id, bool newGame = false)
    {

        saveIndex = id;
        if (loadingGame)
        {
            Debug.Log("Load step 2");
            CheckScripts();

            if (!File.Exists(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat"))
            {
                Debug.Log("No savegame found on ID: " + saveIndex);
            }
            else
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo" + saveIndex + ".dat", FileMode.Open);
                PlayerData loadData = (PlayerData)bf.Deserialize(file);
                file.Close();

                playerScript.CurrentHealth = loadData.health;
                loadInvFromSave(loadData.items);

                Debug.Log("Loaded game on ID: " + saveIndex);
            }
            loadingGame = false;
        }
        else
        {
            Debug.Log("Load step 1");
            if(!newGame)
                loadingGame = true;
            else
                loadingGame = false;
            SceneManager.LoadScene("Main");
        }
    }




    void loadInvFromSave(List<InventoryData> saveData)
    {
        foreach(InventoryData item in saveData)
        {
            for(int i = 0; i < item.amount; i++)
            {
                inventoryScript.AddItem(item.id);
            }
        }
        Debug.Log("Done loading items");
    }

    void copyToStruct(PlayerData playerData, List<Item> items)
    {
        foreach(Item item in items)
        {
            if(item.ID == -1)
            {
                continue;
            }
            else
            {
                InventoryData data = new InventoryData();
                data.id = item.ID;
                data.amount = inventoryScript.CheckItemCount(item.ID);
                playerData.items.Add(data);


                Debug.Log("id: " + data.id + "\t" + "Amount: " + data.amount);

            }
            
        }
    }

    void CheckScripts()
    {
        if (inventoryScript == null && playerScript == null)
        {
            inventoryScript = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
            playerScript = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        }
    }

}
[Serializable]
class PlayerData
{
    public float health;
    public List<InventoryData> items = new List<InventoryData>();


}
[Serializable]
struct InventoryData
{
    public int id;
    public int amount;

    void AddItem(int idValue, int amountValue)
    {
        id = idValue;
        amount = amountValue;
    }

}