using UnityEngine;

public enum EquipmentType
{
    Weapon1,
    Weapon2,
    Accessory
}

public enum AmmoType
{
    _9mm,
    _12Gauge,
    _45Cal
}

[CreateAssetMenu(menuName = "Items/Equippable Item")]
public class EquippableItem : Item
{

    //public int StrengthBonus;
    //[Space]
    //public float StrengthPercentBonus;
    public int maxAmmo;
    public int startingAmmo;

    [Space]
    public EquipmentType EquipmentType;
    [Space]
    public AmmoType AmmoType;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

}
