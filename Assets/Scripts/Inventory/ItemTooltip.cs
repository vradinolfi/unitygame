using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI ItemNameText;
    [SerializeField] TextMeshProUGUI ItemSlotText;
    //[SerializeField] TextMeshProUGUI ItemStatsText;

    public void ShowTooltip(Item item)
    {
        ItemNameText.text = item.ItemName;

        ItemSlotText.text = item.ItemDescription;

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

}
