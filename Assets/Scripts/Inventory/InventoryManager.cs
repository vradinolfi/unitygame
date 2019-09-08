using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    public Player player;

    private void Awake()
    {
        inventory.OnRightClickEvent += InventoryRightClick;
        equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;

        player = FindObjectOfType<Player>();
    }

    private void InventoryRightClick(ItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }
        else if (itemSlot.Item is UsableItem)
        {
            UsableItem usableItem = (UsableItem)itemSlot.Item;

            if (usableItem.name == "GreenHerb" && player.health >= player.maxHealth)
            {
                Debug.Log("Health full");
            }
            else
            {
                usableItem.Use(player);

                if (usableItem.IsConsumable)
                {
                    inventory.RemoveItem(usableItem);
                    usableItem.Destroy();
                }
            }
        }
    }

    private void EquipmentPanelRightClick(ItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem)itemSlot.Item);
        }
    }

    public void Equip(EquippableItem item)
    {
        //removes item from inventory which we might not want to do but we'll come back later for that
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            inventory.AddItem(item);
        }
    }

}
