using UnityEngine;

public enum EquipmentType
{
    Weapon1,
    Weapon2,
    Accessory
}

[CreateAssetMenu]
public class EquippableItem : Item
{

    public int StrengthBonus;
    [Space]
    public float StrengthPercentBonus;
    [Space]
    public EquipmentType EquipmentType;

}
