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

    // TODO: �¸��� �й� �� ���߿� ��ũ��Ʈ �ϳ� ���� �ű��
    [Header("Defeat")]
    public GameObject defeatPanel; // �й� �г�
    public Transform removeCardSpawnPoint; // ���ŵ� ī�带 ������ ��ġ��
    public TMP_Text defeatMnstersKilledText; // óġ�� ���� ��
    public TMP_Text defeatStageClearCountText; // Ŭ������ �������� ��
    [Header("DefeatPoint")]
    public TMP_Text defeatMonstersKilledPointText; // óġ�� ���� ����
    public TMP_Text defeatStageClearCountPointText; // Ŭ������ �������� ����
    public TMP_Text defeatTotalCrystalText; // ȹ���� ũ����Ż
    [Header("Victory")]
    public GameObject victoryPanel; // �¸� �г�
    public TMP_Text victoryMonstersKilledText; // óġ�� ���� ��
    public TMP_Text victoryStageClearCountText; // Ŭ������ �������� ��
    public TMP_Text victoryTotalClearTimeText; // ���� Ŭ���� �ð�
    public TMP_Text victoryBossesDefeatedCountText; // ���� óġ
    public TMP_Text victoryRemainingCoinCountText; // �ܿ� ����
    [Header("VictoryPoint")]
    public TMP_Text victoryMonstersKilledPointText; // óġ�� ���� ��
    public TMP_Text victoryStageClearCountPointText; // Ŭ������ �������� ��
    public TMP_Text victoryTotalClearTimePointText; // ���� Ŭ���� �ð�
    public TMP_Text victoryBossesDefeatedCountPointText; // ���� óġ
    public TMP_Text victoryRemainingCoinCountPointText; // �ܿ� ����
    public TMP_Text victoryTotalCrystal; // ȹ���� ũ����Ż

    [Header("VictoryPoint")]
    public AudioSource SFXAudioSource;
    public AudioClip CoinClip;

    // ���� UI ��ҵ��� �ʱ� ��ġ�� ������ ������
    private Vector2 originalCostImagePosition;
    private RectTransform turnEndButtonRect;
    private Vector2 originalTurnEndButtonPosition;
    private Vector2 originalUnUsedCardsPosition;
    private Vector2 originalUsedCardsPosition;
    private Vector2 originaldungeonDeckCardsPosition;

    // �ӽ÷� ī�带 ������ ������
    private GameObject centerCard;
    private GameObject leftCard;
    private GameObject rightCard;

    private void Start()
    {
        // �ʱ� ��ġ ����
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

        UpdatePlayerTurnCount(1); // �ʱ⿡ 1��° �� ����

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

        // �߾� ī�� ����
        centerCard = Instantiate(rewardCardPrefabs[chosenIndexes[0]], CardSelectPanelCanvas);
        SetCardScale(centerCard);
        centerCard.transform.localPosition = Vector3.zero;
        Destroy(centerCard.transform.GetChild(0).gameObject);
        Destroy(centerCard.GetComponent<CardDrag>());

        // ���� ī�� ����
        leftCard = Instantiate(rewardCardPrefabs[chosenIndexes[1]], CardSelectPanelCanvas);
        SetCardScale(leftCard);
        leftCard.transform.localPosition = Vector3.zero;
        StartCoroutine(MoveCard(leftCard, new Vector3(-400, 0, 0)));
        Destroy(leftCard.transform.GetChild(0).gameObject);
        Destroy(leftCard.GetComponent<CardDrag>());

        // ������ ī�� ����
        rightCard = Instantiate(rewardCardPrefabs[chosenIndexes[2]], CardSelectPanelCanvas);
        SetCardScale(rightCard);
        StartCoroutine(MoveCard(rightCard, new Vector3(400, 0, 0)));
        rightCard.transform.localPosition = Vector3.zero;
        Destroy(rightCard.transform.GetChild(0).gameObject);
        Destroy(rightCard.GetComponent<CardDrag>());

        // �ڷ�ƾ ���� �� ��� �ڷ�ƾ�� �Ϸ�� �� AddClickEvent ����
        StartCoroutine(HandleCardMoveAndAddClickEvent(chosenIndexes));
    }

    private IEnumerator HandleCardMoveAndAddClickEvent(List<int> chosenIndexes)
    {
        yield return new WaitForSeconds(0.7f);

        // ��� ī���� �̵��� ���� �� Ŭ�� �̺�Ʈ �߰�
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

        // Ŭ���� ���� ī�带 DataManager�� �߰�
        if (DataManager.Instance != null)
        {
            SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardPassClip);

            // Ŭ���� ī�带 �߰�
            CardBasic cardToAdd = rewardCardPrefabs[cardIndex].GetComponent<CardBasic>();
            DataManager.Instance.AddCard(cardToAdd);

            // �ٸ� �� ī�� ����
            for (int i = 0; i < rewardCardPrefabs.Count; i++)
            {
                if (i != cardIndex)
                {
                    Destroy(centerCard);
                    Destroy(leftCard);
                    Destroy(rightCard);
                }
            }

            // ī�� ���� �г��� ��Ȱ��ȭ
            cardSelectPanel.SetActive(false);

            // openCardSelectionButton ��Ȱ��ȭ
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

        //SetActive(ClearPanelFade?.gameObject, ); // �гε�� ����
        SetActive(lobbyButton?.gameObject, lobbyBtn);
        SetActive(turnEndButton?.gameObject, turnEndBtn);
        SetActive(rewardPanel, setRewardPanel);
        rewardCoinAmountText.text = $"�������� {GameManager.instance.monsterTotalRewardCoin}��";
        SetActive(fadeRewardPanel?.gameObject, setFadeRewardPanel);
        SetActive(addCoinButton?.gameObject, setAddCoinButton);

        if (openCardSelectionButton != null)
        {
            bool shouldBeActive = UnityEngine.Random.value <= openCardSelectionProbability;
            openCardSelectionButton.gameObject.SetActive(shouldBeActive);
        }
    }

    // ���� ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ ���� ����
    private void SetActive(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
        }
    }

    // UI���� ��ġ�� ���� ���࿡ �˸´� ��ġ�� �ű�� �޼���
    public void MoveUIElementsToStartPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, new Vector2(200, costImage.rectTransform.anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, new Vector2(-200, turnEndButtonRect.anchoredPosition.y), 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, new Vector2(40, 40), 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, new Vector2(-40, 40), 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, new Vector2(-30, -150), 0.5f));
    }

    // ��� ���� óġ�� ��� ���� ���� UI���� �Ⱥ��̰� �ϴ� �޼���
    public void ResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, originalUnUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, originalUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, originaldungeonDeckCardsPosition, 0.5f));
    }

    // ������ �� ���̸� ������ �� �ش� �� ���̸� ������ UI����� �޼���
    public void UnUsedCardsResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, originalUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, originaldungeonDeckCardsPosition, 0.5f));
    }

    // ����� �� ���̸� ������ �� �ش� �� ���̸� ������ UI����� �޼���
    public void UsedCardsResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, originalUnUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(dungeonDeckCards.rectTransform, originaldungeonDeckCardsPosition, 0.5f));
    }

    // ����� �� ���̸� ������ �� �ش� �� ���̸� ������ UI����� �޼���
    public void dungeonDeckCardsResetUIPositions()
    {
        StartCoroutine(MoveUIElement(costImage.rectTransform, originalCostImagePosition, 0.5f));
        StartCoroutine(MoveUIElement(turnEndButtonRect, originalTurnEndButtonPosition, 0.5f));
        StartCoroutine(MoveUIElement(UnUsedCards.rectTransform, originalUnUsedCardsPosition, 0.5f));
        StartCoroutine(MoveUIElement(UsedCards.rectTransform, originalUsedCardsPosition, 0.5f));
    }

    // �й� �� ȣ��� �޼���
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
            // �ؽ�Ʈ ������Ʈ
            UpdateDefeatTexts();

        }

        if (fadeRewardPanel != null)
        {
            fadeRewardPanel.gameObject.SetActive(true);
        }
    }

    private void UpdateDefeatTexts()
    {
        SetText(defeatMnstersKilledText, $"óġ�� ���� ({DataManager.Instance.ClearMonstersKilledCount})");
        SetText(defeatStageClearCountText, $"Ŭ������ �������� ({DataManager.Instance.ClearStageClearCount})");

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
        // �� ����Ʈ���� �������� ī�带 �����Ѵ�.
        if (DataManager.Instance.deckList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, DataManager.Instance.deckList.Count);
            CardBasic removedCard = DataManager.Instance.deckList[randomIndex];
            DataManager.Instance.deckList.RemoveAt(randomIndex);

            // ���ŵ� ī�带 ȭ�鿡 �����ְ� ������� ȿ���� �����Ѵ�.
            ShowRemovedCard(removedCard);
        }
        else
        {
            Debug.LogWarning("���� ������ ī�尡 �����ϴ�.");
        }
    }

    private void ShowRemovedCard(CardBasic cardToRemove)
    {
        // ���ŵ� ī�带 �����ϰ� �θ� removeCardSpawnPoint�� ����
        GameObject removedCardObj = Instantiate(cardToRemove.gameObject, removeCardSpawnPoint);
        removedCardObj.SetActive(true);

        // �ڽ� ������Ʈ���� Image ������Ʈ�� ������
        Image[] cardImages = removedCardObj.GetComponentsInChildren<Image>();

        if (cardImages.Length > 0)
        {
            // ù ��° �ڽ� �̹��� ���� <= ������ ȿ�� �̹���
            Destroy(cardImages[0].gameObject);
        }

        if (cardImages.Length > 1)
        {
            // �� ��° �ڽ� �̹����� �� ���� TMP_Text ������Ʈ�� ��������
            Image secondImage = cardImages[1];
            TMP_Text[] textComponents = secondImage.GetComponentsInChildren<TMP_Text>();

            // �� ��° �ڽ� �̹����� TMP_Text ������Ʈ���� ���İ��� �����ϴ� �޼��� ȣ��
            StartCoroutine(FadeOutAndDestroy(secondImage, textComponents));
        }
        else
        {
            Debug.LogWarning("�� ��° �ڽ� ������Ʈ���� Image ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // �̹��� ũ�� ����
        removedCardObj.transform.localScale = new Vector3(4f, 6f, 1f); // 2�� ũ��� ����
    }

    private IEnumerator FadeOutAndDestroy(Image cardImage, TMP_Text[] textComponents)
    {
        float fadeDuration = 2.0f;
        float fadeTimer = 0.0f;

        Color originalColor = cardImage.color;

        // �ؽ�Ʈ ������Ʈ�� ���� ���� ����
        Color[] originalTextColors = new Color[textComponents.Length];
        for (int i = 0; i < textComponents.Length; i++)
        {
            originalTextColors[i] = textComponents[i].color;
        }

        yield return new WaitForSeconds(1.0f); // 1�� ����

        // ���� ���������� ȿ��
        while (fadeTimer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
            Color newImageColor = originalColor;
            newImageColor.a = alpha;
            cardImage.color = newImageColor;

            // �ؽ�Ʈ ������Ʈ�� ���İ� ����
            foreach (var textComponent in textComponents)
            {
                Color newTextColor = originalTextColors[Array.IndexOf(textComponents, textComponent)];
                newTextColor.a = alpha;
                textComponent.color = newTextColor;
            }

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // ī�尡 ������ �������� �� ����
        Destroy(cardImage.gameObject);
    }

    public void UpdatePlayerTurnCount(int turnNumber)
    {
        if (GameManager.instance.player?.IsDead() == true) return;

        SettingManager.Instance.PlaySound(GameManager.instance.turnClip);
        StartCoroutine(AnimateTurnCount(PlayerTurnCountText, $"�÷��̾� {turnNumber}��° ��"));
    }

    public void UpdateMonsterTurnCount(int turnNumber)
    {
        if (GameManager.instance.player?.IsDead() == true) return;

        StartCoroutine(AnimateTurnCount(MonsterTurnCountText, $"���� {turnNumber}��° ��"));
    }

    private IEnumerator AnimateTurnCount(TMP_Text textElement, string textToShow)
    {
        float maxAlpha = 1.0f;
        float animationDuration = 0.5f;
        float pauseDuration = 0.5f;

        textElement.gameObject.SetActive(true);

        // Alpha ���� 0���� maxAlpha�� ����
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float alpha = Mathf.Lerp(0f, maxAlpha, elapsedTime / animationDuration);
            textElement.alpha = alpha;
            textElement.text = textToShow;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� �ð� ���� ���
        yield return new WaitForSeconds(pauseDuration);

        // Alpha ���� maxAlpha���� 0���� ����
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

    // �κ� ������ �̵��ϴ� �޼��� �׾��� �� �ߴ� �г� �ϴ��� ��ư
    public void GoToLobbyScene()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);

        SaveManager.Instance.isBossStage = false;
        SaveManager.Instance.isEliteStage = false;
        DataManager.Instance.currentCrystal += DataManager.Instance.DefeatTotalCrystal;
        SaveManager.Instance.accessDungeon = false;
        SceneFader.instance.LoadSceneWithFade(1); // �κ� ���� ���� �ε����� ����Ͽ� �ε�
    }
}
