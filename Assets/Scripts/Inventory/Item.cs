using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    public string itemName = "New Item";
    public string itemDescription = "New Description";
    public Sprite icon;
    public int price = 0;
    public enum Type { Default, Consumable, Weapon, Ammunition, Key };
    public Type type = Type.Default;

}
