using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Type : MonoBehaviour
{
    TextMeshProUGUI txt;
    string story;

    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();

        story = txt.text;

        txt.text = "";

        // TODO: add optional delay when to start
        StartCoroutine("PlayText");
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

// IDK