using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    [SerializeField] Item item;
    [SerializeField] Inventory inventory;

    private bool isInRange;
    private bool isPickedUp;

    private void Update()
    {
        if (isInRange && Input.GetButtonDown("Interact"))
        {
            if (!isPickedUp)
            {
                inventory.AddItem(item);
                isPickedUp = true;
                this.gameObject.SetActive(false);

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
