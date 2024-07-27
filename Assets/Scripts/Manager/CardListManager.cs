using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardListManager : MonoBehaviour
{
    public RectTransform deckContent; // 뽑을 카드 리스트의 스크롤 뷰 Content
    public RectTransform usedCardsContent; // 버린 카드 리스트의 스크롤 뷰 Content

    // 카드 데이터에 따라 카드 생성 및 추가
    private GameObject CreateCard(CardBasic cardData)
    {
        // 카드 프리팹을 사용하여 카드 생성
        GameObject newCard = Instantiate(cardData.gameObject, Vector3.zero, Quaternion.identity);
        newCard.GetComponent<CardBasic>().cardBasic = cardData;

        ProcessCardObject(newCard);

        return newCard;
    }

    // 카드 오브젝트의 자식 처리
    private void ProcessCardObject(GameObject cardObject)
    {
        RectTransform textRectTransform = cardObject.transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
        if (textRectTransform != null)
        {
            // PosX에 3.25를 빼고, PosY에 4를 더함 (코스트의 텍스트 위치가 안맞아서 임시로 조정)
            Vector2 newPosition = textRectTransform.anchoredPosition;
            newPosition.x -= 3.25f;
            newPosition.y += 4f;
            textRectTransform.anchoredPosition = newPosition;
        }

        // 카드 오브젝트의 자식에서 카드 이미지 처리
        Destroy(cardObject.transform.GetChild(0).gameObject);
    }

    // 카드 리스트 갱신 메서드
    private void RefreshCardList(RectTransform content, List<CardBasic> cardList)
    {
        // 기존 카드 제거
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 데이터에서 카드 추가
        foreach (CardBasic card in cardList)
        {
            GameObject cardObject = CreateCard(card);
            cardObject.transform.SetParent(content, false); // 스크롤 뷰 Content에 카드 추가
        }
    }

    // 뽑을 카드 리스트 갱신
    public void UpdateDeckList()
    {
        // Stack을 List로 변환
        List<CardBasic> deckList = new List<CardBasic>(DataManager.Instance.deck);
        RefreshCardList(deckContent, deckList);
    }

    // 버린 카드 리스트 갱신
    public void UpdateUsedCardsList()
    {
        // Stack을 List로 변환
        List<CardBasic> usedCardsList = new List<CardBasic>(DataManager.Instance.usedCards);
        RefreshCardList(usedCardsContent, usedCardsList);
    }
}
