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
    public List<Card> deckList = new List<Card>();
    public Stack<CardSO> deck = new Stack<CardSO>();

    [Header("CardSOs")]
    public List<CardSO> cardSOs;

    [Header("CardPiece")]
    public int[] CardPiece = new int[(int)Rate.Count];

    public void SuffleAction(){
        Suffle(deckList);
    }
    private void Suffle(List<Card> deckList)
    {
        //게임 시작시 셔플하게 하기
        List<Card> temp = deckList.OrderBy(_ => Random.Range(0, deckList.Count)).ToList();
        foreach (Card tempCard in temp)
        {
            deck.Push(tempCard.GetComponent<Card>().cardSO);
        }
    }

    public CardSO PopCard()
    {
        Debug.Log("카드배출");
        return deck.Pop();
    }

    public void DeckClear(){
        //스테이지 종료시 사용
        deck.Clear();
    }

    public void AddCard(List<Card> newCards){
        //보상패널에서 획득한 카드 덱에 넣기
        foreach(Card card in newCards){
            deckList.Add(card);
        }
    }

    public void DeckCheck(){
        foreach(CardSO card in deck){
            Debug.Log("Card : " + card.name);
            Debug.Log("Card Image : " + card.Image);
        }
    }
}
