using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable Object", menuName = "Inventory System/Items/Equippable")]
public class EquippableObject : ItemObject
{

    public float attackValue;

    public void Awake()
    {
        type = ItemType.Equippable;
    }
}
