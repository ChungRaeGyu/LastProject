using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCardControl : Card
{
    public Text text;

    public void OpenPanel(){
        DescriptionManager.Instance.currentCard = this;
        DescriptionManager.Instance.OpenPanel();
    }
    private void OnEnable() {
        text.text = cardSO.currentCount.ToString();
    }
}
