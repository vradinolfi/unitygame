using UnityEngine;
using UnityEngine.UI;

public class Customizable : MonoBehaviour {

    public Button Next;
    public Button Previous;
    public GameObject[] Items;
    public Text ItemName;
    public int currentItem;
	void Start () {
        Next.onClick.AddListener(delegate { NextItem(true); });
        Previous.onClick.AddListener(delegate { PreviousItem(true); });

        InitializedItems();
    }

    public void InitializedItems()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (currentItem == i)
            {
                Items[i].SetActive(true);
                ItemName.text = Items[i].name.ToString();
            }
            else
            {
                Items[i].SetActive(false);
            }
        }
    }

    public void NextItem(bool clicked)
    {
        currentItem++;
        if(currentItem == Items.Length)
        {
            currentItem = 0;
        }

        for(int i = 0; i < Items.Length; i++)
        {
            if(currentItem == i)
            {
                Items[i].SetActive(true);
                ItemName.text = Items[i].name.ToString();
            }
            else
            {
                Items[i].SetActive(false);
            }
        }
    }

    public void PreviousItem(bool clicked)
    {
        if(currentItem > 0)
        {
            currentItem--;
        }
        else
        {
            currentItem = Items.Length - 1;
        }
        for (int i = 0; i < Items.Length; i++)
        {
            if (currentItem == i)
            {
                Items[i].SetActive(true);
                ItemName.text = Items[i].name.ToString();
            }
            else
            {
                Items[i].SetActive(false);
            }
        }
    }
}
