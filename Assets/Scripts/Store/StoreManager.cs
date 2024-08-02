using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public List<GameObject> CardParents; // ī�� �θ� ������Ʈ�� ���� ����Ʈ
    public List<GameObject> Cards; // ������ ī�带 ���� ����Ʈ
    public TMP_Text dungeonCoin; // ȭ�鿡 ������ ���������� ����
    public GameObject storePanel; // ���� �г�
    public RectTransform backButton;
    public RectTransform lobbyButton;

    private Vector2 backButtonOriginalPos;
    private Vector2 lobbyButtonOriginalPos;
    private Vector2 backButtonTargetPos = new Vector2(0, 100);
    private Vector2 lobbyButtonTargetPos = new Vector2(550, 100);

    private void Start()
    {
        backButtonOriginalPos = backButton.anchoredPosition;
        lobbyButtonOriginalPos = lobbyButton.anchoredPosition;

        UpdateCoin();

        if (CardParents.Count == 0 || Cards.Count == 0)
        {
            Debug.LogError("ī�� �θ� ������Ʈ �Ǵ� ī�� ����Ʈ�� ��� �ֽ��ϴ�.");
            return;
        }

        if (Cards.Count < CardParents.Count)
        {
            Debug.LogError("ī�� �θ� ������Ʈ�� ������ ī���� ���� �����ϴ�.");
            return;
        }

        List<GameObject> availableCards = new List<GameObject>(Cards);

        // �������� ī�带 �����Ͽ� CardParents�� �ڽ����� ����
        for (int i = 0; i < CardParents.Count; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            GameObject selectedCard = Instantiate(availableCards[randomIndex], CardParents[i].transform);
            SetCardPrice(selectedCard, CardParents[i]);

            // ���õ� ī�带 ����Ʈ���� ���� (�ߺ� ������)
            availableCards.RemoveAt(randomIndex);
        }
    }

    private void SetCardPrice(GameObject card, GameObject parent)
    {
        CardBasic cardBasic = card.GetComponent<CardBasic>();
        if (cardBasic == null)
        {
            Debug.LogError("ī�忡 CardBasic ������Ʈ�� �����ϴ�.");
            return;
        }

        int price = 0;
        switch (cardBasic.rate)
        {
            case Rate.Normal:
                price = Random.Range(35, 51);
                break;
            case Rate.Rarity:
                price = Random.Range(65, 81);
                break;
            case Rate.Hero:
                price = Random.Range(110, 131);
                break;
            case Rate.Legend:
                price = Random.Range(150, 181);
                break;
        }

        TMP_Text priceText = parent.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
        if (priceText != null)
        {
            priceText.text = $"{price}";

            // ī�� �θ� ������Ʈ�� ��ư �߰� �� Ŭ�� �̺�Ʈ ����
            Button button = parent.GetComponent<Button>();
            if (button == null)
            {
                button = parent.AddComponent<Button>();
            }

            button.onClick.AddListener(() => OnCardClicked(cardBasic, price, parent));
        }
        else
        {
            Debug.LogError("������ ǥ���� TMP_Text ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void OnCardClicked(CardBasic cardBasic, int price, GameObject parent)
    {
        if (DataManager.Instance.currentCoin >= price)
        {
            // ���� �Ҹ�
            DataManager.Instance.currentCoin -= price;
            DataManager.Instance.AddCard(cardBasic); // ī�� �߰�

            // ������ ī�� �θ� ������Ʈ�� �ڽ� ī��鸸 ����
            RemoveChildren(parent);

            UpdateCoin();
        }
        else
        {
            Debug.LogWarning("������ �����մϴ�!");
        }
    }

    private void RemoveChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateCoin()
    {
        dungeonCoin.text = DataManager.Instance.currentCoin.ToString();
    }

    public void ShowStorePanel()
    {
        storePanel.SetActive(true);
        StartCoroutine(MoveButtonsCoroutine(backButton.anchoredPosition, backButtonTargetPos, lobbyButton.anchoredPosition, lobbyButtonTargetPos));
    }

    public void CloseStorePanel()
    {
        StartCoroutine(MoveButtonsCoroutine(backButton.anchoredPosition, backButtonOriginalPos, lobbyButton.anchoredPosition, lobbyButtonOriginalPos));
        storePanel.SetActive(false);
    }

    private IEnumerator MoveButtonsCoroutine(Vector2 fromBackButton, Vector2 toBackButton, Vector2 fromLobbyButton, Vector2 toLobbyButton)
    {
        float duration = 0.2f; // �̵� �ð�
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            backButton.anchoredPosition = Vector2.Lerp(fromBackButton, toBackButton, t);
            lobbyButton.anchoredPosition = Vector2.Lerp(fromLobbyButton, toLobbyButton, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ ����
        backButton.anchoredPosition = toBackButton;
        lobbyButton.anchoredPosition = toLobbyButton;
    }

    public void OnLobbyButtonClick()
    {
        DataManager.Instance.stageClearCount++;

        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);
        SceneManager.LoadScene(2);
    }
}
