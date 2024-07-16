using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;
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

    [Header("current Battle Monsters")]
    public List<GameObject> Monsters = new List<GameObject>();

    //�÷��̾��� ������ �����ϴ� ������
    public int currenthealth { get; set; }

    // ���͸� ���� Ƚ���� �����ϴ� ����
    public int monstersKilledCount { get; private set; }

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
        }

        usedCards.Clear();
    }

    public CardBasic PopCard()
    {
        if (deck.Count == 0)
        {
            SuffleUsedCards();

            // ���� ������ ����ִٸ� ������ �����ϱ� ���� ���ܸ� �����ϴ�.
            if (deck.Count == 0)
            {
            }
        }
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

    // ���͸� ���� �� ȣ���� �޼���
    public void IncreaseMonstersKilledCount()
    {
        monstersKilledCount++;
    }

    // ���� ų �� �ʱ�ȭ
    public void ResetMonstersKilledCount()
    {
        monstersKilledCount = 0;
    }

    // �÷��̾� ü�� �ʱ�ȭ
    public void ResetPlayerHealth()
    {
        currenthealth = 0; // �ϴ� 0���� �ʱ�ȭ (0�϶� �ִ밪���� ���� �س��� ������ player�� ������)
    }
}
