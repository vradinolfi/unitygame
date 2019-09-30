using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{
    
    public Dialogue dialogue;

    bool activeDialogue;
    bool interactable = false;
    Player player;
    GameManager gameManager;
    Item item;

    void Start()
    {

        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        item = GetComponent<Item>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Submit"))
        {

                activeDialogue = dialogue.isActive;

                if (!player.isAiming)
                {

                    if (!activeDialogue && !gameManager.gamePaused && interactable)
                    {

                        TriggerDialogue(dialogue);

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

    public void TriggerDialogue(Dialogue dialogue)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        //Debug.Log("Trigger Dialogue");
    }

    void PickUp()
    {
        
        player.inventory.AddItem(item.item, 1);
        gameObject.SetActive(false);

        // crouch animation?

        // also probably add to inventory here later

    }

}
