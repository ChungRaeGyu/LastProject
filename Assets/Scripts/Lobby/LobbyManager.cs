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

    // ������ ī�带 �����ϴ� �޼���
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
        // ī�尡 ��ġ�� �������� ã�� ���� �ε���
        int pageIndex = -1;
        int cardIndex = -1;

        // �������� ī�� �ε����� ã�� ���� ����
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
            if (pageIndex != -1) break; // ī�� ��ġ�� ã���� ���� ����
        }

        // ������ ī�带 ã�� ���� ���
        if (pageIndex == -1)
        {
            Debug.LogError("������ ī�带 ã�� �� �����ϴ�.");
            return;
        }

        // ���� ī�带 ����
        Destroy(pages[pageIndex].transform.GetChild(cardIndex).gameObject);

        // ���� ��ġ�� ī�� �����
        GameObject newCard = Instantiate(card.cardBasic.gameObject, pages[pageIndex].transform);
        newCard.transform.SetSiblingIndex(cardIndex);
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1.7f, 2.55f, 1);
        CardBasic newCardBasic = newCard.GetComponent<CardBasic>();
        newCardBasic.cardBasic = card.cardBasic;

        Instantiate(num, newCard.transform);
    }

}
