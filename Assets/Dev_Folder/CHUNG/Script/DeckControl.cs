using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DeckControl : MonoBehaviour
{
    //위치 : DeckManager->Canvas->DeckPanel
    //될 수 있으면 GetComponent와 Find를 쓰지 않아야함
    
    List<Card> cardObj = new List<Card>(); //Queue를 받아서 임시 저장해 놓는 곳이다.
    

    #region 로비
    //Book to Deck
    public void AddCard(Card card){
        DataManager.Instance.deckList.Add(card);
        UpdateDeck();
        Debug.Log("카드추가");
    }
    public void RemoveCard(){
        if(DataManager.Instance.deckList.Count==0)return;
        int endCard = DataManager.Instance.deckList.Count-1;
        DataManager.Instance.deckList[endCard].GetComponent<Card>().cardSO.currentCount++;
        DataManager.Instance.deckList.RemoveAt(endCard);
        DescriptionManager.Instance.bookCardControl.UpdateBook();
        UpdateDeck();
    }

    private void UpdateDeck(){
        ClearDeck();
        SetDeck();
    }

    private void OnEnable()
    {
        SetDeck();
    }

    private void OnDisable()
    {
        ClearDeck();
    }

    //DeckVisualization
    private void SetDeck(){
        for (int i = 0; i < DataManager.Instance.deckList.Count; i++)
        {
            GameObject obj = ObjectPool.cardsObj.Dequeue();
            Card tempCard = obj.GetComponent<Card>();
            cardObj.Add(tempCard);
            tempCard.cardSO = DataManager.Instance.deckList[i].cardSO;
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }

    private void ClearDeck(){
        Debug.Log("Deck ClearDeck()");
        foreach (Card tempcard in cardObj)
        {
            ObjectPool.cardsObj.Enqueue(tempcard.gameObject);
            tempcard.gameObject.SetActive(false);
        }
        cardObj.Clear();
    }
    #endregion
    #region 인게임
    //CardManager에 Draw()메소드 내부에 있음

    #endregion
}
