using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public ItemDatabaseObject database;
    TextMeshProUGUI nameTooltip;
    TextMeshProUGUI descTooltip;

    GameObject weaponSlot;
    

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        nameTooltip = GameObject.Find("/Canvas/InventoryBG/ItemTooltip/Item Name Text").GetComponent<TextMeshProUGUI>();
        descTooltip = GameObject.Find("/Canvas/InventoryBG/ItemTooltip/Item Slot Text").GetComponent<TextMeshProUGUI>();
        weaponSlot = GameObject.Find("/Canvas/InventoryBG/EquipmentPanel/RawImage/Weapon1 Slot");
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Gun>().equipPistol == true)
        {
            weaponSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.GetComponent<Gun>().currentAmmo.ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var name = this.name;

        name = name.Replace("(Clone)", "");

        if (name == "Beretta92FS")
        {
            Debug.Log("clicked equippable");
            Debug.Log(name);

            if (player.GetComponent<Gun>().equipPistol == true)
            {
                player.GetComponent<Gun>().equipPistol = false;
                Destroy(weaponSlot.transform.GetChild(0).gameObject);
            }
            else
            {
                player.GetComponent<Gun>().equipPistol = true;
                Instantiate(this.gameObject, weaponSlot.transform);
                weaponSlot.transform.GetChild(0).transform.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                weaponSlot.transform.GetChild(0).transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nameTooltip.gameObject.SetActive(true);
        descTooltip.gameObject.SetActive(true);

        var name = this.name;

        name = name.Replace("(Clone)", "");

        for (int i = 0; i < database.Items.Length; i++)
        {

            if (database.Items[i].name == name)
            {
                nameTooltip.text = database.Items[i].properName;
                descTooltip.text = database.Items[i].Description;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nameTooltip.gameObject.SetActive(false);
        descTooltip.gameObject.SetActive(false);
    }
}
