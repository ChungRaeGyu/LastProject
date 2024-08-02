using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawSystem : MonoBehaviour
{
    //DrawSystem�� �޷��ֽ��ϴ�.
    List<CardBasic> normalCards = new List<CardBasic>(); //����;
    List<CardBasic> rarityCards = new List<CardBasic>(); //���;
    List<CardBasic> heroCards = new List<CardBasic>();//����ī��;
    List<CardBasic> legendCards = new List<CardBasic>();//����ī��


    Queue<CardBasic> tempCardBasic = new Queue<CardBasic>();
    List<GameObject> tempCardObj = new List<GameObject>();

    public Button drawButton;
    //���, ���°����
    [SerializeField] GameObject board;
    [SerializeField] int count;

    RectTransform boardtransform;
    Vector3 initTransform;
    //���߿� switch�� ���� ����� ������ ����
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

    // ī�� �̱�� �޸����� ���̰� �������ִ� �޼���
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

            // �ؽ�Ʈ���� �Ⱥ��̰� �Ѵ�
            tempObj.GetComponent<CardData>().SetTextVisibility(false);

            tempCardBasic.Enqueue(cardList[randomCard]);
            tempCardObj.Add(tempObj);
        }
    }

    //Book(����)���� �־��ش�. �׸��� ī�带 �� �ʱ�ȭ �����ֱ�
    private void SaveCardInBook()
    {
        //�ʱ�ȭ
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

    //�г� �ݱ� 
    public void CloseCanvas()
    {
        SaveCardInBook();
        Debug.Log("���� : " + initTransform);
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
