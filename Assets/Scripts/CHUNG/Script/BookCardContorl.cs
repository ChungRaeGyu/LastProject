using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCardControl : MonoBehaviour
{
    public Text text;
    public Button button;
    public Card childCard;
    private void Awake() {

        button = GetComponent<Button>();
        childCard = this.GetComponentInChildren<Card>();
        
    }

    //버튼을 눌렀을 때 그 카드의 값을 받아서 가야해
    public void OpenPanel(){
        //DescriptionManager.Instance.currentCard = childCard;
        
    }
}
