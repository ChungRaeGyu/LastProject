using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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
    public Image dungeonDeckCards;

    [Header("FadePanel")]
    public GameObject ClearPanelFade;

    [Header("Reward")]
    public Image fadeRewardPanel;
    public GameObject rewardPanel;
    public TMP_Text rewardCoinAmountText;

    [Header("Button")]
    public Button lobbyButton;
    public Button openCardSelectionButton;
    public Button addCoinButton;

    [Header("UI")]
    public Canvas healthBarCanvas;
    public Canvas conditionCanvas;
    public Canvas nextActionIconCanvas;
    public Canvas nextActionDescriptionCanvas;
    public Canvas monsterNameCanvas;
    public TMP_Text costText;
    public TMP_Text TurnText;
    public TMP_Text currentCoinText;

    [Header("TurnCountText")]
    public TMP_Text PlayerTurnCountText;
    public TMP_Text MonsterTurnCountText;

    // TODO: 승리와 패배 싹 나중에 스크립트 하나 만들어서 옮기기
    [Header("Defeat")]
    public GameObject defeatPanel; // 패배 패널
    public Transform removeCardSpawnPoint; // 제거된 카드를 보여줄 위치값
    public TMP_Text defeatMnstersKilledText; // 처치한 몬스터 수
    public TMP_Text defeatStageClearCountText; // 클리어한 스테이지 수
    [Header("DefeatPoint")]
    public TMP_Text defeatMonstersKilledPointText; // 처치한 몬스터 점수
    public TMP_Text defeatStageClearCountPointText; // 클리어한 스테이지 점수
    public TMP_Text defeatTotalCrystalText; // 획득한 크리스탈
    [Header("Victory")]
    public GameObject victoryPanel; // 승리 패널
    public TMP_Text victoryMonstersKilledText; // 처치한 몬스터 수
    public TMP_Text victoryStageClearCountText; // 클리어한 스테이지 수
    public TMP_Text victoryTotalClearTimeText; // 던전 클리어 시간
    public TMP_Text victoryBossesDefeatedCountText; // 보스 처치
    public TMP_Text victoryRemainingCoinCountText; // 잔여 코인
    [Header("VictoryPoint")]
    public TMP_Text victoryMonstersKilledPointText; // 처치한 몬스터 수
    public TMP_Text victoryStageClearCountPointText; // 클리어한 스테이지 수
    public TMP_Text victoryTotalClearTimePointText; // 던전 클리어 시간
    public TMP_Text victoryBossesDefeatedCountPointText; // 보스 처치
    public TMP_Text victoryRemainingCoinCountPointText; // 잔여 코인
    public TMP_Text victoryTotalCrystal; // 획득한 크리스탈

    [Header("VictoryPoint")]
    public AudioSource SFXAudioSource;
    public AudioClip CoinClip;

    // 원래 UI 요소들의 초기 위치를 저장할 변수들
    private Vector2 originalCostImagePosition;
    private RectTransform turnEndButtonRect;
    private Vector2 originalTurnEndButtonPosition;
    private Vector2 originalUnUsedCardsPosition;
    private Vector2 originalUsedCardsPosition;
    private Vector2 originaldungeonDeckCardsPosition;

    // 임시로 카드를 저장할 변수들
    private GameObject centerCard;
    private GameObject leftCard;
    private GameObject rightCard;

    private void Start()
    {
        // 초기 위치 저장
        originalCostImagePosition = costImage.rectTransform.anchoredPosition;
        turnEndButtonRect = turnEndButton.GetComponent<RectTransform>();
        originalTurnEndButtonPosition = turnEndButtonRect.anchoredPosition;
        originalUnUsedCardsPosition = UnUsedCards.rectTransform.anchoredPosition;
        originalUsedCardsPosition = UsedCards.rectTransform.anchoredPosition;
        originaldungeonDeckCardsPosition = dungeonDeckCards.rectTransform.anchoredPosition;

        victoryPanel.gameObject.SetActive(false);
        ClearPanelFade.SetActive(false);
        cardSelectPanel.SetActive(false);
        UIClear(false, true, false, false, false);

        MoveUIElementsToStartPositions();

        UpdatePlayerTurnCount(1); // 초기에 1번째 턴 진행

        currentCoinText.text = DataManager.Instance.currentCoin.ToString();
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

    public void AddCoin()
    {
        DataManager.Instance.currentCoin += GameManager.instance.monsterTotalRewardCoin;

        SFXAudioSource.PlayOneShot(CoinClip);
        currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        addCoinButton.gameObject.SetActive(false);
    }

    public void SpawnRewardCards()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);
        SettingManager.Instance.SFXAudioSource.PlayOneShot(GameManager.instance.rewardCardClip);

        cardSelectPanel.SetActive(true);

        List<int> chosenIndexes = GetRandomIndexes(rewardCardPrefabs.Count, 3);

        // 중앙 카드 생성
        centerCard = Instantiate(rewardCardPrefabs[chosenIndexes[0]], CardSelectPanelCanvas);
        SetCardScale(centerCard);
        centerCard.transform.localPosition = Vector3.zero;
        Destroy(centerCard.transform.GetChild(0).gameObject);
        Destroy(centerCard.GetComponent<CardDrag>());

        // 왼쪽 카드 생성
        leftCard = Instantiate(rewardCardPrefabs[chosenIndexes[1]], CardSelectPanelCanvas);
        SetCardScale(leftCard);
        leftCard.transform.localPosition = Vector3.zero;
        StartCoroutine(MoveCard(leftCard, new Vector3(-400, 0, 0)));
        Destroy(leftCard.transform.GetChild(0).gameObject);
        Destroy(leftCard.GetComponent<CardDrag>());

        // 오른쪽 카드 생성
        rightCard = Instantiate(rewardCardPrefabs[chosenIndexes[2]], CardSelectPanelCanvas);
        SetCardScale(rightCard);
        StartCoroutine(MoveCard(rightCard, new Vector3(400, 0, 0)));
        rightCard.transform.localPosition = Vector3.zero;
        Destroy(rightCard.transform.GetChild(0).gameObject);
        Destroy(rightCard.GetComponent<CardDrag>());

        // 코루틴 실행 및 모든 코루틴이 완료된 후 AddClickEvent 실행
        StartCoroutine(HandleCardMoveAndAddClickEvent(chosenIndexes));
    }

    private IEnumerator HandleCardMoveAndAddClickEvent(List<int> chosenIndexes)
    {
        yield return new WaitForSeconds(0.7f);

        // 모든 카드의 이동이 끝난 후 클릭 이벤트 추가
        AddClickEvent(centerCard, chosenIndexes[0]);
        AddClickEvent(leftCard, chosenIndexes[1]);
        AddClickEvent(rightCard, chosenIndexes[2]);
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
            int randomIndex = UnityEngine.Random.Range(0, indexes.Count);
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

        // 클릭한 보상 카드를 DataManager에 추가
        if (DataManager.Instance != null)
        {
            SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardPassClip);

            // 클릭한 카드를 추가
            CardBasic cardToAdd = rewardCardPrefabs[cardIndex].GetComponent<CardBasic>();
            DataManager.Instance.AddCard(cardToAdd);

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
            openCardSelectionButton.gameObject.SetActive(false);
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

    public void UIClear(bool lobbyBtn, bool turnEndBtn, bool setRewardPanel, bool setFadeRewardPanel, bool setAddCoinButton, float openCardSelectionProbability = 0.6f)
    {
        if (SaveManager.Instance.isEliteStage) openCardSelectionProbability = 1f;

        //SetActive(ClearPanelFade?.gameObject, ); // 패널들과 같음
        SetActive(lobbyButton?.gameObject, lobbyBtn);
        SetActive(turnEndButton?.gameObject, turnEndBtn);
        SetActive(rewardPanel, setRewardPanel);
        rewardCoinAmountText.text = $"던전코인 {GameManager.instance.monsterTotalRewardCoin}개";
        SetActive(fadeRewardPanel?.gameObject, setFadeRewardPanel);
        SetActive(addCoinButton?.gameObject, setAddCoinButton);

        if (openCardSelectionButton != null)
        {
            bool shouldBeActive = UnityEngine.Random.value <= openCardSelectionProbability;
            openCardSelectionButton.gameObject.SetActive(shouldBeActive);
        }
    }

    // 게임 오브젝트의 활성화/비활성화 상태 설정
    private void SetActive(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
        }
    }

    // UI들의 위치를 전투 진행에 알맞는 위치로 옮기는 메서드
    public void MoveUIElementsToStartPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, new Vector2(200, costImage.rectTransform.anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, new Vector2(-200, turnEndButtonRect.anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, new Vector2(40, 40), 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, new Vector2(-40, 40), 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, new Vector2(-30, -150), 0.5f));
    }

    // 모든 적을 처치시 모든 전투 진행 UI들을 안보이게 하는 메서드
    public void ResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, originalUnUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, originalUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, originaldungeonDeckCardsPosition, 0.5f));
    }

    // 사용안한 덱 더미를 눌렀을 때 해당 덱 더미를 제외한 UI숨기기 메서드
    public void UnUsedCardsResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, originalUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, originaldungeonDeckCardsPosition, 0.5f));
    }

    // 사용한 덱 더미를 눌렀을 때 해당 덱 더미를 제외한 UI숨기기 메서드
    public void UsedCardsResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, originalUnUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, originaldungeonDeckCardsPosition, 0.5f));
    }

    // 사용한 덱 더미를 눌렀을 때 해당 덱 더미를 제외한 UI숨기기 메서드
    public void dungeonDeckCardsResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, originalUnUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, originalUsedCardsPosition, 0.5f));
    }

    // 패배 시 호출될 메서드
    public void ShowDefeatPanel()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
            ClearPanelFade.SetActive(true);

            DataManager.Instance.DefeatCalculateTotalCrystal();
            if (defeatTotalCrystalText != null)
            {
                defeatTotalCrystalText.text = $"{DataManager.Instance.DefeatTotalCrystal}";
            }
            // 텍스트 업데이트
            UpdateDefeatTexts();

        }

        if (fadeRewardPanel != null)
        {
            fadeRewardPanel.gameObject.SetActive(true);
        }
    }

    private void UpdateDefeatTexts()
    {
        SetText(defeatMnstersKilledText, $"처치한 몬스터 ({DataManager.Instance.ClearMonstersKilledCount})");
        SetText(defeatStageClearCountText, $"클리어한 스테이지 ({DataManager.Instance.ClearStageClearCount})");

        SetText(defeatMonstersKilledPointText, $"{DataManager.Instance.adjustedDefeatMonstersKilledCount}");
        SetText(defeatStageClearCountPointText, $"{DataManager.Instance.adjustedDefeatStageClearCount}");

        SetText(defeatTotalCrystalText, $"{DataManager.Instance.DefeatTotalCrystal}");
    }

    private void SetText(TMP_Text textComponent, string text)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
        }
    }

    public void ApplyDeathPenalty()
    {
        // 덱 리스트에서 랜덤으로 카드를 제거한다.
        if (DataManager.Instance.deckList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, DataManager.Instance.deckList.Count);
            CardBasic removedCard = DataManager.Instance.deckList[randomIndex];
            DataManager.Instance.deckList.RemoveAt(randomIndex);

            // 제거된 카드를 화면에 보여주고 사라지는 효과를 구현한다.
            ShowRemovedCard(removedCard);
        }
        else
        {
            Debug.LogWarning("덱에 제거할 카드가 없습니다.");
        }
    }

    private void ShowRemovedCard(CardBasic cardToRemove)
    {
        // 제거된 카드를 생성하고 부모를 removeCardSpawnPoint로 설정
        GameObject removedCardObj = Instantiate(cardToRemove.gameObject, removeCardSpawnPoint);
        removedCardObj.SetActive(true);

        // 자식 오브젝트들의 Image 컴포넌트를 가져옴
        Image[] cardImages = removedCardObj.GetComponentsInChildren<Image>();

        if (cardImages.Length > 0)
        {
            // 첫 번째 자식 이미지 삭제 <= 형광색 효과 이미지
            Destroy(cardImages[0].gameObject);
        }

        if (cardImages.Length > 1)
        {
            // 두 번째 자식 이미지와 그 안의 TMP_Text 컴포넌트들 가져오기
            Image secondImage = cardImages[1];
            TMP_Text[] textComponents = secondImage.GetComponentsInChildren<TMP_Text>();

            // 두 번째 자식 이미지와 TMP_Text 컴포넌트들의 알파값을 조절하는 메서드 호출
            StartCoroutine(FadeOutAndDestroy(secondImage, textComponents));
        }
        else
        {
            Debug.LogWarning("두 번째 자식 오브젝트에서 Image 컴포넌트를 찾을 수 없습니다.");
        }

        // 이미지 크기 조정
        removedCardObj.transform.localScale = new Vector3(4f, 6f, 1f); // 2배 크기로 설정
    }

    private IEnumerator FadeOutAndDestroy(Image cardImage, TMP_Text[] textComponents)
    {
        float fadeDuration = 2.0f;
        float fadeTimer = 0.0f;

        Color originalColor = cardImage.color;

        // 텍스트 컴포넌트의 원래 색상 저장
        Color[] originalTextColors = new Color[textComponents.Length];
        for (int i = 0; i < textComponents.Length; i++)
        {
            originalTextColors[i] = textComponents[i].color;
        }

        yield return new WaitForSeconds(1.0f); // 1초 지연

        // 점점 투명해지는 효과
        while (fadeTimer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
            Color newImageColor = originalColor;
            newImageColor.a = alpha;
            cardImage.color = newImageColor;

            // 텍스트 컴포넌트의 알파값 조정
            foreach (var textComponent in textComponents)
            {
                Color newTextColor = originalTextColors[Array.IndexOf(textComponents, textComponent)];
                newTextColor.a = alpha;
                textComponent.color = newTextColor;
            }

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // 카드가 완전히 투명해진 후 제거
        Destroy(cardImage.gameObject);
    }

    public void UpdatePlayerTurnCount(int turnNumber)
    {
        if (GameManager.instance.player?.IsDead() == true) return;

        SettingManager.Instance.PlaySound(GameManager.instance.turnClip);
        StartCoroutine(AnimateTurnCount(PlayerTurnCountText, $"플레이어 {turnNumber}번째 턴"));
    }

    public void UpdateMonsterTurnCount(int turnNumber)
    {
        if (GameManager.instance.player?.IsDead() == true) return;

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

    // 로비 씬으로 이동하는 메서드 죽었을 때 뜨는 패널 하단의 버튼
    public void GoToLobbyScene()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);

        SaveManager.Instance.isBossStage = false;
        SaveManager.Instance.isEliteStage = false;
        DataManager.Instance.currentCrystal += DataManager.Instance.DefeatTotalCrystal;
        SaveManager.Instance.accessDungeon = false;
        SceneFader.instance.LoadSceneWithFade(1); // 로비 씬의 빌드 인덱스를 사용하여 로드
    }
}
