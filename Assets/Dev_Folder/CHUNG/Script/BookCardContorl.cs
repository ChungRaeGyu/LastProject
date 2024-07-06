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
    public void OpenPanel(){
        DescriptionManager.Instance.bookCardControl = this;
        DescriptionManager.Instance.currentCard = childCard;
        DescriptionManager.Instance.OpenPanel();
    }
    private void OnEnable() {
        UpdateBook();
    }

    public void UpdateBook(){
        text.text = childCard.cardObj.currentCount.ToString();
        if (childCard.cardObj.currentCount <= 0)
        {
            childCard.cardObj.currentCount = 0;
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }
}
