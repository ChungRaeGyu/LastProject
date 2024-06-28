using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Stack<Card> deck = new Stack<Card>();
    public void AddCard(Card card){
        deck.Push(card);
        Debug.Log("카드추가");
    }

    public Card PopCard(){
        Debug.Log("카드배출");
        return deck.Pop();
        
    }


}
