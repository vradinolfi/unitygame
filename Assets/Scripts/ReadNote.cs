﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadNote : MonoBehaviour
{

    [SerializeField] string text;
    public GameObject noteView;
    public GameObject note;
    public AudioSource pageflip;

    private TextMeshProUGUI noteText;
    private bool isInRange;
    private bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        noteText = noteView.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {

             Inspect();

        }
    }

    private void Inspect()
    {
        if (isInRange && !gamePaused)
        {

            noteText.text = text;
            note.SetActive(true);
            Time.timeScale = 0;
            gamePaused = true;
            pageflip.Play();

        }
        else
        {
            if (isInRange)
            {

                Time.timeScale = 1f;
                gamePaused = false;
                note.SetActive(false);

            }
        }
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