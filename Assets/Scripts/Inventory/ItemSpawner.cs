using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    [SerializeField] Item item;
    [SerializeField] int amount = 1;
    [SerializeField] Inventory inventory;

    private bool isInRange;
    private bool isPickedUp;

    private void Update()
    {
        if (isInRange && !isPickedUp && Input.GetButtonDown("Interact"))
        {

            for (int i = amount; i > 0; i--)
            {
                Item itemCopy = item.GetCopy();

                if (inventory.AddItem(item.GetCopy()))
                {
                    amount--;
                    if (amount == 0)
                    {
                        isPickedUp = true;
                        this.gameObject.SetActive(false);
                    }
                }
                else
                {
                    itemCopy.Destroy();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

}
