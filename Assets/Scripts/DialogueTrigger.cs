using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    private bool activeDialogue;
    private bool isInRange = false;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {

        activeDialogue = dialogue.isActive;

        if (!player.isAiming && Input.GetButtonDown("Submit") && isInRange && !activeDialogue)
        {
            TriggerDialogue();
        }

        if (activeDialogue && Input.GetButtonDown("Submit"))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogue);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
