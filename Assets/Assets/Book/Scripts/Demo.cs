﻿using System;
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
    Button CreditOpenButton;
    [SerializeField]
    Image bookImage;
    [SerializeField]
    Sprite bookTexture;
    [SerializeField]
    Sprite notepadTexture;
    
    // 새로 추가된 변수들
    [SerializeField] private AudioClip pageTurnClip; // 책 넘기는 소리
    [SerializeField] private AudioSource audioSource;

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
        CreditOpenButton.onClick.AddListener(CreditPage);
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
    void CreditPage()
    {
        if (currentPage == 5 || isChangePage) return;
        bookAnim.animator.SetBool("Open", false);
        isChangePage = true;
        audioSource.PlayOneShot(pageTurnClip); // 페이지 넘기는 소리 재생
        if (currentPage > 5)
        {
            bookController.PreviousPage();
        }
        else
        {
            bookController.NextPage();
        }
        currentPage = 5;
        StartCoroutine(UpdatePageDelayed());
    }
    void GachaPage()
    {
        if (currentPage == 0|| isChangePage) return;
        isChangePage = true;
        bookAnim.animator.SetBool("Open", false);
        audioSource.PlayOneShot(pageTurnClip); // 페이지 넘기는 소리 재생
        if (currentPage > 0)
        {
            bookController.PreviousPage();
        }
        else
        {
            bookController.NextPage();
        }
        currentPage = 0;
        StartCoroutine(UpdatePageDelayed());
    }

    void BookPage()
    {
        if (currentPage == 1|| isChangePage) return;
        isChangePage = true;
        bookAnim.animator.SetBool("Open", true);
        audioSource.PlayOneShot(pageTurnClip); // 페이지 넘기는 소리 재생
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

    void NextPage()
    {
        if (isChangePage) return;
        if (currentPage == 0)
        {
            bookAnim.animator.SetBool("Open", true);
        }
        if (currentPage == 3)
        {
            bookAnim.animator.SetBool("Open", false);
        }
        isChangePage = true;
        audioSource.PlayOneShot(pageTurnClip); // 페이지 넘기는 소리 재생
        bookController.NextPage();
        currentPage = Mathf.Min(++currentPage, pages.Length - 1);
        StartCoroutine(UpdatePageDelayed());
    }

    void PreviousPage()
    {
        if (isChangePage) return;
        isChangePage = true;
        if (currentPage == 1)
        {
            bookAnim.animator.SetBool("Open", false);
        }
        if (currentPage == 3)
        {
            bookAnim.animator.SetBool("Open", true);
        }
        audioSource.PlayOneShot(pageTurnClip); // 페이지 넘기는 소리 재생
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
        GaChaOpenButton.GetComponent<Image>().color = (currentPage == 0) ? highlightColor : defaultColor;
        BookOpenButton.GetComponent<Image>().color = (currentPage >= 1 && currentPage <= 4) ? highlightColor : defaultColor;
        CreditOpenButton.GetComponent<Image>().color = (currentPage == 5) ? highlightColor : defaultColor;
    }
}
