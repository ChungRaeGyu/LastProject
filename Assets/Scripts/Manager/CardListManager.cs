using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardListManager : MonoBehaviour
{
    public RectTransform deckContent; // ���� ī�� ����Ʈ�� ��ũ�� �� Content
    public RectTransform usedCardsContent; // ���� ī�� ����Ʈ�� ��ũ�� �� Content

    // ī�� �����Ϳ� ���� ī�� ���� �� �߰�
    private GameObject CreateCard(CardBasic cardData)
    {
        // ī�� �������� ����Ͽ� ī�� ����
        GameObject newCard = Instantiate(cardData.gameObject, Vector3.zero, Quaternion.identity);
        newCard.GetComponent<CardBasic>().cardBasic = cardData;

        ProcessCardObject(newCard);

        return newCard;
    }

    // ī�� ������Ʈ�� �ڽ� ó��
    private void ProcessCardObject(GameObject cardObject)
    {
        RectTransform textRectTransform = cardObject.transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
        if (textRectTransform != null)
        {
            // PosX�� 3.25�� ����, PosY�� 4�� ���� (�ڽ�Ʈ�� �ؽ�Ʈ ��ġ�� �ȸ¾Ƽ� �ӽ÷� ����)
            Vector2 newPosition = textRectTransform.anchoredPosition;
            newPosition.x -= 3.25f;
            newPosition.y += 4f;
            textRectTransform.anchoredPosition = newPosition;
        }

        // ī�� ������Ʈ�� �ڽĿ��� ī�� �̹��� ó��
        Destroy(cardObject.transform.GetChild(0).gameObject);
    }

    // ī�� ����Ʈ ���� �޼���
    private void RefreshCardList(RectTransform content, List<CardBasic> cardList)
    {
        // ���� ī�� ����
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // �����Ϳ��� ī�� �߰�
        foreach (CardBasic card in cardList)
        {
            GameObject cardObject = CreateCard(card);
            cardObject.transform.SetParent(content, false); // ��ũ�� �� Content�� ī�� �߰�
        }
    }

    // ���� ī�� ����Ʈ ����
    public void UpdateDeckList()
    {
        // Stack�� List�� ��ȯ
        List<CardBasic> deckList = new List<CardBasic>(DataManager.Instance.deck);
        RefreshCardList(deckContent, deckList);
    }

    // ���� ī�� ����Ʈ ����
    public void UpdateUsedCardsList()
    {
        // Stack�� List�� ��ȯ
        List<CardBasic> usedCardsList = new List<CardBasic>(DataManager.Instance.usedCards);
        RefreshCardList(usedCardsContent, usedCardsList);
    }
}
