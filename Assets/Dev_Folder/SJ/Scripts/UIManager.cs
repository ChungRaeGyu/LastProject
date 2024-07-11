using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public List<GameObject> rewardCardPrefabs;
    public Transform CardSelectPanelCanvas;
    public GameObject cardSelectPanel;

    [Header("MainUI")]
    public Image costImage;
    public Button turnEndButton;
    public Image UnUsedCards;
    public Image UsedCards;

    [Header("Button")]
    public Button lobbyButton;
    public Button openCardSelectionButton;

    [Header("Reward")]
    public Image fadeRewardPanel;
    public GameObject rewardPanel;

    [Header("UI")]
    public Canvas healthBarCanvas;
    public TMP_Text costText;
    public TMP_Text TurnText;

    private void Start()
    {
        cardSelectPanel.SetActive(false);
        UIClear(false, true, false, false, false);

        StartCoroutine(MoveUIElement(costImage.rectTransform, new Vector2(-725, costImage.rectTransform.anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(turnEndButton.GetComponent<RectTransform>(), new Vector2(-130, turnEndButton.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, new Vector2(40, 40), 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, new Vector2(-40, 40), 0.5f));
    }

    private IEnumerator MoveUIElement(RectTransform rectTransform, Vector2 targetPosition, float duration)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    public void SpawnRewardCards()
    {
        cardSelectPanel.SetActive(true);

        List<int> chosenIndexes = GetRandomIndexes(rewardCardPrefabs.Count, 3);

        // 중앙 카드 생성
        GameObject centerCard = Instantiate(rewardCardPrefabs[chosenIndexes[0]], CardSelectPanelCanvas);
        SetCardScale(centerCard);
        centerCard.transform.localPosition = Vector3.zero;
        AddClickEvent(centerCard, chosenIndexes[0]);

        // 왼쪽 카드 생성
        GameObject leftCard = Instantiate(rewardCardPrefabs[chosenIndexes[1]], CardSelectPanelCanvas);
        SetCardScale(leftCard);
        leftCard.transform.localPosition = Vector3.zero;
        AddClickEvent(leftCard, chosenIndexes[1]);
        StartCoroutine(MoveCard(leftCard, new Vector3(-400, 0, 0)));

        // 오른쪽 카드 생성
        GameObject rightCard = Instantiate(rewardCardPrefabs[chosenIndexes[2]], CardSelectPanelCanvas);
        SetCardScale(rightCard);
        rightCard.transform.localPosition = Vector3.zero;
        AddClickEvent(rightCard, chosenIndexes[2]);
        StartCoroutine(MoveCard(rightCard, new Vector3(400, 0, 0)));
    }

    private List<int> GetRandomIndexes(int count, int numberOfIndexes)
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i < count; i++)
        {
            indexes.Add(i);
        }

        List<int> chosenIndexes = new List<int>();
        for (int i = 0; i < numberOfIndexes; i++)
        {
            int randomIndex = Random.Range(0, indexes.Count);
            chosenIndexes.Add(indexes[randomIndex]);
            indexes.RemoveAt(randomIndex);
        }

        return chosenIndexes;
    }

    private void SetCardScale(GameObject card)
    {
        card.transform.localScale = new Vector3(2.8f, 4.2f, 1f);
    }

    private void AddClickEvent(GameObject card, int cardIndex)
    {
        var clickableArea = card.GetComponent<Image>();
        if (clickableArea == null)
        {
            clickableArea = card.AddComponent<Image>();
            clickableArea.color = Color.clear; // 투명한 색으로 설정하여 보이지 않게 함
        }

        var button = card.GetComponent<Button>();
        if (button == null)
        {
            button = card.AddComponent<Button>();
        }

        button.onClick.AddListener(() => OnClickRewardCard(cardIndex));
    }

    private void OnClickRewardCard(int cardIndex)
    {
        Debug.Log($"{cardIndex}");

        // 클릭한 보상 카드를 DataManager에 추가
        if (DataManager.Instance != null)
        {
            CardBasic cardToAdd = rewardCardPrefabs[cardIndex].GetComponent<CardBasic>();
            DataManager.Instance.deckList.Add(cardToAdd);

            // 다른 두 카드 삭제
            for (int i = 0; i < rewardCardPrefabs.Count; i++)
            {
                if (i != cardIndex)
                {
                    CardBasic otherCard = rewardCardPrefabs[i].GetComponent<CardBasic>();
                    Destroy(otherCard.gameObject);
                }
            }

            // 카드 선택 패널을 비활성화
            cardSelectPanel.SetActive(false);

            // openCardSelectionButton 비활성화
            if (openCardSelectionButton != null)
            {
                openCardSelectionButton.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator MoveCard(GameObject card, Vector3 targetPosition)
    {
        float duration = 0.5f;
        float elapsedTime = 0;
        Vector3 startingPos = card.transform.localPosition;

        while (elapsedTime < duration)
        {
            card.transform.localPosition = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.localPosition = targetPosition;
    }

    public void UIClear(bool lobbyBtn, bool turnEndBtn, bool setRewardPanel, bool setFadeRewardPanel, bool setOpenCardSelectionButton)
    {
        if (lobbyButton != null)
        {
            lobbyButton.gameObject.SetActive(lobbyBtn);
        }

        if (turnEndButton != null)
        {
            turnEndButton.gameObject.SetActive(turnEndBtn);
        }

        if (rewardPanel != null)
        {
            rewardPanel.gameObject.SetActive(setRewardPanel);
        }

        if (fadeRewardPanel != null)
        {
            fadeRewardPanel.gameObject.SetActive(setFadeRewardPanel);
        }

        if (openCardSelectionButton != null)
        {
            openCardSelectionButton.gameObject.SetActive(setOpenCardSelectionButton);
        }
    }
}
