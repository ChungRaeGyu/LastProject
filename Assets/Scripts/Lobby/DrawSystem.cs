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

    //���, ���°����
    [SerializeField] GameObject board;
    [SerializeField] int count;

    RectTransform boardtransform;
    Vector3 initTransform;
    //���߿� switch�� ���� ����� ������ ����

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
        // Start�� �Ϸ�� ������ ���
        yield return new WaitUntil(() => cardData.isStartCompleted);

        // Start�� �Ϸ�� �� �޸� �̹����� ����
        Image tempObjImage = tempObj.transform.GetChild(1).GetComponent<Image>();
        tempObjImage.sprite = DataManager.Instance.cardBackImage;
        tempObjImage.raycastTarget = false;

        // �ؽ�Ʈ���� �Ⱥ��̰� �Ѵ�
        cardData.SetTextVisibility(false, tempObj.GetComponent<CardBasic>());

        tempCardBasic.Enqueue(cardData.GetComponent<CardBasic>());
        tempCardObj.Add(tempObj);
    }

    //Book(����)���� �־��ش�. �׸��� ī�带 �� �ʱ�ȭ �����ֱ�
    private void SaveCardInBook()
    {
        //�ʱ�ȭ
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

    //�г� �ݱ� 
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
