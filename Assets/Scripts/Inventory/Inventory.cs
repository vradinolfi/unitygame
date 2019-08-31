using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public GameObject inventory;
    public GameObject slotHandler;
    public GameObject itemManager;

    private bool inventoryEnabled;

    private int slots;
    private Transform[] slot;

    private GameObject itemPickedUp;
    private bool itemAdded;

    public void Start()
    {
        inventoryEnabled = true;
        // slots being detected
        slots = slotHandler.transform.childCount;
        slot = new Transform[slots];

        DetectInventorySlots();
    }

    public void Update()
    {

        if (Input.GetButtonDown("Inventory"))
        {
            inventoryEnabled = !inventoryEnabled;
        }

        if (inventoryEnabled)
        {
            inventory.SetActive(true);
            //inventory.GetComponent<Canvas>.enabled = true;
        }
        else
        {
            inventory.SetActive(false);
            //inventory.GetComponent<Canvas>.enabled = false;
        }

    }
    
    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Item"))
        {
            //print("in trigger");
            itemPickedUp = other.gameObject;
            AddItem(itemPickedUp);
            print(itemPickedUp);
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemAdded = false;
        }
    }
    
    public void AddItem(GameObject item)
    {
        //print("step1");
        for (int i = 0; i < slots; i++)
        {
            //print("step2");
            if (slot[i].GetComponent<Slot>().empty && itemAdded == false)
            {
                //print("step3");
                slot[i].GetComponent<Slot>().item = itemPickedUp;
                slot[i].GetComponent<Slot>().itemIcon = itemPickedUp.GetComponent<Item>().icon;

                item.transform.parent = itemManager.transform;
                item.transform.position = itemManager.transform.position;

                if (item.GetComponent<MeshRenderer>())
                {
                    item.GetComponent<MeshRenderer>().enabled = false;
                }

                Destroy(item.GetComponent<Rigidbody>());

                itemAdded = true;
                //print("item added");
            }
        }
    }

    public void DetectInventorySlots()
    {

        inventory.SetActive(true);

        for (int i = 0; i < slots; i++)
        {
            slot[i] = slotHandler.transform.GetChild(i);
            //print(slot[i].name);
        }

        inventoryEnabled = false;
        inventory.SetActive(false);
    }
}
