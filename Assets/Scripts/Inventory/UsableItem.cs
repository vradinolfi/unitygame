using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Usable Item")]
public class UsableItem : Item
{
    public bool IsConsumable;

    public List<UsableItemEffect> Effects;

    public virtual void Use(Player player)
    {
        foreach(UsableItemEffect effect in Effects)
        {
            effect.ExecuteEffect(this, player);
        }

    }
}
