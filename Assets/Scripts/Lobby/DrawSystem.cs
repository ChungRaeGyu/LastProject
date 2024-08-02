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
    List<CardBasic> legendCards = new List<CardBasic>();//전설카드


    Queue<CardBasic> tempCardBasic = new Queue<CardBasic>();
    List<GameObject> tempCardObj = new List<GameObject>();

    public Button drawButton;
    //등급, 몇번째인지
    [SerializeField] GameObject board;
    [SerializeField] int count;

    RectTransform boardtransform;
    Vector3 initTransform;
    //나중에 switch를 없앨 방법을 생각해 보자
    private void Start()
    {
        //boardtransform = board.GetComponent<RectTransform>();
        //initTransform = board.GetComponent<RectTransform>().localPosition;
        foreach (CardBasic card in DataManager.Instance.cardObjs)
        {
            switch (card.rate)
            {
                case Rate.Normal:
                    normalCards.Add(card);
                    break;
                case Rate.Rarity:
                    rarityCards.Add(card);
                    break;
                case Rate.Hero:
                    heroCards.Add(card);
                    break;
                case Rate.Legend:
                    legendCards.Add(card);
                    break;
            }
        }
    }

    // 카드 뽑기시 뒷면으로 보이게 생성해주는 메서드
    public void DrawingCardBtn()
    {
        LobbyManager.instance.isDrawing = true;
        for (int i = 0; i < count; i++)
        {
            int random = Random.Range(1, 100);
            List<CardBasic> cardList;
            if (random < 80)
            {
                cardList = normalCards;
            }
            else if (random < 95)
            {
                cardList = rarityCards;
            }
            else if(random<99)
            {
                cardList = heroCards;
            }
            else
            {
                cardList = legendCards;
            }

            int randomCard = Random.Range(0, cardList.Count);
            GameObject tempObj = Instantiate(cardList[randomCard].gameObject, board.transform);
            Image[] tempObjImage = tempObj.GetComponentsInChildren<Image>();
            tempObjImage[1].sprite = DataManager.Instance.cardBackImage;

            // 텍스트들이 안보이게 한다
            tempObj.GetComponent<CardData>().SetTextVisibility(false);

            tempCardBasic.Enqueue(cardList[randomCard]);
            tempCardObj.Add(tempObj);
        }
    }

    //Book(도감)으로 넣어준다. 그리고 카드를 다 초기화 시켜주기
    private void SaveCardInBook()
    {
        //초기화
        foreach (GameObject cardObj in tempCardObj)
        {
            CardBasic temp = tempCardBasic.Dequeue();
            temp.currentCount++;
            temp.cardBasic.isFind = true;
            Destroy(cardObj);
        }
        LobbyManager.instance.InvokeCount();
        tempCardBasic.Clear();
        tempCardObj.Clear();
        LobbyManager.instance.isDrawing = false;
    }

    //패널 닫기 
    public void CloseCanvas()
    {
        SaveCardInBook();
        Debug.Log("종료 : " + initTransform);
        //boardtransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,0,0);
    }

    public void OpenCard()
    {
        if (tempCardBasic.Count == 0) return;
        foreach (GameObject cardObj in tempCardObj)
        {
            cardObj.GetComponentInChildren<Image>().sprite = cardObj.GetComponent<CardBasic>().image;
        }
    }

}
