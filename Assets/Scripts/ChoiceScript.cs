using UnityEngine;
using TMPro;

public class ChoiceScript : MonoBehaviour
{

    public GameObject text;
    public GameObject choice1;
    public GameObject choice2;
    public int choiceMade;

    public void Choice1()
    {
        //text.GetComponent<TextMeshProUGUI>().text = "Choice1 selected";
        choiceMade = 1;
    }

    public void Choice2()
    {
        //text.GetComponent<TextMeshProUGUI>().text = "Choice2 selected";
        choiceMade = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (choiceMade >= 1)
        {
            choice1.SetActive(false);
            choice2.SetActive(false);
        }
    }
}
