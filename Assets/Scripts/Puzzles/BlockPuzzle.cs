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
                if (player.transform.Find("Items").Find("GreenBlock").name == "GreenBlock" &&
                    player.transform.Find("Items").Find("YellowBlock").name == "YellowBlock" &&
                    player.transform.Find("Items").Find("PurpleBlock").name == "PurpleBlock")
                {
                    hasItems = true;

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
