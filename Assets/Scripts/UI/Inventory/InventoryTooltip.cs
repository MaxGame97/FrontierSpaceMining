using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class InventoryTooltip : MonoBehaviour {

    private GameObject tooltipPanel;    // The tooltip panel

    private Text tooltipText;           // The text component of the tooltip

    //private Rect tooltipRect;           // The rectangle object of the tooltip panel
    private Rect canvasRect;

    private Canvas canvas;              // The UI canvas component

    private Item item;                  // The current item hovered over

	// Use this for initialization
	void Start () {
        // If all inventory and tooltip UI exists
        if (GameObject.Find("Inventory Tooltip Panel") != null && GameObject.Find("Inventory System") != null)
        {
            tooltipPanel = GameObject.Find("Inventory Tooltip Panel");              // Find the tooltip panel object
        
            tooltipText = tooltipPanel.transform.GetChild(0).GetComponent<Text>();  // Get the text component from the tooltip panel's child object

            //tooltipRect = tooltipPanel.GetComponent<RectTransform>().rect;          // Get the rectangle object from the tooltip panel
            canvasRect = GameObject.Find("Inventory System").GetComponent<RectTransform>().rect;

            canvas = GameObject.Find("Inventory System").GetComponent<Canvas>();    // Find the UI canvas and get its canvas component

            // Hides the tooltip panel
            tooltipPanel.SetActive(false);
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the inventory UI is missing, inventory system disabled");
            // If the inventory is missing, delete the item pickup behaviour and exit this function
            Destroy(this);
            return;
        }
    }

    // Activates and updates the tooltip panel
    public void Activate(Item item)
    {
        tooltipPanel.SetActive(true);   // Shows the tooltip panel

        this.item = item;               // Update the current item hovered over

        ConstructTooltipString();       // Update the text component's string

        float verticalOffset = ((tooltipText.preferredHeight / 2f) * canvas.scaleFactor) + 30f;

        if (Input.mousePosition.y > canvasRect.height / 2f)
            verticalOffset = -verticalOffset;

        // Move the tooltip panel to be slightly to the left of the cursor
        tooltipPanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + verticalOffset, 0f);
    }

    // Deactivates the tooltip panel
    public void Deactivate()
    {
        // Hides the tooltip panel
        tooltipPanel.SetActive(false);
    }

    // Updates the text component's string
    void ConstructTooltipString()
    {
        // Update the text component's string based on the current item values
        tooltipText.text = "<size=14>" + item.Name + "</size><size=4>\n\n</size>" + item.Description;
    }
}
