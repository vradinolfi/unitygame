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
    [TextArea(3,10)]
    public string Description;

}
