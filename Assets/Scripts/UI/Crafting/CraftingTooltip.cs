using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CraftingTooltip : MonoBehaviour {

    private ItemDatabase itemDatabase;

    private GameObject tooltipPanel;    

    private Text tooltipText;           
    private Rect tooltipRect;           
    private Canvas canvas;              

    private CraftingIngredient[] craftingIngredients;                  

    // Use this for initialization
    void Start()
    {
        
        if (GameObject.Find("Crafting Tooltip Panel") != null && GameObject.Find("Crafting System") != null)
        {
            itemDatabase = GameObject.Find("Inventory Controller").GetComponent<ItemDatabase>();

            tooltipPanel = GameObject.Find("Crafting Tooltip Panel");              

            tooltipText = tooltipPanel.transform.GetChild(0).GetComponent<Text>();  
            tooltipRect = tooltipPanel.GetComponent<RectTransform>().rect;          

            canvas = GameObject.Find("Crafting System").GetComponent<Canvas>();    

            
            tooltipPanel.SetActive(false);
        }
        else
        {
            // Throw an error message to the debug log
            Debug.LogError("Some or all of the inventory UI is missing, inventory system disabled");
            
            Destroy(this);
            return;
        }
    }

    // Activates and updates the tooltip panel
    public void Activate(CraftingIngredient[] craftingIngredients, Inventory inventory)
    {
        tooltipPanel.SetActive(true);   // Shows the tooltip panel

        this.craftingIngredients = craftingIngredients;               

        ConstructTooltipString(inventory);       

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
    public void UpdateText(CraftingIngredient[] craftingIngredients, Inventory inventory)
    {
        this.craftingIngredients = craftingIngredients;

        ConstructTooltipString(inventory);
    }

    // Updates the text component's string
    void ConstructTooltipString(Inventory inventory)
    {
        // Update the text component's string based on
        tooltipText.text = "";

        for(int i = 0; i < craftingIngredients.Length; i++)
        {
            if (inventory.CheckItemCount(craftingIngredients[i].iD) >= craftingIngredients[i].amount)
                tooltipText.text += "<color=green>";
            else
                tooltipText.text += "<color=red>";

            tooltipText.text += inventory.CheckItemCount(craftingIngredients[i].iD) + "/" + craftingIngredients[i].amount + "</color> - " + itemDatabase.FetchItemFromID(craftingIngredients[i].iD).Name;

            if (craftingIngredients.Length - i != 1)
                tooltipText.text += "\n";
        }
    }
}
