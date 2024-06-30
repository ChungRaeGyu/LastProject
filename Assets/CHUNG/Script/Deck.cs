using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Deck : MonoBehaviour
{
    //위치 : DeckManager->Canvas->DeckPanel
    //될 수 있으면 GetComponent와 Find를 쓰지 않아야함
    public Stack<Card> deck = new Stack<Card>(); //게임안에서 카드를 뽑을때만 사용할꺼임 
    public List<Card> deckList = new List<Card>(); //카드의 추가, 실물화, 섞기에서 사용
    
    List<Card> cardObj = new List<Card>(); //Queue를 받아서 임시 저장해 놓는 곳이다.
    #region 로비
    public void AddCard(Card card){
        //TODO : 여러장씩 받아서 넣을 수 있게 만들기
            deckList.Add(card);

        Debug.Log("카드추가");
    }

    private void OnEnable(){
        for(int i=0 ; i< deckList.Count;i++){
            GameObject obj = ObjectPool.bookCardObj.Dequeue();
            Card tempCard = obj.GetComponent<Card>();
            Debug.Log("에셋타입 : "+ PrefabUtility.GetPrefabAssetType(obj));
            cardObj.Add(tempCard);
            tempCard.cardSO = deckList[i].cardSO;
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }

    private void OnDisable(){
        foreach (Card tempCard in deckList)
        {
            tempCard.gameObject.SetActive(false);
            
        }
        foreach(Card tempcard in cardObj){
            ObjectPool.bookCardObj.Enqueue(tempcard.gameObject);
        }
        cardObj.Clear();
    }

    private void Suffle(){
        //게임 시작시 셔플하게 하기
        List<Card> temp  = deckList.OrderBy(_ => Random.Range(0, deckList.Count)).ToList();
        foreach(Card tempCard in temp){
            deck.Push(tempCard);
        }
    }
    #endregion
    #region 인게임
    //CardManager에 Draw()메소드 내부에 있음
    public Card PopCard()
    {
        Debug.Log("카드배출");
        return deck.Pop();
    }
    #endregion
}
