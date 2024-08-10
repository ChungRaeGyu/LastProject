using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager instance;
    // Start is called before the first frame update
    public List<GameObject> pages;
    public GameObject num;
    public DeckControl deckControl;
    public GameObject deckContent;
    [Header("Canvas")]
    public GameObject deckCanvas;
    public GameObject BookCanvas;
    [Header("InputScript")]
    public GameObject currentCanvas;
    public event Action OnCount;

    public bool isDrawing = false;

    [Header("UI")]
    public TMP_Text currentCrystal;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        Init();

        currentCrystal.text = DataManager.Instance.currentCrystal.ToString();
    }

    // 도감에 카드를 생성하는 메서드
    public void Init()
    {
        int j = 0;
        for (int i = 0; i < DataManager.Instance.cardObjs.Count; i++)
        {
            if (i != 0 && i % 9 == 0) j++;
            GameObject temp = Instantiate(DataManager.Instance.cardObjs[i].gameObject, pages[j].transform);
            temp.GetComponent<RectTransform>().localScale = new Vector3(1.7f, 2.55f, 1);
            CardBasic tempCardBasic = temp.GetComponent<CardBasic>();
            tempCardBasic.cardBasic = DataManager.Instance.cardObjs[i];
            Instantiate(num, temp.transform);
        }

        DataManager.Instance.deckList.Clear();
    }

    public void InvokeCount()
    {
        OnCount?.Invoke();
    }

    public void ReplaceCard(CardBasic card)
    {
        // 카드가 위치한 페이지를 찾기 위한 인덱스
        int pageIndex = -1;
        int cardIndex = -1;

        // 페이지와 카드 인덱스를 찾기 위한 루프
        for (int i = 0; i < pages.Count; i++)
        {
            Transform pageTransform = pages[i].transform;
            for (int j = 0; j < pageTransform.childCount; j++)
            {
                CardBasic existingCard = pageTransform.GetChild(j).GetComponent<CardBasic>();
                if (existingCard.cardName == card.cardName)
                {
                    pageIndex = i;
                    cardIndex = j;
                    break;
                }
            }
            if (pageIndex != -1) break; // 카드 위치를 찾으면 루프 종료
        }

        // 지정한 카드를 찾지 못한 경우
        if (pageIndex == -1)
        {
            Debug.LogError("지정한 카드를 찾을 수 없습니다.");
            return;
        }

        // 기존 카드를 삭제
        Destroy(pages[pageIndex].transform.GetChild(cardIndex).gameObject);

        // 같은 위치에 카드 재생성
        GameObject newCard = Instantiate(card.cardBasic.gameObject, pages[pageIndex].transform);
        newCard.transform.SetSiblingIndex(cardIndex);
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1.7f, 2.55f, 1);
        CardBasic newCardBasic = newCard.GetComponent<CardBasic>();
        newCardBasic.cardBasic = card.cardBasic;

        Instantiate(num, newCard.transform);
    }

}
