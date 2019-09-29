using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    public ItemDatabaseObject database;
    TextMeshProUGUI nameTooltip;
    TextMeshProUGUI descTooltip;

    // Start is called before the first frame update
    void Start()
    {
        nameTooltip = GameObject.Find("/Canvas/InventoryBG/ItemTooltip/Item Name Text").GetComponent<TextMeshProUGUI>();
        descTooltip = GameObject.Find("/Canvas/InventoryBG/ItemTooltip/Item Slot Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        nameTooltip.gameObject.SetActive(true);
        descTooltip.gameObject.SetActive(true);

        var name = this.name;

        name = name.Replace("(Clone)","");

        for (int i=0; i < database.Items.Length; i++)
        {

            if (database.Items[i].name == name)
            {
                nameTooltip.text = database.Items[i].properName;
                descTooltip.text = database.Items[i].Description;
            }
        }

    }
}
