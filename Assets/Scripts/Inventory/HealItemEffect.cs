using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
    public float HealAmount;

    public override void ExecuteEffect(UsableItem parentItem, Player player)
    {
        player.health += HealAmount;
        if (player.health > player.maxHealth)
        {
            player.health = player.maxHealth;
        }
    }
}
