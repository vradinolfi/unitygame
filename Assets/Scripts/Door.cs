using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{


    public Transform destination;
    //public string doorText;
    public bool isLocked;

    public GameObject blackScreen;
    public float waitTime = 1f;

    Animator fadeAnim;
    Player player;
    GameManager gameManager;
    bool interactable;

    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        fadeAnim = blackScreen.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && !player.isAiming)
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        if (interactable)
        {

            blackScreen.SetActive(true);
            gameManager.PauseGame();
            fadeAnim.SetBool("isFaded", true);

            yield return new WaitForSecondsRealtime(waitTime);

            gameManager.UnpauseGame();
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = destination.transform.Find("EnterPoint").position;
            player.GetComponent<CharacterController>().enabled = true;
            interactable = false;
            fadeAnim.SetBool("isFaded", false);

            yield return new WaitForSecondsRealtime(waitTime);

            blackScreen.SetActive(false);

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
}
