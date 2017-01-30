using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private int inventorySlotCount = 20;

    private ItemDatabase itemDatabase;

    private GameObject inventoryPanel;
    private GameObject slotPanel;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

	// Use this for initialization
	void Start () {
        itemDatabase = GetComponent<ItemDatabase>();

        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = GameObject.Find("Slot Panel");

        for(int i = 0; i < inventorySlotCount; i++)
        {
            items.Add(new Item());

            slots.Add(Instantiate(inventorySlotPrefab));
            slots[i].transform.SetParent(slotPanel.transform);
        }

        AddItem(0);
        AddItem(1);
        AddItem(0);
        AddItem(2);
    }

    public void AddItem(int iD)
    {
        Item tempItem = itemDatabase.FetchItemFromID(iD);

        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].ID == -1)
            {
                items[i] = tempItem;

                GameObject tempItemObject = Instantiate(inventoryItemPrefab);
                tempItemObject.GetComponent<Image>().sprite = tempItem.Sprite;
                tempItemObject.transform.SetParent(slots[i].transform);
                tempItemObject.transform.position = Vector3.zero;

                tempItemObject.name = tempItem.Name + " (Item)";

                break;
            }
        }
    }
}
