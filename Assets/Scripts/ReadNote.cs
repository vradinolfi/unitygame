using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadNote : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject noteView;
    public GameObject note;
    public AudioSource pageflip;

    private bool isInRange;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (!player.isAiming && Input.GetButtonDown("Submit"))
        {

             Inspect();

        }
    }

    public void Inspect()
    {
        if (isInRange && !gameManager.gamePaused)
        {
            noteView.SetActive(true);
            note.SetActive(true);
            gameManager.PauseGame();
            pageflip.Play();

        }
        else
        {
            if (isInRange)
            {

                gameManager.UnpauseGame();
                note.SetActive(false);
                noteView.SetActive(false);
                note.GetComponent<BookController>().ResetBook();

            }
        }
    }

    public void PlayFlip()
    {
        pageflip.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
