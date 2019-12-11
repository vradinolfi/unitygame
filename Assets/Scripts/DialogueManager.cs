using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public GameObject dialogueContainer;
    public TextMeshProUGUI dialogueText;

    public bool isBusy;

    private Queue<string> sentences;

    bool done;

    GameManager gameManager;

    void Start()
    {
        sentences = new Queue<string>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //isBusy = true; //wtf
        dialogue.isActive = true;
        dialogueContainer.SetActive(true);
        gameManager.PauseGame();

        //Debug.Log("Starting conversation with " + dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(dialogue);
    }

    public void DisplayNextSentence(Dialogue dialogue)
    {
        if (sentences.Count == 0)
        {
            EndDialogue(dialogue);
            return;
        }

        string sentence = sentences.Dequeue();

        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {

            dialogueText.text += letter;
            //Debug.Log(dialogueText.text);
            //yield return null;
            yield return new WaitForSecondsRealtime(0.03f);
        }
    }

    public bool EndDialogue(Dialogue dialogue)
    {
        if (dialogue.needsConfirmation)
        {
            return true;
        }
        else
        {
            dialogueContainer.SetActive(false);
            dialogue.isActive = false;
            gameManager.UnpauseGame();
            //isBusy = false;
            return true;
        }
        
    }

}
