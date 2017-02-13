using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    private Inventory inventory;
    private Crafting crafting;

    [SerializeField] private string toggleInventoryInput;
    [SerializeField] private string toggleCraftingInput;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Inventory Controller") != null && GameObject.Find("Crafting Controller") != null)
        {
            inventory = GameObject.Find("Inventory Controller").GetComponent<Inventory>();
            crafting = GameObject.Find("Crafting Controller").GetComponent<Crafting>();
        }
        else
        {
            // Throw an error message to the debug log and destroy this script
            Debug.LogError("Some or all of the UI is missing, UI system disabled");
            Destroy(this);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(toggleInventoryInput))
        {
            inventory.ToggleInventoryPanel();

            if (crafting.CraftingEnabled)
                crafting.ToggleCraftingPanel();
        }

        if (Input.GetButtonDown(toggleCraftingInput))
        {
            crafting.ToggleCraftingPanel();

            if (inventory.InventoryEnabled)
                inventory.ToggleInventoryPanel(); ;
        }
    }
}
