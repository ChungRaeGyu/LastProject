using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //�⺻ī��� ��� ��
                                                              //���ӽ��� ��ư�� ������ �� deckList�� �־������)
                                                              //�ִ� 6�� ���� 6�� �̸��� ���ӽ��� �Ұ���

    //[HideInInspector]
    public List<CardBasic> deckList = new List<CardBasic>(); //�������� ����� �� ����Ʈ
    public Stack<CardBasic> deck = new Stack<CardBasic>(); //���� ī�带 �̴� ��( deckList�� ����ؼ� �־��ش�.)

    [Header("Used Cards")]
    public List<CardBasic> usedCards = new List<CardBasic>(); // ���� ī�� ����Ʈ

    [Header("GameObjects")]
    public List<CardBasic> cardObjs = new List<CardBasic>();//���� �����͸� ��� ������ �ִ� ��

    [Header("CardPiece")]
    public int[] CardPiece;

    [Header("CardBack")]
    public Sprite cardBackImage;

    private void Start()
    {
        CardPiece = new int[(int)Rate.Count];
    }

    [Header("current Battle Monsters")]
    public List<GameObject> Monsters = new List<GameObject>();

    //�÷��̾��� ������ �����ϴ� ������
    public int currenthealth { get; set; }

    // ���� Ŭ���� ��ϵ�
    public int monstersKilledCount { get; set; }
    public int stageClearCount { get; set; }
    public int totalClearTime { get; set; }
    public int bossesDefeatedCount { get; set; }
    public int totalCrystal { get; set; }

    // ���� ��� ������
    public int adjustedCurrentCoin { get; set; }
    public int adjustedClearTime { get; set; }

    // ���� ��ȭ
    //public int currentCoin { get; set; }
    public int currentCoin; // �׽�Ʈ������ �ν����Ϳ��� ������ �����ϰ� �ص�
    // public int currentCrystal { get; set; } = 0; // �ϴ� 0���� (�ӽ�)
    public int currentCrystal; // �׽�Ʈ������ �ν����Ϳ��� ������ �����ϰ� �ص�

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

    // TotalCrystal�� ����ϴ� �޼���
    public void CalculateTotalCrystal()
    {
        // �ݿø�
        //int adjustedCoin = Mathf.RoundToInt(currentCoin / 100f);
        // �Ҽ��� �Ʒ��� ���� (��ȭ�� 1���� �� �����μ� ���̵� ���)
        adjustedCurrentCoin = Mathf.FloorToInt(currentCoin / 100f);
        adjustedClearTime = Mathf.Max(300 - totalClearTime, 0);

        // TotalCrystal ���
        totalCrystal = adjustedCurrentCoin
                     + monstersKilledCount
                     + stageClearCount
                     + adjustedClearTime
                     + bossesDefeatedCount;
    }

    // ��� �ʱ�ȭ
    public void ResetRecord()
    {
        currentCoin = 0;
        monstersKilledCount = 0;
        stageClearCount = 0;
        totalClearTime = 0;
        bossesDefeatedCount = 0;
        totalCrystal = 0;
    }

    // �÷��̾� ü�� �ʱ�ȭ
    public void ResetPlayerHealth()
    {
        currenthealth = 0; // �ϴ� 0���� �ʱ�ȭ (0�϶� �ִ밪���� ���� �س��� ������ player�� ������)
    }
}
