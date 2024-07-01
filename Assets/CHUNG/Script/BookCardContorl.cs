using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCardControl : Card
{
    public Text text;
    public Button button;
    private void Awake() {
        button = GetComponent<Button>();
    }
    public void OpenPanel(){
        DescriptionManager.Instance.currentCard = this;
        DescriptionManager.Instance.OpenPanel();
    }
    private void OnEnable() {
        text.text = cardSO.currentCount.ToString();
        if(cardSO.currentCount <=0){
            cardSO.currentCount = 0;
            button.enabled = false;
        }else{
            button.enabled = true;
        }
    }
}
