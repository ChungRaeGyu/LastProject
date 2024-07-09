using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawSystem : MonoBehaviour
{
    //DrawSystem에 달려있습니다.
    List<CardBasic> normalCards = new List<CardBasic>(); //보통;
    List<CardBasic> rarityCards = new List<CardBasic>(); //희귀;
    List<CardBasic> heroCards = new List<CardBasic>();//영웅카드;

    Queue<CardBasic> tempCardBasic = new Queue<CardBasic>();
    public Button drawButton;
    //등급, 몇번째인지
    [SerializeField] GameObject board;
    [SerializeField] int count;
    //나중에 switch를 없앨 방법을 생각해 보자
    private void Start(){
        foreach(CardBasic card in DataManager.Instance.cardObjs){
            switch(card.rate){
                case Rate.Normal: 
                    normalCards.Add(card);
                    break;
                case Rate.Rarity:
                    rarityCards.Add(card);
                    break;
                case Rate.Hero:
                    heroCards.Add(card);
                    break;
            }
        }
    }
    public void DrawingCardBtn(){
        drawButton.enabled=false;
        for(int i=0; i< count; i++) { 
            int random = Random.Range(1,100);
            if(random<80)
            {
                //노말카드
                int randomCard = Random.Range(0, normalCards.Count);
                GameObject tempObj = Instantiate(normalCards[randomCard].gameObject, board.transform);
                tempObj.GetComponent<SpriteRenderer>().sprite = normalCards[randomCard].defaultImage;
                tempCardBasic.Enqueue(normalCards[randomCard]);
            }
            else if(random<95){
                //희귀카드뽑기
                int randomCard = Random.Range(0, rarityCards.Count);
                Instantiate(rarityCards[randomCard].gameObject);
                tempCardBasic.Enqueue(rarityCards[randomCard]);
            }
            else{
                //영웅카드뽑기
                int randomCard = Random.Range(0, heroCards.Count);
                Instantiate(heroCards[randomCard].gameObject);
                tempCardBasic.Enqueue(heroCards[randomCard]);
            }
        }
    }
    //Book(도감)으로 넣어준다. 그리고 카드를 다 초기화 시켜주기
    private void SaveCardInBook()
    {            
        //초기화
        foreach(CardBasic cardBasic in tempCardBasic)
        {
            cardBasic.currentCount++;
            Destroy(cardBasic.gameObject);
        }
        tempCardBasic.Clear();
    }

    //패널 닫기 
    public void CloseCanvas(){
        drawButton.enabled=true;
        SaveCardInBook();
    }

    public void OpenCard(){
        if(tempCardBasic.Count==0)return;
        foreach(CardBasic cardBasic in tempCardBasic)
        {
            cardBasic.gameObject.GetComponent<SpriteRenderer>().sprite = cardBasic.image;
        }
    }

}
