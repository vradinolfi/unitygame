using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{

    public Texture icon;
    public Inventory inventory;
    public Item item;

    public string type;
    public float healValue;

    /*
    private bool pickUpAllowed;
    private bool paused = false;

    [SerializeField]
    private TextMeshProUGUI pickUpText;

    void Start()
    {
        pickUpText.gameObject.SetActive(false);

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void Update()
    {

        if (Input.GetButtonDown("Interact"))
        {
            if (!paused && pickUpAllowed)
            {
                paused = true;
                pickUpText.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                if (pickUpAllowed)
                {
                    paused = false;
                    pickUpText.gameObject.SetActive(false);
                    Time.timeScale = 1f;
                }
            }

        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickUpAllowed = false;
        }
    }*/
}