using UnityEngine;
using TMPro;

public class ItemSpawner : MonoBehaviour
{

    [SerializeField] Item item;
    [SerializeField] int amount = 1;
    [SerializeField] Inventory inventory;
    public GameObject textPopup;
    public GameObject choice1;
    public GameObject choice2;
    public AudioSource hit;


    private TextMeshProUGUI ingameText;
    private bool isInRange;
    private bool isPickedUp;
    private bool paused = false;

    Player player;

    private void Start()
    {
        ingameText = textPopup.GetComponent<TextMeshProUGUI>();

        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (!player.isAiming && Input.GetButtonDown("Submit"))
        {
            if (isInRange && !isPickedUp && !paused)
            {

                ingameText.text = "Will you take the <color=green><uppercase>" + item.ItemName + "</uppercase></color>?";
                textPopup.SetActive(true);
                Time.timeScale = 0;
                paused = true;

            }
            else
            {
                if (isInRange)
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

                    Time.timeScale = 1f;
                    paused = false;
                    textPopup.SetActive(false);
                    hit.Play();
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
