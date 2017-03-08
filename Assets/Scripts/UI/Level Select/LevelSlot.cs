using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class LevelSlot : MonoBehaviour
{
    private LevelSelect levelSelect;    // Reference to the level select controller

    private LevelData level;            // The data of the current level

    public LevelData Level { get { return level; } set { level = value; } }
    
    // Use this for initialization
    void Start () {
        // Get the level select controller behaviour
        levelSelect = GameObject.Find("Level Select Controller").GetComponent<LevelSelect>();
	}

    // Sends the level data of this level to the level select controller
    public void SelectLevel()
    {
        // Update the currently selected level with this object's level data
        levelSelect.UpdateCurrentSelectedLevel(level);
    }
}
