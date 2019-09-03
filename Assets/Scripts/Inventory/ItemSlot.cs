using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] RawImage Image;
    [SerializeField] ItemTooltip tooltip;

    public event Action<Item> OnRightClickEvent;

    private Item _item;

    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;

            if (_item == null)
            {
                Image.enabled = false;
            }
            else
            {
                Image.texture = _item.Icon;
                Image.enabled = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Click");
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null && OnRightClickEvent != null)
            {
                OnRightClickEvent(Item);
            }
        }
    }

    protected virtual void OnValidate()
    {
        if (Image == null)
        {
            Image = GetComponent<RawImage>();
        }

        if(tooltip == null)
        {
            tooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*if (Item is EquippableItem)
        {
            tooltip.ShowTooltip((EquippableItem)Item);
        }*/

        tooltip.ShowTooltip(Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

}
