using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{

    public Transform destination;
    [Space]
    public bool isLocked;
    public string doorKey;
    public Dialogue dialogue;
    public Dialogue unlockText;
    [Space]
    public GameObject blackScreen;
    public float waitTime = 1f;
    [Space]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;
    public AudioClip unlockSound;

    AudioSource doorAudio;
    private bool activeDialogue;
    Animator fadeAnim;
    Player player;
    GameManager gameManager;
    DialogueManager dialogueManager;
    bool interactable;
    bool hasKey;
    bool teleporting;


    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        fadeAnim = blackScreen.GetComponent<Animator>();
        doorAudio = GetComponentInChildren<AudioSource>();
        dialogueManager = FindObjectOfType<DialogueManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Submit"))
        {
            activeDialogue = dialogue.isActive || unlockText.isActive;

            if (!player.isAiming && !isLocked && !activeDialogue && !teleporting)
            {
                StartCoroutine(Teleport());
            }

            if (!player.isAiming && !hasKey && isLocked && !activeDialogue)
            {
                TriggerDialogue();
            }

            if (isLocked && activeDialogue)
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogue);
            }

            if (!player.isAiming && hasKey && isLocked && !activeDialogue)
            {
                UnlockDoor();
            }

            if (!isLocked && activeDialogue)
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence(unlockText);
            }
        }
    }

    IEnumerator Teleport()
    {
        if (interactable)
        {
            teleporting = true;

            blackScreen.SetActive(true);
            gameManager.PauseGame();
            fadeAnim.SetBool("isFaded", true);
            doorAudio.clip = openSound;
            doorAudio.Play();
            
            yield return new WaitForSecondsRealtime(waitTime);
            //yield return new WaitForSecondsRealtime(doorAudio.clip.length);
            doorAudio.clip = closeSound;
            doorAudio.Play();
            gameManager.UnpauseGame();
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = destination.transform.Find("EnterPoint").position;
            player.GetComponent<CharacterController>().enabled = true;
            interactable = false;
            fadeAnim.SetBool("isFaded", false);

            yield return new WaitForSecondsRealtime(waitTime);

            blackScreen.SetActive(false);

            teleporting = false;

        }
    }

    public void TriggerDialogue()
    {
        if (interactable)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            doorAudio.clip = lockedSound;
            doorAudio.Play();
        }
    }

    public void UnlockDoor()
    {
        if (interactable)
        {
            isLocked = false;
            destination.GetComponentInParent<Door>().isLocked = false;
            doorAudio.clip = unlockSound;
            doorAudio.Play();
            FindObjectOfType<DialogueManager>().StartDialogue(unlockText);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = true;

            if (player.transform.Find("Items").Find(doorKey).gameObject.name == doorKey) {
                hasKey = true;
                Debug.Log("Has " + doorKey);
            }

            //Debug.Log("/Player/" + doorKey.name);
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
