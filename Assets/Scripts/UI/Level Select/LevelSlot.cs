using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class LevelSlot : MonoBehaviour, IPointerDownHandler
{
    private LevelSelect levelSelect;

    private LevelData level;

    public LevelData Level { get { return level; } set { level = value; } }
    
    // Use this for initialization
    void Start () {
        levelSelect = GameObject.Find("Level Select Controller").GetComponent<LevelSelect>();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        levelSelect.UpdateCurrentSelectedLevel(level);
    }
}
