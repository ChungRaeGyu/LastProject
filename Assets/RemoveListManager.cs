using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RemoveListManager : MonoBehaviour
{
    public RectTransform deckContent; // ī�� ����Ʈ�� ��ũ�� �� Content
    public ContentHeightAdjuster CardsContentHeightAdjuster; // ���� ī�� ����Ʈ�� ContentHeightAdjuster
    public Button sortDeckButton; // ī�� ����Ʈ ���� ��ư
    public Image CostSortImage; // CostSort ȭ��ǥ �̹���

    private bool isDeckAscending = true; // ī�� ����Ʈ �������� ���� ����
    private Color defaultButtonColor; // �⺻ ��ư ��
    private Color selectedButtonColor = Color.yellow; // ���õ� ��ư ��

    private void Start()
    {
        // �⺻ ��ư �� ����
        defaultButtonColor = sortDeckButton.GetComponent<Image>().color;
    }

    // ī�� �����Ϳ� ���� ī�� ���� �� �߰�
    private GameObject CreateCard(CardBasic cardData)
    {
        // ī�� �������� ����Ͽ� ī�� ����
        GameObject newCard = Instantiate(cardData.gameObject, Vector3.zero, Quaternion.identity);
        newCard.GetComponent<CardBasic>().cardBasic = cardData;

        ProcessCardObject(newCard);

        // ��ư �߰�
        Button cardButton = newCard.AddComponent<Button>();
        cardButton.onClick.AddListener(() => OnCardButtonClicked(cardData));

        return newCard;
    }

    // ī�� ������Ʈ�� �ڽ� ó��
    private void ProcessCardObject(GameObject cardObject)
    {
        RectTransform textRectTransform = cardObject.transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
        if (textRectTransform != null)
        {
            // PosX�� 3.25�� ����, PosY�� 4�� ���� (�ڽ�Ʈ�� �ؽ�Ʈ ��ġ�� �� �¾Ƽ� �ӽ÷� ����)
            Vector2 newPosition = textRectTransform.anchoredPosition;
            newPosition.x -= 3.25f;
            newPosition.y += 4f;
            textRectTransform.anchoredPosition = newPosition;
        }

        // ī�� ������Ʈ�� �ڽĿ��� ī�� �̹��� ����
        Destroy(cardObject.transform.GetChild(0).gameObject);
    }

    // ī�� ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    private void OnCardButtonClicked(CardBasic cardData)
    {
        // DataManager.Instance.deckList���� �ش� ī�� ������ ���� ī�� �� �ϳ��� ����
        DataManager.Instance.deckList.Remove(cardData);

        // �г� ��Ȱ��ȭ
        StoreManager.Instance.removePanel.SetActive(false);
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

    // ī�� ����Ʈ ����
    public void UpdateDeckList()
    {
        // Stack�� List�� ��ȯ
        List<CardBasic> deckList = new List<CardBasic>(DataManager.Instance.deckList);
        RefreshCardList(deckContent, deckList);
        CardsContentHeightAdjuster.cardCount = DataManager.Instance.deckList.Count;
        CardsContentHeightAdjuster.AdjustContentHeight();
    }

    // ���� ���� ���� �� �� ����
    public void ToggleSortOrderAndSortDeck()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        isDeckAscending = !isDeckAscending;
        SortDeckByCost();
    }

    // �ڽ�Ʈ �������� ����
    private void SortDeckByCost()
    {
        SortCardsByCost(deckContent, isDeckAscending);
        // ȭ��ǥ �̹��� ȸ��
        CostSortImage.transform.rotation = isDeckAscending ? Quaternion.Euler(180, 0, 0) : Quaternion.Euler(0, 0, 0);
        CardsContentHeightAdjuster.AdjustContentHeight();
    }

    // �־��� �������� �ڽ�Ʈ �������� ����
    private void SortCardsByCost(RectTransform content, bool isAscending)
    {
        List<Transform> cards = new List<Transform>();

        // ���� content ���� �ڽ� ī����� ����Ʈ�� �߰�
        foreach (Transform card in content)
        {
            cards.Add(card);
        }

        // ī�� ������Ʈ���� cost �������� ����
        if (isAscending)
        {
            cards.Sort((x, y) => x.GetComponent<CardBasic>().cost.CompareTo(y.GetComponent<CardBasic>().cost)); // �������� ����
        }
        else
        {
            cards.Sort((x, y) => y.GetComponent<CardBasic>().cost.CompareTo(x.GetComponent<CardBasic>().cost)); // �������� ����
        }

        // ���ĵ� ������� hierarchy���� ���ġ
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetSiblingIndex(i);
        }
    }
}

