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

    //등급, 몇번째인지
    [SerializeField] GameObject board;
    [SerializeField] int count;

    RectTransform boardtransform;
    Vector3 initTransform;
    //나중에 switch를 없앨 방법을 생각해 보자

    public CardData cardData;

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
<<<<<<< Updated upstream
            
            Image[] tempObjImage = tempObj.GetComponentsInChildren<Image>();
            tempObjImage[0].sprite = DataManager.Instance.cardBackImage;
            tempObjImage[0].raycastTarget = false;
=======
>>>>>>> Stashed changes

            cardData = tempObj.GetComponent<CardData>();

            tempCardBasic.Enqueue(cardList[randomCard]);
            tempCardObj.Add(tempObj);

            StartCoroutine(SetCardBackImageWhenReady(cardData, tempObj));
        }
    }

    private IEnumerator SetCardBackImageWhenReady(CardData cardData, GameObject tempObj)
    {
        // Start가 완료될 때까지 대기
        yield return new WaitUntil(() => cardData.isStartCompleted);

        // Start가 완료된 후 뒷면 이미지로 설정
        Image tempObjImage = tempObj.transform.GetChild(1).GetComponent<Image>();
        tempObjImage.sprite = DataManager.Instance.cardBackImage;
        tempObjImage.raycastTarget = false;

        // 텍스트들이 안보이게 한다
        cardData.SetTextVisibility(false, tempObj.GetComponent<CardBasic>());

        tempCardBasic.Enqueue(cardData.GetComponent<CardBasic>());
        tempCardObj.Add(tempObj);
    }

    //Book(도감)으로 넣어준다. 그리고 카드를 다 초기화 시켜주기
    private void SaveCardInBook()
    {
        //초기화
        foreach (GameObject cardObj in tempCardObj)
        {
            CardBasic temp = tempCardBasic.Dequeue();
            temp.currentCount++;
            temp.isFind = true;
            Image[] tempImage = temp.GetComponentsInChildren<Image>();
            temp.GetComponent<CardData>().CardOpenControl(temp, true);
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
<<<<<<< Updated upstream
=======
        LobbyManager.instance.ResetAndReinitialize();

        //boardtransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,0,0);
>>>>>>> Stashed changes
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
