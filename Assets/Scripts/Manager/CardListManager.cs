using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CardListManager : MonoBehaviour
{
    public RectTransform deckContent; // 뽑을 카드 리스트의 스크롤 뷰 Content
    public ContentHeightAdjuster unUsedCardsContentHeightAdjuster; // 뽑을 카드 리스트의 ContentHeightAdjuster
    public Button sortDeckButton; // 뽑을 카드 리스트 정렬 버튼
    public Image unUsedCostSortImage; // UnUsedCostSort 화살표 이미지

    private bool isDeckAscending = true; // 뽑을 카드 리스트 오름차순 정렬 여부
    private Color defaultButtonColor; // 기본 버튼 색
    private Color selectedButtonColor = Color.yellow; // 선택된 버튼 색

    private void Start()
    {
        // 기본 버튼 색 저장
        defaultButtonColor = sortDeckButton.GetComponent<Image>().color;
    }

    // 카드 데이터에 따라 카드 생성 및 추가
    private GameObject CreateCard(CardBasic cardData)
    {
        // 카드 프리팹을 사용하여 카드 생성
        GameObject newCard = Instantiate(cardData.gameObject, Vector3.zero, Quaternion.identity);
        newCard.GetComponent<CardBasic>().cardBasic = cardData;

        // 메인씬에서 CardDrag 컴포넌트 제거
        if (SceneManager.GetActiveScene().buildIndex == 3)
            Destroy(newCard.GetComponent<CardDrag>());

        ProcessCardObject(newCard);

        return newCard;
    }

    // 카드 오브젝트의 자식 처리
    private void ProcessCardObject(GameObject cardObject)
    {
        RectTransform textRectTransform = cardObject.transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
        if (textRectTransform != null)
        {
            // PosX에 3.25를 빼고, PosY에 4를 더함 (코스트의 텍스트 위치가 안 맞아서 임시로 조정)
            Vector2 newPosition = textRectTransform.anchoredPosition;
            newPosition.x -= 3.25f;
            newPosition.y += 4f;
            textRectTransform.anchoredPosition = newPosition;
        }

        // 카드 오브젝트의 자식에서 카드 이미지 제거
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
        unUsedCardsContentHeightAdjuster.cardCount = DataManager.Instance.deck.Count;
        unUsedCardsContentHeightAdjuster.AdjustContentHeight();
    }

    // 버린 카드 리스트 갱신
    public void UpdateUsedCardsList()
    {
        // Stack을 List로 변환
        List<CardBasic> usedCardsList = new List<CardBasic>(DataManager.Instance.usedCards);
        RefreshCardList(deckContent, usedCardsList);
        unUsedCardsContentHeightAdjuster.cardCount = DataManager.Instance.usedCards.Count;
        unUsedCardsContentHeightAdjuster.AdjustContentHeight();
    }

    // 뽑을 카드 리스트 갱신
    public void UpdateDungeonDeckList()
    {
        // Stack을 List로 변환
        List<CardBasic> deckList = new List<CardBasic>(DataManager.Instance.deckList);
        RefreshCardList(deckContent, deckList);
        unUsedCardsContentHeightAdjuster.cardCount = DataManager.Instance.deckList.Count;
        unUsedCardsContentHeightAdjuster.AdjustContentHeight();
    }

    // 정렬 순서 변경 및 덱 정렬
    public void ToggleSortOrderAndSortDeck()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        isDeckAscending = !isDeckAscending;
        SortDeckByCost();
    }

    // 덱을 코스트 기준으로 정렬
    private void SortDeckByCost()
    {
        SortCardsByCost(deckContent, isDeckAscending);
        // 화살표 이미지 회전
        unUsedCostSortImage.transform.rotation = isDeckAscending ? Quaternion.Euler(180, 0, 0) : Quaternion.Euler(0, 0, 0);
        unUsedCardsContentHeightAdjuster.AdjustContentHeight();
    }

    // 주어진 컨텐츠를 코스트 기준으로 정렬
    private void SortCardsByCost(RectTransform content, bool isAscending)
    {
        List<Transform> cards = new List<Transform>();

        // 현재 content 내의 자식 카드들을 리스트에 추가
        foreach (Transform card in content)
        {
            cards.Add(card);
        }

        // 카드 오브젝트들을 cost 기준으로 정렬
        if (isAscending)
        {
            cards.Sort((x, y) => x.GetComponent<CardBasic>().cost.CompareTo(y.GetComponent<CardBasic>().cost)); // 오름차순 정렬
        }
        else
        {
            cards.Sort((x, y) => y.GetComponent<CardBasic>().cost.CompareTo(x.GetComponent<CardBasic>().cost)); // 내림차순 정렬
        }

        // 정렬된 순서대로 hierarchy에서 재배치
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetSiblingIndex(i);
        }
    }
}
