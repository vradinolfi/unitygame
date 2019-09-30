using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Consumable,
    Equippable,
    Default
}
public abstract class ItemObject : ScriptableObject
{

    public GameObject prefab;
    public ItemType type;
    [Space]
    public string properName;
    [TextArea(3,10)]
    public string Description;
    public int amount = 1;

}
