using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI pickUpText;

    private bool pickUpAllowed;
    private GameObject player;
    private bool paused = false;

    public bool persistent = false;

    // Start is called before the first frame update
    void Start()
    {
        pickUpText.gameObject.SetActive(false);
    }

    // Update is called once per frame
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
                    PickUp();
                }
            }

        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")) {
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickUpAllowed = false;
        }
    }

    void PickUp()
    {

        if (!persistent)
        {
            Destroy(gameObject);
            Destroy(pickUpText);
        }

        // crouch animation?

        // also probably add to inventory here later

    }

}
