using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;
public class DataManager : MonoBehaviour
{
    #region 싱글톤
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
    public List<CardBasic> usedCards = new List<CardBasic>(); // 사용된 카드 리스트

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

    //플레이어의 스탯을 저장하는 변수들
    public int currenthealth { get; set; }

    // 몬스터를 죽인 횟수를 저장하는 변수
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

        //게임 시작시 셔플하게 하기
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

            // 덱이 여전히 비어있다면 오류를 방지하기 위해 예외를 던집니다.
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
        //스테이지 종료시 사용
        deck.Clear();
        usedCards.Clear();
    }

    public void AddCard(List<CardBasic> newCards)
    {
        //보상패널에서 획득한 카드 덱에 넣기
        foreach (CardBasic GameObject in newCards)
        {
            deckList.Add(GameObject);
        }
    }

    // 몬스터를 죽일 때 호출할 메서드
    public void IncreaseMonstersKilledCount()
    {
        monstersKilledCount++;
    }

    // 몬스터 킬 수 초기화
    public void ResetMonstersKilledCount()
    {
        monstersKilledCount = 0;
    }

    // 플레이어 체력 초기화
    public void ResetPlayerHealth()
    {
        currenthealth = 0; // 일단 0으로 초기화 (0일때 최대값으로 들어가게 해놓은 로직이 player에 존재함)
    }
}
