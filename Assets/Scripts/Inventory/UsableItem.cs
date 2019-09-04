using UnityEngine;

[CreateAssetMenu]
public class UsableItem : Item
{
    public bool IsConsumable;

    public virtual void Use(Player player)
    {

    }
}
