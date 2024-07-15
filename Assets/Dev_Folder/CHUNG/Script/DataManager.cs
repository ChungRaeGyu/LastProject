using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class DataManager : MonoBehaviour
{
    #region �̱���
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DataManager").AddComponent<DataManager>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
                Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion
    [Header("Deck")]
    public List<CardBasic> deckList = new List<CardBasic>();
    public Stack<CardBasic> deck = new Stack<CardBasic>();

    [Header("Used Cards")]
    public List<CardBasic> usedCards = new List<CardBasic>(); // ���� ī�� ����Ʈ

    [Header("GameObjects")]
    public List<CardBasic> cardObjs = new List<CardBasic>();



    [Header("CardPiece")]
    public int[] CardPiece; 
    private void Start()
    {
        CardPiece= new int[(int)Rate.Count];
    }
    public void SuffleDeckList()
    {

        Suffle(deckList);
    }
    public void SuffleUsedCards()
    {
        Suffle(usedCards);
    }
    private void Suffle(List<CardBasic> deckList)
    {
        deck.Clear();

        //���� ���۽� �����ϰ� �ϱ�
        List<CardBasic> temp = deckList.OrderBy(_ => UnityEngine.Random.Range(0, deckList.Count)).ToList();
        foreach (CardBasic tempCard in temp)
        {
            deck.Push(tempCard);
            Debug.Log($"Card pushed: {tempCard.name}");
        }

        usedCards.Clear();
    }

    public CardBasic PopCard()
    {
        if (deck.Count == 0)
        {
            Debug.Log("���� ����ֽ��ϴ�. ���� ī�带 �����մϴ�.");
            SuffleUsedCards();

            // ���� ������ ����ִٸ� ������ �����ϱ� ���� ���ܸ� �����ϴ�.
            if (deck.Count == 0)
            {
                throw new InvalidOperationException("���� ����ֽ��ϴ�. ���� ī�带 ������ �Ŀ��� ���� ����ֽ��ϴ�.");
            }
        }
        Debug.Log("ī�����");
        return deck.Pop();
    }

    public void AddUsedCard(CardBasic usedCard)
    {
        usedCards.Add(usedCard);
    }

    public void DeckClear()
    {
        //�������� ����� ���
        deck.Clear();
        usedCards.Clear();
    }

    public void AddCard(List<CardBasic> newCards)
    {
        //�����гο��� ȹ���� ī�� ���� �ֱ�
        foreach (CardBasic GameObject in newCards)
        {
            deckList.Add(GameObject);
        }
    }
}
