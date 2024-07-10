using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DeckControl : MonoBehaviour
{
    //위치 : DeckManager->Canvas->DeckPanel
    //될 수 있으면 GetComponent와 Find를 쓰지 않아야함
    
    List<CardBasic> cardObj = new List<CardBasic>(); //Queue를 받아서 임시 저장해 놓는 곳이다.
    [SerializeField]GameObject prefab;

    #region 로비
    //Book to Deck
    public void AddCard(CardBasic cardObj){
        //설명창에서 덱추가 버튼을 눌렀을 때
        //드래그해서 덱에 닿였을때 호출
        DataManager.Instance.deckList.Add(cardObj);
        AddObj();
        Debug.Log("카드추가");
    }
    public void RemoveCard(){
        if(DataManager.Instance.deckList.Count==0)return;
        int endCard = DataManager.Instance.deckList.Count-1;
        DataManager.Instance.deckList[endCard].GetComponent<Card>().cardObj.currentCount++;
        DataManager.Instance.deckList.RemoveAt(endCard);
        DescriptionManager.Instance.bookCardControl.UpdateBook();
        //UpdateDeck();
    }

    private void OnEnable()
    {
        SetDeck();
    }

    //DeckVisualization
    private void SetDeck(){
        //덱 전부다 다시 생성 
        for (int i = 0; i < DataManager.Instance.deckList.Count; i++)
        {
            GameObject obj = Instantiate(prefab, this.transform);
            CardBasic tempCard = obj.GetComponent<CardBasic>();
            cardObj.Add(tempCard);
            tempCard.cardObj = DataManager.Instance.deckList[i];
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }

    private void AddObj()
    {
       GameObject obj = Instantiate(prefab, this.transform);
        CardBasic tempCard = obj.GetComponent<CardBasic>();
        obj.transform.SetParent(transform);
        obj.SetActive(true); 
    }
    /*
    private void ClearDeck(){
        Debug.Log("Deck ClearDeck()");
        foreach (Card tempcard in cardObj)
        {
            ObjectPool.cardsObj.Enqueue(tempcard.gameObject);
            tempcard.gameObject.SetActive(false);
        }
        cardObj.Clear();
    }
    */
    #endregion
}
