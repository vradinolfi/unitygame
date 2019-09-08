using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class MenuButtonController : Button
{

    public void Update()
    {
        Button button = this.GetComponent<Button>();
        TextMeshProUGUI text = (TextMeshProUGUI)button.GetComponentInChildren<TextMeshProUGUI>();

        ColorBlock colorBlock = this.colors;

        if (this.currentSelectionState == SelectionState.Normal)
        {
            text.color = colorBlock.normalColor;
        }
        else if (this.currentSelectionState == SelectionState.Highlighted)
        {
            text.color = colorBlock.highlightedColor;
        }
        else if (this.currentSelectionState == SelectionState.Pressed)
        {
            text.color = colorBlock.pressedColor;
        }
        else if (this.currentSelectionState == SelectionState.Disabled)
        {
            text.color = colorBlock.disabledColor;
        }
    }
}