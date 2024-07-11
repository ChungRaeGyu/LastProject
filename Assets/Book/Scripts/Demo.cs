using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public BookController bookController;
    
    [SerializeField]
    Button nextButton;
    [SerializeField]
    Button previousButton;
    [SerializeField]
    Button BookOpenButton;
    [SerializeField]
    Button GaChaOpenButton;
    [SerializeField]
    Image bookImage;
    [SerializeField]
    Sprite bookTexture;
    [SerializeField]
    Sprite notepadTexture;

    public GameObject[] pages;
    public BookAnimation bookAnim;
    int currentPage;
    View currentView;


    public enum View
    {
        Book,
        Notepad
    }

    void Start()
    {
        UpdatePage();

        nextButton.onClick.AddListener(NextPage);
        bookAnim.animator.SetBool("Open", false);
        previousButton.onClick.AddListener(PreviousPage);
        BookOpenButton.onClick.AddListener(BookPage);
        GaChaOpenButton.onClick.AddListener(GachaPage);
    }

    public void SetBook(bool value)
    {
        SetView(value ? View.Book : View.Notepad);
    }

    void SetView(View value)
    {
        if (currentView == value) return;

        currentView = value;
        bookImage.sprite = currentView == View.Book ? bookTexture : notepadTexture;
    }
    void GachaPage()
    {
        if (currentPage == 1) return;
        if (currentPage > 1)
        {
            bookController.PreviousPage();
        }
        else
        {
            bookController.NextPage();
        }
        currentPage = 1;
        StartCoroutine(UpdatePageDelayed());
    }
    void BookPage()
    {
        if (currentPage == 2) return;
        if (currentPage > 2)
        {
            bookController.PreviousPage();
        }
        else
        {
            bookController.NextPage();
        }
        currentPage = 2;
        StartCoroutine(UpdatePageDelayed());
    }
    void NextPage()
    {
        if (currentPage == 1)
        {
            bookAnim.animator.SetBool("Open", true);
        }
        if (currentPage == 3)
        {
            bookAnim.animator.SetBool("Open", false);
        }
        bookController.NextPage();
        currentPage = Mathf.Min(++currentPage, pages.Length - 1);
        StartCoroutine(UpdatePageDelayed());
    }

    void PreviousPage()
    {
        if (currentPage == 2)
        {
            bookAnim.animator.SetBool("Open", false);
        }
        if (currentPage == 4)
        {
            bookAnim.animator.SetBool("Open", true);
        }
        bookController.PreviousPage();
        currentPage = Mathf.Max(--currentPage, 0);
        StartCoroutine(UpdatePageDelayed());
    }
    
    IEnumerator UpdatePageDelayed()
    {
        yield return new WaitForEndOfFrame();
        UpdatePage();
    }
    
    void UpdatePage()
    {
        Array.ForEach(pages, c => { c.SetActive(false);});
        pages[currentPage].SetActive(true);
        nextButton.gameObject.SetActive(currentPage < pages.Length - 1);
        previousButton.gameObject.SetActive(currentPage > 0);
    }
}
