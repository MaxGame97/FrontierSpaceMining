using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class LevelSlot : MonoBehaviour, IPointerDownHandler
{
    private LevelSelect levelSelect;    // Reference to the level select controller

    private LevelData level;            // The data of the current level

    private bool selected = false;

    private Color selectedColor = new Color(0.8f, 0.8f, 0.8f);
    private Color color = new Color(1.0f, 1.0f, 1.0f);

    public LevelData Level { get { return level; } set { level = value; } }
    public bool Selected { set { selected = value; } }
    
    // Use this for initialization
    void Start () {
        // Get the level select controller behaviour
        levelSelect = GameObject.Find("Level Select Controller").GetComponent<LevelSelect>();

        if (levelSelect.CurrentSelectedLevel == level)
        {
            selected = true;
        }
    }


    void Update()
    {
        if(levelSelect.CurrentSelectedLevel != level)
        {
            selected = false;
        }

        if (selected)
        {
            gameObject.GetComponent<CanvasRenderer>().SetColor(selectedColor);
        }else
        {
            gameObject.GetComponent<CanvasRenderer>().SetColor(color);
        }
    }

    // If this object has been pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        // Update the currently selected level with this object's level data
        levelSelect.UpdateCurrentSelectedLevel(level);

        selected = true;
    }
}
