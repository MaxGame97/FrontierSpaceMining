using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CraftingTooltip : MonoBehaviour {

    private ItemDatabase itemDatabase;                  // The item database

    private GameObject tooltipPanel;                    // The crafting tooltip panel object

    private Text tooltipText;                           // The tooltip's text UI
    private Rect tooltipRect;                           // The tooltip's rect UI
    private Canvas canvas;                              // The crafting system's canvas

    // Use this for initialization
    void Start()
    {
        //If the inventory system exists
        if(GameObject.Find("Inventory Controller") != null)
        {
            // Get the item database
            itemDatabase = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>();
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the inventory system is missing, crafting tooltip disabled");

            Destroy(this);
            return;
        }

        // If the crafting system exists
        if (GameObject.Find("Crafting Tooltip Panel") != null && GameObject.Find("Crafting System") != null)
        {
            // Get the crafting tooltip panel
            tooltipPanel = GameObject.Find("Crafting Tooltip Panel");              

            // Get the text and the rect from the crafting tooltip panel
            tooltipText = tooltipPanel.transform.GetChild(0).GetComponent<Text>();  
            tooltipRect = tooltipPanel.GetComponent<RectTransform>().rect;          

            // Get the crafting system's canvas
            canvas = GameObject.Find("Crafting System").GetComponent<Canvas>();    

            // Hides the tooltip panel as default
            tooltipPanel.SetActive(false);
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the crafting system is missing, crafting tooltip disabled");
            
            Destroy(this);
            return;
        }
    }

    // Activates and updates the tooltip panel
    public void Activate(CraftingIngredient[] craftingIngredients, Inventory inventory, Item resultItem)
    {
        tooltipPanel.SetActive(true);   // Shows the tooltip panel             

        ConstructTooltipString(inventory, resultItem, craftingIngredients);       

        // Move the tooltip panel to be slightly to the left of the cursor
        tooltipPanel.transform.position = new Vector3(Input.mousePosition.x - ((tooltipRect.width * canvas.scaleFactor) / 1.5f), Input.mousePosition.y, 0f);
    }

    // Deactivates the tooltip panel
    public void Deactivate()
    {
        // Hides the tooltip panel
        tooltipPanel.SetActive(false);
    }

    // Updates the tooltip panel (Doesn't move it from where it was)
    public void UpdateText(CraftingIngredient[] craftingIngredients, Inventory inventory, Item resultItem)
    {
        ConstructTooltipString(inventory, resultItem, craftingIngredients);
    }

    // Updates the text component's string
    void ConstructTooltipString(Inventory inventory, Item resultItem, CraftingIngredient[] craftingIngredients)
    {
        // Update the text component's string based on the resulting item data
        tooltipText.text = "<size=14>" + resultItem.Name + "</size><size=4>\n\n</size>" + resultItem.Description + "<size=4>\n\n</size>";

        // Check all the crafting ingredients
        for (int i = 0; i < craftingIngredients.Length; i++)
        {
            // Show the amount of items in the inventory as well as the amount needed, in green if there are enouth, in red if there aren't
            if (inventory.CheckItemCount(craftingIngredients[i].iD) >= craftingIngredients[i].amount)
                tooltipText.text += "<color=green>";
            else
                tooltipText.text += "<color=red>";
            
            tooltipText.text += inventory.CheckItemCount(craftingIngredients[i].iD) + "/" + craftingIngredients[i].amount + "</color> - " + itemDatabase.FetchItemFromID(craftingIngredients[i].iD).Name;

            // Unless it's the last item in the recipe, add a line break
            if (craftingIngredients.Length - i != 1)
                tooltipText.text += "\n";
        }
    }
}
