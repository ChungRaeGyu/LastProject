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
    public List<CardSO> deckList = new List<CardSO>();
    public Stack<CardSO> deck = new Stack<CardSO>();

    [Header("Used Cards")]
    public List<CardSO> usedCards = new List<CardSO>(); // 사용된 카드 리스트

    [Header("CardSOs")]
    public List<CardSO> cardSOs;

    [Header("CardPiece")]
    public int[] CardPiece = new int[(int)Rate.Count];

    public void SuffleAction()
    {
        Suffle(deckList);
    }

    private void Suffle(List<CardSO> deckList)
    {
        //게임 시작시 셔플하게 하기
        List<CardSO> temp = deckList.OrderBy(_ => Random.Range(0, deckList.Count)).ToList();
        foreach (CardSO tempCard in temp)
        {
            deck.Push(tempCard);
        }
    }

    public CardSO PopCard()
    {
        if (deck.Count == 0)
        {
            ReshuffleUsedCards();
        }
        Debug.Log("카드배출");
        return deck.Pop();
    }

    public void AddUsedCard(CardSO usedCard)
    {
        usedCards.Add(usedCard);
    }

    private void ReshuffleUsedCards()
    {
        if (usedCards.Count == 0)
        {
            Debug.Log("다시 섞을 카드가 없습니다!");
            return;
        }

        foreach (CardSO card in usedCards)
        {
            deck.Push(card);
        }
        usedCards.Clear();
        Suffle(deck.ToList());
        Debug.Log("사용된 카드가 덱에 다시 섞였습니다.");
    }

    public void DeckClear()
    {
        //스테이지 종료시 사용
        deck.Clear();
        usedCards.Clear();
    }

    public void AddCard(List<CardSO> newCards)
    {
        //보상패널에서 획득한 카드 덱에 넣기
        foreach (CardSO cardSO in newCards)
        {
            deckList.Add(cardSO);
        }
    }

    public void DeckCheck()
    {
        foreach (CardSO card in deck)
        {
            Debug.Log("Card : " + card.name);
            Debug.Log("Card Image : " + card.Image);
        }
    }
}
