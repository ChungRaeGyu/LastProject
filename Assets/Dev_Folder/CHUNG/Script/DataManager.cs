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
    public int[] CardPiece = new int[(int)Rate.Count];

    public void SuffleAction()
    {
        Suffle(deckList);
    }

    private void Suffle(List<CardBasic> deckList)
    {
        //���� ���۽� �����ϰ� �ϱ�
        List<CardBasic> temp = deckList.OrderBy(_ => Random.Range(0, deckList.Count)).ToList();
        foreach (CardBasic tempCard in temp)
        {
            deck.Push(tempCard);
        }
    }

    public CardBasic PopCard()
    {
        if (deck.Count == 0)
        {
            ReshuffleUsedCards();
        }
        Debug.Log("ī�����");
        return deck.Pop();
    }

    public void AddUsedCard(CardBasic usedCard)
    {
        usedCards.Add(usedCard);
    }

    private void ReshuffleUsedCards()
    {
        if (usedCards.Count == 0)
        {
            Debug.Log("�ٽ� ���� ī�尡 �����ϴ�!");
            return;
        }

        foreach (CardBasic card in usedCards)
        {
            deck.Push(card);
        }
        usedCards.Clear();
        Suffle(deck.ToList());
        Debug.Log("���� ī�尡 ���� �ٽ� �������ϴ�.");
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
