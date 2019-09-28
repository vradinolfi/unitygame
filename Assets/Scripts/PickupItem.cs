using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{

    public Dialogue dialogue;

    private bool activeDialogue;
    bool interactable = false;
    Player player;
    GameManager gameManager;

    void Start()
    {

        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Submit"))
        {
            activeDialogue = dialogue.isActive;

            Debug.Log(activeDialogue);

            if (!player.isAiming)
            {

                if (!activeDialogue && !gameManager.gamePaused && interactable)
                {

                    TriggerDialogue();

                }
                
                if (activeDialogue)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogue);
                    PickUp();
                }

            }

        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
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

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        //Debug.Log("Trigger Dialogue");
    }

    void PickUp()
    {

        transform.SetParent(player.transform.Find("Items").transform);
        gameObject.SetActive(false);

        // crouch animation?

        // also probably add to inventory here later

    }

}
