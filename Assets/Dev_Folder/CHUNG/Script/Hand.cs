using System.Collections.Generic;
using UnityEngine;


public class Hand : MonoBehaviour
{
    public List<GameObject> CardObject;
    public List<Card> hand = new List<Card>();

    public void Addhand(Card card){
        Debug.Log("Addhand");
        hand.Add(card);
        UpdateHand();
    }

    //테스트 필요
    public void UseCard(GameObject cardObj){
        hand.Remove(cardObj.GetComponent<Card>());
        cardObj.SetActive(false);
        UpdateHand();
    }
    public void UpdateHand(){
        Debug.Log("UpdateHand" + hand.Count);
        for(int i=0; i<hand.Count;i++){
            CardObject[i].SetActive(true);
            CardObject[i].GetComponent<Card>().cardSO = hand[i].cardSO;
        }
    }
}