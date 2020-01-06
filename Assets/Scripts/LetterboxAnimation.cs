using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterboxAnimation : MonoBehaviour
{

    public GameObject letterbox;
    public Animator letterboxTop;
    public Animator letterboxBottom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            letterbox.SetActive(true);

            letterboxTop.ResetTrigger("slideOut");
            letterboxTop.SetTrigger("slideIn");

            letterboxBottom.ResetTrigger("slideOut");
            letterboxBottom.SetTrigger("slideIn");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            StartCoroutine(SlideOut());

        }
    }

    IEnumerator SlideOut()
    {
        letterboxTop.ResetTrigger("slideIn");
        letterboxTop.SetTrigger("slideOut");

        letterboxBottom.ResetTrigger("slideIn");
        letterboxBottom.SetTrigger("slideOut");

        Debug.Log("before wait after triggers");

        yield return new WaitForSeconds(1);

        Debug.Log("waited for 60 seconds");

        letterbox.SetActive(false);
    }
}
