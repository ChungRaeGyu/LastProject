using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Canvas conditionCanvas;
    public TMP_Text costText;
    public TMP_Text TurnText;

    [Header("TurnCountText")]
    public TMP_Text PlayerTurnCountText;
    public TMP_Text MonsterTurnCountText;

    [Header("Defeat")]
    public GameObject defeatPanel;
    public Transform removeCardSpawnPoint; // 제거된 카드를 보여줄 위치값
    public TMP_Text monstersKilledText; // 처치한 몬스터 수

    // 원래 UI 요소들의 초기 위치를 저장할 변수들
    private Vector2 originalCostImagePosition;
    private Vector2 originalTurnEndButtonPosition;
    private Vector2 originalUnUsedCardsPosition;
    private Vector2 originalUsedCardsPosition;

    // 임시로 카드를 저장할 변수들
    private GameObject centerCard;
    private GameObject leftCard;
    private GameObject rightCard;

    private void Start()
    {
        // 초기 위치 저장
        originalCostImagePosition = costImage.rectTransform.anchoredPosition;
        originalTurnEndButtonPosition = turnEndButton.GetComponent<RectTransform>().anchoredPosition;
        originalUnUsedCardsPosition = UnUsedCards.rectTransform.anchoredPosition;
        originalUsedCardsPosition = UsedCards.rectTransform.anchoredPosition;

        cardSelectPanel.SetActive(false);
        UIClear(false, true, false, false, false);

        MoveUIElementsToStartPositions();

        UpdatePlayerTurnCount(1); // 초기에 1번째 턴 진행
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
        centerCard = Instantiate(rewardCardPrefabs[chosenIndexes[0]], CardSelectPanelCanvas);
        SetCardScale(centerCard);
        centerCard.transform.localPosition = Vector3.zero;
        AddClickEvent(centerCard, chosenIndexes[0]);
        Destroy(centerCard.transform.GetChild(0).gameObject);

        // 왼쪽 카드 생성
        leftCard = Instantiate(rewardCardPrefabs[chosenIndexes[1]], CardSelectPanelCanvas);
        SetCardScale(leftCard);
        leftCard.transform.localPosition = Vector3.zero;
        AddClickEvent(leftCard, chosenIndexes[1]);
        StartCoroutine(MoveCard(leftCard, new Vector3(-400, 0, 0)));
        Destroy(leftCard.transform.GetChild(0).gameObject);


        // 오른쪽 카드 생성
        rightCard = Instantiate(rewardCardPrefabs[chosenIndexes[2]], CardSelectPanelCanvas);
        SetCardScale(rightCard);
        rightCard.transform.localPosition = Vector3.zero;
        AddClickEvent(rightCard, chosenIndexes[2]);
        StartCoroutine(MoveCard(rightCard, new Vector3(400, 0, 0)));
        Destroy(rightCard.transform.GetChild(0).gameObject);
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
        var button = card.GetComponent<Button>();
        if (button == null)
        {
            button = card.AddComponent<Button>();
        }

        button.onClick.AddListener(() => OnClickRewardCard(cardIndex));
    }

    private void OnClickRewardCard(int cardIndex)
    {
        Debug.Log($"클릭된 카드의 인덱스 {cardIndex}");

        // 클릭한 보상 카드를 DataManager에 추가
        if (DataManager.Instance != null)
        {
            // 클릭한 카드를 추가
            CardBasic cardToAdd = rewardCardPrefabs[cardIndex].GetComponent<CardBasic>();
            DataManager.Instance.deckList.Add(cardToAdd);

            // 다른 두 카드 삭제
            for (int i = 0; i < rewardCardPrefabs.Count; i++)
            {
                if (i != cardIndex)
                {
                    Destroy(centerCard);
                    Destroy(leftCard);
                    Destroy(rightCard);
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

    public void UIClear(bool lobbyBtn, bool turnEndBtn, bool setRewardPanel, bool setFadeRewardPanel, bool setOpenCardSelectionButton, float openCardSelectionProbability = 0.5f)
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
            // 확률적으로 활성화
            bool shouldBeActive = Random.value <= openCardSelectionProbability;
            openCardSelectionButton.gameObject.SetActive(shouldBeActive);
        }
    }

    private void MoveUIElementsToStartPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, new Vector2(-725, costImage.rectTransform.anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(turnEndButton.GetComponent<RectTransform>(), new Vector2(-130, turnEndButton.GetComponent<RectTransform>().anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, new Vector2(40, 40), 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, new Vector2(-40, 40), 0.5f));
    }

    public void ResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButton.GetComponent<RectTransform>(), originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, originalUnUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, originalUsedCardsPosition, 0.5f));
    }

    // 패배 시 호출될 메서드
    public void ShowDefeatPanel()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);

            // 처치한 몬스터 수를 표시하는 부분
            if (monstersKilledText != null)
            {
                monstersKilledText.text = $"처치한 몬스터 수: {DataManager.Instance.monstersKilledCount}";
            }
        }

        if (fadeRewardPanel != null)
        {
            fadeRewardPanel.gameObject.SetActive(true);
        }
    }

    public void ApplyDeathPenalty()
    {
        // 덱 리스트에서 랜덤으로 카드를 제거합니다.
        if (DataManager.Instance.deckList.Count > 0)
        {
            int randomIndex = Random.Range(0, DataManager.Instance.deckList.Count);
            CardBasic removedCard = DataManager.Instance.deckList[randomIndex];
            DataManager.Instance.deckList.RemoveAt(randomIndex);

            // 제거된 카드를 화면에 보여주고 사라지는 효과를 구현합니다.
            ShowRemovedCard(removedCard);
        }
        else
        {
            Debug.LogWarning("덱에 제거할 카드가 없습니다.");
        }
    }

    private void ShowRemovedCard(CardBasic cardToRemove)
    {
        // 제거된 카드를 생성하고 부모를 removeCardSpawnPoint로 설정합니다.
        GameObject removedCardObj = Instantiate(cardToRemove.gameObject, removeCardSpawnPoint);
        removedCardObj.SetActive(true);

        // 스케일 조정
        removedCardObj.transform.localScale = new Vector3(4f, 6f, 1f); // 2배 크기로 설정

        // 자식 오브젝트의 Image 컴포넌트 가져오기
        Image cardImage = removedCardObj.GetComponentInChildren<Image>();
        if (cardImage != null)
        {
            // 카드의 알파값을 조정하기 위해 코루틴 호출
            StartCoroutine(FadeOutAndDestroy(cardImage));
        }
        else
        {
            Debug.LogWarning("자식 오브젝트에서 Image 컴포넌트를 찾을 수 없습니다.");
        }
    }


    private IEnumerator FadeOutAndDestroy(Image cardImage)
    {
        float fadeDuration = 2.0f;
        float fadeTimer = 0.0f;

        Color originalColor = cardImage.color;

        yield return new WaitForSeconds(1.0f); // 1초 지연

        // 점점 투명해지는 효과
        while (fadeTimer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
            Color newColor = originalColor;
            newColor.a = alpha;
            cardImage.color = newColor;

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // 카드가 완전히 투명해진 후 제거
        Destroy(cardImage.gameObject);
    }

    public void UpdatePlayerTurnCount(int turnNumber)
    {
        StartCoroutine(AnimateTurnCount(PlayerTurnCountText, $"플레이어 {turnNumber}번째 턴"));
    }

    public void UpdateMonsterTurnCount(int turnNumber)
    {
        StartCoroutine(AnimateTurnCount(MonsterTurnCountText, $"몬스터 {turnNumber}번째 턴"));
    }

    private IEnumerator AnimateTurnCount(TMP_Text textElement, string textToShow)
    {
        float maxAlpha = 1.0f;
        float animationDuration = 0.5f;
        float pauseDuration = 0.5f;

        textElement.gameObject.SetActive(true);

        // Alpha 값을 0에서 maxAlpha로 증가
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float alpha = Mathf.Lerp(0f, maxAlpha, elapsedTime / animationDuration);
            textElement.alpha = alpha;
            textElement.text = textToShow;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 일정 시간 동안 대기
        yield return new WaitForSeconds(pauseDuration);

        // Alpha 값을 maxAlpha에서 0으로 감소
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float alpha = Mathf.Lerp(maxAlpha, 0f, elapsedTime / animationDuration);
            textElement.alpha = alpha;
            textElement.text = textToShow;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textElement.gameObject.SetActive(false);
    }

    // 로비 씬으로 이동하는 메서드
    public void GoToLobbyScene()
    {
        SceneManager.LoadScene(1); // 로비 씬의 빌드 인덱스를 사용하여 로드
    }
}
