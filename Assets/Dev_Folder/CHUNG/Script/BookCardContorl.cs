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

    //��ư�� ������ �� �� ī���� ���� �޾Ƽ� ������
    public void OpenPanel(){
        //DescriptionManager.Instance.currentCard = childCard;
        
    }
}
