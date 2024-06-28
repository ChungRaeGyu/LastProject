using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Deck deck;
    public Hand hand;

    public void Draw(){
        Debug.Log("Draw실행");
        if(deck.deck.Count==0){
            Debug.Log("덱을 다 사용함");
            return;
        }
        if(hand.hand.Count==hand.CardObject.Count){
            Debug.Log("hand가 꽉차서 다음 카드를 파괴 합니다.");
            deck.PopCard();
            return;
        }
        hand.Addhand(deck.PopCard());
    }
}
