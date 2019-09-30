using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPuzzle : MonoBehaviour
{
    public GameObject puzzleDoor;
    public Dialogue dialogue;

    Player player;
    bool interactable;
    bool hasItems;
    bool activeDialogue;

    bool green;
    bool yellow;
    bool purple;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {

            activeDialogue = dialogue.isActive;

            if (!activeDialogue && interactable && !player.isAiming)
            {
                if (hasItems)
                {

                    GetComponent<DialogueTrigger>().enabled = false;

                    SolvePuzzle();
                }
            }

            if (activeDialogue)
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogue);
            }
        }
    }

    void SolvePuzzle()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

        puzzleDoor.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = true;

            for (int i = 0; i < player.inventory.Container.Count; i++)
            {
                if (player.inventory.Container[i].item.name == "GreenBlock")
                {
                    green = true;
                }
                if (player.inventory.Container[i].item.name == "YellowBlock")
                {
                    yellow = true;
                }
                if (player.inventory.Container[i].item.name == "PurpleBlock")
                {
                    purple = true;
                }
            }

            if(green && purple && yellow)
            {
                hasItems = true;
            }
            else
            {
                hasItems = false;
            }

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = false;
        }
    }
}
