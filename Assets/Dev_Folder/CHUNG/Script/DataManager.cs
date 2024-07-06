using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    public int[] CardPiece = new int[(int)Rate.Count];

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
        List<CardBasic> temp = deckList.OrderBy(_ => Random.Range(0, deckList.Count)).ToList();
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
            Debug.Log("덱이 0이다.");

            SuffleUsedCards();
        }
        Debug.Log("카드배출");
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
}
