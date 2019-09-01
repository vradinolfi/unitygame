using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI pickUpText;

    public bool interactable = false;

    private Player playerScript;

    //public Animator anim;
    private bool paused = false;
    //private GameObject player;

    public bool persistent = false;

    // Start is called before the first frame update
    void Start()
    {
        pickUpText.gameObject.SetActive(false);

        playerScript = FindObjectOfType<Player>();
        
        //anim = playerScript.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Interact"))
        {
            if (!paused && interactable)
            {
                playerScript.GetComponent<Animator>().SetBool("IsCrouching", true);
                paused = true;
                pickUpText.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                if (interactable)
                {
                    playerScript.GetComponent<Animator>().SetBool("IsCrouching", false);
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
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = false;
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
