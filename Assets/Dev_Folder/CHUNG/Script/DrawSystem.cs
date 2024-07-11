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
    List<GameObject> tempCardObj = new List<GameObject> ();

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
            Debug.Log($"{i}번째 {random}");
            if(random<80)
            {
                //노말카드
                int randomCard = Random.Range(0, normalCards.Count);
                GameObject tempObj = Instantiate(normalCards[randomCard].gameObject, board.transform);
                tempObj.GetComponentInChildren<Image>().sprite = normalCards[randomCard].defaultImage;
                tempCardBasic.Enqueue(normalCards[randomCard]);
                tempCardObj.Add(tempObj);

            }
            else if(random<95){
                //희귀카드뽑기
                int randomCard = Random.Range(0, rarityCards.Count);
                GameObject tempObj = Instantiate(rarityCards[randomCard].gameObject, board.transform);
                tempObj.GetComponentInChildren<Image>().sprite = rarityCards[randomCard].defaultImage;
                tempCardBasic.Enqueue(rarityCards[randomCard]);
                tempCardObj.Add(tempObj);
            }
            else{
                //영웅카드뽑기
                int randomCard = Random.Range(0, heroCards.Count);
                GameObject tempObj= Instantiate(heroCards[randomCard].gameObject, board.transform);
                tempObj.GetComponentInChildren<Image>().sprite = heroCards[randomCard].defaultImage;
                tempCardBasic.Enqueue(heroCards[randomCard]);
                tempCardObj.Add(tempObj);
            }
        }
    }
    //Book(도감)으로 넣어준다. 그리고 카드를 다 초기화 시켜주기
    private void SaveCardInBook()
    {
        Debug.Log($"Queue Count : {tempCardBasic.Count}");
        Debug.Log($"List Count : {tempCardObj.Count}");

        //초기화
        foreach (GameObject cardObj in tempCardObj)
        {
            tempCardBasic.Dequeue().currentCount++;
            Destroy(cardObj);
        }
        LobbyManager.instance.InvokeCount();
        tempCardBasic.Clear();
        tempCardObj.Clear();
    }

    //패널 닫기 
    public void CloseCanvas(){
        drawButton.enabled=true;
        SaveCardInBook();
    }

    public void OpenCard(){
        if(tempCardBasic.Count==0)return;
        foreach(GameObject cardObj in tempCardObj)
        {
            cardObj.GetComponentInChildren<Image>().sprite = cardObj.GetComponent<CardBasic>().image;
        }
    }

}
