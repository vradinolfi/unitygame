public class LostItemReaction : DelayedReaction
{
    public Item item;


    private Inventory inventory;


    protected override void SpecificInit()
    {
        inventory = FindObjectOfType<Inventory> ();
    }


    protected override void ImmediateReaction()
    {
        inventory.RemoveItem (item);
    }
}
