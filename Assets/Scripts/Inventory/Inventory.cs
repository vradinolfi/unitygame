using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public Item[] itemList = new Item[8];
    //public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public InventorySlot[] inventorySlots = new InventorySlot[8];


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        //inventorySlots = FindObjectsOfType<InventorySlot>();
        UpdateSlotUI();
    }

    private bool Add(Item item)
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = item;
                return true;
            }
        }
        return false;
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].UpdateSlot();
        }
    }

    public void AddItem(Item item)
    {
        bool hasAdded = Add(item);

        if (hasAdded)
        {
            UpdateSlotUI();
        }
    }

}
