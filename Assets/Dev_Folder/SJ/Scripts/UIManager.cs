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
    public Transform removeCardSpawnPoint; // ���ŵ� ī�带 ������ ��ġ��
    public TMP_Text monstersKilledText; // óġ�� ���� ��

    // ���� UI ��ҵ��� �ʱ� ��ġ�� ������ ������
    private Vector2 originalCostImagePosition;
    private Vector2 originalTurnEndButtonPosition;
    private Vector2 originalUnUsedCardsPosition;
    private Vector2 originalUsedCardsPosition;

    // �ӽ÷� ī�带 ������ ������
    private GameObject centerCard;
    private GameObject leftCard;
    private GameObject rightCard;

    private void Start()
    {
        // �ʱ� ��ġ ����
        originalCostImagePosition = costImage.rectTransform.anchoredPosition;
        originalTurnEndButtonPosition = turnEndButton.GetComponent<RectTransform>().anchoredPosition;
        originalUnUsedCardsPosition = UnUsedCards.rectTransform.anchoredPosition;
        originalUsedCardsPosition = UsedCards.rectTransform.anchoredPosition;

        cardSelectPanel.SetActive(false);
        UIClear(false, true, false, false, false);

        MoveUIElementsToStartPositions();

        UpdatePlayerTurnCount(1); // �ʱ⿡ 1��° �� ����
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

        // �߾� ī�� ����
        centerCard = Instantiate(rewardCardPrefabs[chosenIndexes[0]], CardSelectPanelCanvas);
        SetCardScale(centerCard);
        centerCard.transform.localPosition = Vector3.zero;
        AddClickEvent(centerCard, chosenIndexes[0]);
        Destroy(centerCard.transform.GetChild(0).gameObject);

        // ���� ī�� ����
        leftCard = Instantiate(rewardCardPrefabs[chosenIndexes[1]], CardSelectPanelCanvas);
        SetCardScale(leftCard);
        leftCard.transform.localPosition = Vector3.zero;
        AddClickEvent(leftCard, chosenIndexes[1]);
        StartCoroutine(MoveCard(leftCard, new Vector3(-400, 0, 0)));
        Destroy(leftCard.transform.GetChild(0).gameObject);


        // ������ ī�� ����
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
        Debug.Log($"Ŭ���� ī���� �ε��� {cardIndex}");

        // Ŭ���� ���� ī�带 DataManager�� �߰�
        if (DataManager.Instance != null)
        {
            // Ŭ���� ī�带 �߰�
            CardBasic cardToAdd = rewardCardPrefabs[cardIndex].GetComponent<CardBasic>();
            DataManager.Instance.deckList.Add(cardToAdd);

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
            // Ȯ�������� Ȱ��ȭ
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

    // �й� �� ȣ��� �޼���
    public void ShowDefeatPanel()
    {
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);

            // óġ�� ���� ���� ǥ���ϴ� �κ�
            if (monstersKilledText != null)
            {
                monstersKilledText.text = $"óġ�� ���� ��: {DataManager.Instance.monstersKilledCount}";
            }
        }

        if (fadeRewardPanel != null)
        {
            fadeRewardPanel.gameObject.SetActive(true);
        }
    }

    public void ApplyDeathPenalty()
    {
        // �� ����Ʈ���� �������� ī�带 �����մϴ�.
        if (DataManager.Instance.deckList.Count > 0)
        {
            int randomIndex = Random.Range(0, DataManager.Instance.deckList.Count);
            CardBasic removedCard = DataManager.Instance.deckList[randomIndex];
            DataManager.Instance.deckList.RemoveAt(randomIndex);

            // ���ŵ� ī�带 ȭ�鿡 �����ְ� ������� ȿ���� �����մϴ�.
            ShowRemovedCard(removedCard);
        }
        else
        {
            Debug.LogWarning("���� ������ ī�尡 �����ϴ�.");
        }
    }

    private void ShowRemovedCard(CardBasic cardToRemove)
    {
        // ���ŵ� ī�带 �����ϰ� �θ� removeCardSpawnPoint�� �����մϴ�.
        GameObject removedCardObj = Instantiate(cardToRemove.gameObject, removeCardSpawnPoint);
        removedCardObj.SetActive(true);

        // ������ ����
        removedCardObj.transform.localScale = new Vector3(4f, 6f, 1f); // 2�� ũ��� ����

        // �ڽ� ������Ʈ�� Image ������Ʈ ��������
        Image cardImage = removedCardObj.GetComponentInChildren<Image>();
        if (cardImage != null)
        {
            // ī���� ���İ��� �����ϱ� ���� �ڷ�ƾ ȣ��
            StartCoroutine(FadeOutAndDestroy(cardImage));
        }
        else
        {
            Debug.LogWarning("�ڽ� ������Ʈ���� Image ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }


    private IEnumerator FadeOutAndDestroy(Image cardImage)
    {
        float fadeDuration = 2.0f;
        float fadeTimer = 0.0f;

        Color originalColor = cardImage.color;

        yield return new WaitForSeconds(1.0f); // 1�� ����

        // ���� ���������� ȿ��
        while (fadeTimer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
            Color newColor = originalColor;
            newColor.a = alpha;
            cardImage.color = newColor;

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // ī�尡 ������ �������� �� ����
        Destroy(cardImage.gameObject);
    }

    public void UpdatePlayerTurnCount(int turnNumber)
    {
        StartCoroutine(AnimateTurnCount(PlayerTurnCountText, $"�÷��̾� {turnNumber}��° ��"));
    }

    public void UpdateMonsterTurnCount(int turnNumber)
    {
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

    // �κ� ������ �̵��ϴ� �޼���
    public void GoToLobbyScene()
    {
        SceneManager.LoadScene(1); // �κ� ���� ���� �ε����� ����Ͽ� �ε�
    }
}
