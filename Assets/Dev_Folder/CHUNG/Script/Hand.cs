using System.Collections.Generic;
using UnityEngine;


public class Hand : MonoBehaviour
{
    public List<GameObject> CardObject;
    public List<CardSO> hand = new List<CardSO>();

    private void Start(){
        Debug.Log("Start()");
        for(int i=0; i<4;i++){
            Addhand(DataManager.Instance.deck.Pop());
        }
    }
    public void Addhand(CardSO cardso){
        Debug.Log("Addhand");
        hand.Add(cardso);
        UpdateHand();
    }

    //테스트 필요
    public void UseCard(GameObject cardObj){
        hand.Remove(cardObj.GetComponent<Card>().cardSO);
        cardObj.SetActive(false);
        UpdateHand();
    }
    public void UpdateHand(){
        Debug.Log("UpdateHand" + hand.Count);
        for(int i=0; i<hand.Count;i++){
            CardObject[i].SetActive(true);
            CardObject[i].GetComponent<Card>().cardSO = hand[i];
            CardObject[i].GetComponent<Card>().ImageSet();
        }
    }
}