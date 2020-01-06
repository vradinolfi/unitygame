using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookController : MonoBehaviour
{
    public ReadNote readNote;
    Transform book;
    GameObject nextBtn;
    GameObject prevBtn;
    GameObject exitBtn;
    
    int numPages;
    int currentPage = 0;

    void Start()
    {
        book = this.transform.Find("Pages");

        //print(book);

        numPages = book.childCount;

        //print(numPages);

        //Debug.Log(pages);

        nextBtn = this.transform.Find("NextButton").gameObject;
        prevBtn = this.transform.Find("PrevButton").gameObject;
        exitBtn = this.transform.Find("ExitButton").gameObject;
    }

    private void Update()
    {
        if (currentPage > 0 && numPages > 1)
        {
            prevBtn.SetActive(true);
        }
            else
        {
            prevBtn.SetActive(false);
        }

        if (currentPage == (numPages - 1))
        {
            exitBtn.SetActive(true);
            nextBtn.SetActive(false);

        }
            else
        {
            exitBtn.SetActive(false);
            nextBtn.SetActive(true);
        }
    }

    private void OnEnable()
    {
        /*GameObject[] pages = new GameObject[numPages];

        for (int i = 0; i < numPages; i++)
        {
            pages[i] = book.GetChild(i).gameObject;
        }*/
        //print(currentPage);
        //print(pages[currentPage]);
        //pages[currentPage].GetComponent<Animation>().Play("PageSlideInLeft");
        //book.GetChild(0).gameObject.GetComponent<Animation>().Play("PageSlideInLeft");

    }

    public void NextPage()
    {
        GameObject[] pages = new GameObject[numPages];

        for (int i = 0; i < numPages; i++)
        {
            pages[i] = book.GetChild(i).gameObject;
        }

        if (currentPage < (numPages - 1))
        {

            pages[currentPage].SetActive(false);

            readNote.PlayFlip();

            currentPage += 1;

            pages[currentPage].SetActive(true);

        }

        //print(currentPage);
    }

    public void PrevPage()
    {

        GameObject[] pages = new GameObject[numPages];

        for (int i = 0; i < numPages; i++)
        {
            pages[i] = book.GetChild(i).gameObject;
        }

        if (currentPage != 0)
        {

            pages[currentPage].SetActive(false);

            readNote.PlayFlip();

            currentPage -= 1;

            pages[currentPage].SetActive(true);

        }

    }

    public void Exit()
    {
        //noteView.SetActive(false);
        readNote.Inspect();

        ResetBook();

    }

    public void ResetBook()
    {
        currentPage = 0;

        GameObject[] pages = new GameObject[numPages];

        for (int i = 0; i < numPages; i++)
        {
            pages[i] = book.GetChild(i).gameObject;
        }

        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        pages[0].SetActive(true);
    }
}
