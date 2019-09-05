using UnityEngine;

public enum EquipmentType
{
    Weapon1,
    Weapon2,
    Accessory
}

[CreateAssetMenu(menuName = "Items/Equippable Item")]
public class EquippableItem : Item
{

    //public int StrengthBonus;
    //[Space]
    //public float StrengthPercentBonus;
    [Space]
    public EquipmentType EquipmentType;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

}
