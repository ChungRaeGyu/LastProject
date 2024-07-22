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
    bool isChangePage = false;

    // 색상 설정
    [SerializeField]
    Color highlightColor = Color.yellow; // 현재 페이지를 나타내는 색상
    [SerializeField]
    Color defaultColor = Color.white; // 기본 버튼 색상

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
        UpdateButtonColors(); // 색상 업데이트 호출
    }
    void GachaPage()
    {
        if (currentPage == 1|| isChangePage) return;
        isChangePage = true;
        bookAnim.animator.SetBool("Open", false);
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
        if (currentPage == 2|| isChangePage) return;
        isChangePage = true;
        bookAnim.animator.SetBool("Open", true);
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
        if (isChangePage) return;
        if (currentPage == 1)
        {
            bookAnim.animator.SetBool("Open", true);
        }
        if (currentPage == 3)
        {
            bookAnim.animator.SetBool("Open", false);
        }
        isChangePage = true;
        bookController.NextPage();
        currentPage = Mathf.Min(++currentPage, pages.Length - 1);
        StartCoroutine(UpdatePageDelayed());
    }

    void PreviousPage()
    {
        if (isChangePage) return;
        isChangePage = true;
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
        isChangePage = false;

        UpdateButtonColors(); // 페이지 업데이트 후 색상 변경 호출
    }

    void UpdateButtonColors()
    {
        // 버튼 색상 설정
        BookOpenButton.GetComponent<Image>().color = (currentPage == 2) ? highlightColor : defaultColor;
        GaChaOpenButton.GetComponent<Image>().color = (currentPage == 1) ? highlightColor : defaultColor;
    }
}
