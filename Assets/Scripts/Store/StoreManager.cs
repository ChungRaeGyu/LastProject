using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    public List<GameObject> CardParents; // ī�� �θ� ������Ʈ�� ���� ����Ʈ
    public List<GameObject> Cards; // ������ ī�带 ���� ����Ʈ
    public GameObject storePanel; // ���� �г�
    public RectTransform backButton;
    public RectTransform lobbyButton;

    public int needCoinAmount;
    public int healthprice;

    [Header("TMP_Text")]
    public TMP_Text dungeonCoin; // ȭ�鿡 ������ ���������� ����
    public TMP_Text cardRemovePriceText; // ī�带 �����ϴµ��� ��� ����� ��Ÿ���� TMP_Text
    public TMP_Text healthItemPriceText; // ī�带 �����ϴµ��� ��� ����� ��Ÿ���� TMP_Text

    private Vector2 backButtonOriginalPos;
    private Vector2 lobbyButtonOriginalPos;
    private Vector2 backButtonTargetPos = new Vector2(0, 100);
    private Vector2 lobbyButtonTargetPos = new Vector2(550, 100);

    [Header("Manager")]
    public RemoveListManager removeListManager;

    [Header("DeletePanel")]
    public GameObject removePanel;

    [Header("AudioClip")]
    public AudioClip StoreUseCoinClip;

    [SerializeField]
    List<GameObject> availableCards;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        needCoinAmount = 65 + (DataManager.Instance.removeCardCount * 25);
        cardRemovePriceText.text = needCoinAmount.ToString();

        healthItemPriceText.text = healthprice.ToString();

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

        availableCards = new List<GameObject>(Cards);

        // �������� ī�带 �����Ͽ� CardParents�� �ڽ����� ����
        for (int i = 0; i < CardParents.Count; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            GameObject selectedCard = Instantiate(availableCards[randomIndex], CardParents[i].transform);
            CardBasic tempcard = availableCards[randomIndex].GetComponent<CardBasic>();
            SetCardPrice(tempcard, CardParents[i]);

            // ���õ� ī�带 ����Ʈ���� ���� (�ߺ� ������)
            availableCards.RemoveAt(randomIndex);
        }
    }

    private void SetCardPrice(CardBasic cardBasic, GameObject parent)
    {
        if (cardBasic == null)
        {
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

        TMP_Text priceText = parent.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
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
            SettingManager.Instance.PlaySound(StoreUseCoinClip);

            // ���� �Ҹ�
            DataManager.Instance.currentCoin -= price;
            DataManager.Instance.AddCard(cardBasic); // ī�� �߰�

            // ������ ī�� �θ� ������Ʈ�� �ڽ� ī��鸸 ����
            RemoveChildren(parent);

            UpdateCoin();
        }
        else
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

            Debug.LogError("������ �����մϴ�!");
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
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        storePanel.SetActive(true);
        StartCoroutine(MoveButtonsCoroutine(backButton.anchoredPosition, backButtonTargetPos, lobbyButton.anchoredPosition, lobbyButtonTargetPos));
    }

    public void CloseStorePanel()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip2);

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
        SettingManager.Instance.PlaySound(SettingManager.Instance.CardPassClip);

        DataManager.Instance.stageClearCount++;

        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);
        SceneManager.LoadScene(2);
    }

    public void ShowDeleteList()
    {
        if (DataManager.Instance.currentCoin >= needCoinAmount)
        {
            DataManager.Instance.currentCoin -= needCoinAmount;
            dungeonCoin.text = DataManager.Instance.currentCoin.ToString();
            DataManager.Instance.removeCardCount++;
            SettingManager.Instance.PlaySound(StoreUseCoinClip);
            removePanel.SetActive(true);
            removeListManager.UpdateDeckList();
        }
        else
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

            Debug.LogError("������ �����մϴ�!");
        }
    }

    // ������ �����ϰ� ü���� ȸ����Ű�� �޼���
    public void HealPlayer()
    {
        if (DataManager.Instance.currentCoin >= healthprice)
        {
            DataManager.Instance.currentCoin -= healthprice;
            dungeonCoin.text = DataManager.Instance.currentCoin.ToString();
            DataManager.Instance.currenthealth = DataManager.Instance.maxHealth;
            SettingManager.Instance.PlaySound(StoreUseCoinClip);
            UpdateCoin();
        }
        else
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

            Debug.LogError("������ �����մϴ�!");
        }
    }
}
