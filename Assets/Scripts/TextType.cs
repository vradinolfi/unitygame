using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextType : MonoBehaviour
{
    
    private TextMeshProUGUI pickUpText;

    IEnumerator Start()
    {
        pickUpText = gameObject.GetComponent<TextMeshProUGUI>();

        int totalVisibleCharacters = pickUpText.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            pickUpText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
                yield return new WaitForSeconds(1.0f);

            counter += 1;

            yield return new WaitForSeconds(0.05f);
        }
    }
    
}
