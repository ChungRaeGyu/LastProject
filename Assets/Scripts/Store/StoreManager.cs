using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    public List<GameObject> CardParents; // 카드 부모 오브젝트를 담을 리스트
    public List<GameObject> Cards; // 생성할 카드를 담을 리스트
    public GameObject storePanel; // 상점 패널
    public RectTransform backButton;
    public RectTransform lobbyButton;

    public int needCoinAmount;
    public int healthprice;

    [Header("TMP_Text")]
    public TMP_Text dungeonCoin; // 화면에 보여줄 던전코인의 개수
    public TMP_Text cardRemovePriceText; // 카드를 제거하는데에 드는 비용을 나타내는 TMP_Text
    public TMP_Text healthItemPriceText; // 카드를 제거하는데에 드는 비용을 나타내는 TMP_Text

    private Vector2 backButtonOriginalPos;
    private Vector2 lobbyButtonOriginalPos;
    private Vector2 backButtonTargetPos = new Vector2(0, 100);
    private Vector2 lobbyButtonTargetPos = new Vector2(550, 100);

    [Header("Manager")]
    public RemoveListManager removeListManager;
    public CardListManager cardListManager;

    [Header("GameObject")]
    public GameObject deckPanel;

    [Header("DeletePanel")]
    public GameObject removePanel;

    [Header("AudioClip")]
    public AudioClip StoreUseCoinClip;

    [SerializeField]
    List<GameObject> availableCards;

    public TMP_Text dungeonHpText;

    // 카드 제거를 이번 상점에서 구매했는지 확인
    private bool purchased;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        deckPanel.SetActive(false);

        dungeonHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";

        needCoinAmount = 65 + (DataManager.Instance.removeCardCount * 25);
        cardRemovePriceText.text = needCoinAmount.ToString();

        healthItemPriceText.text = healthprice.ToString();

        backButtonOriginalPos = backButton.anchoredPosition;
        lobbyButtonOriginalPos = lobbyButton.anchoredPosition;

        UpdateCoin();

        if (CardParents.Count == 0 || Cards.Count == 0)
        {
            Debug.LogError("카드 부모 오브젝트 또는 카드 리스트가 비어 있습니다.");
            return;
        }

        if (Cards.Count < CardParents.Count)
        {
            Debug.LogError("카드 부모 오브젝트의 수보다 카드의 수가 적습니다.");
            return;
        }

        availableCards = new List<GameObject>(Cards);

        // 무작위로 카드를 선택하여 CardParents의 자식으로 생성
        for (int i = 0; i < CardParents.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableCards.Count);
            GameObject selectedCard = Instantiate(availableCards[randomIndex], CardParents[i].transform);
            CardBasic tempcard = availableCards[randomIndex].GetComponent<CardBasic>();
            SetCardPrice(tempcard, CardParents[i]);

            // 선택된 카드를 리스트에서 제거 (중복 방지용)
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
                price = UnityEngine.Random.Range(35, 51);
                break;
            case Rate.Rarity:
                price = UnityEngine.Random.Range(65, 81);
                break;
            case Rate.Hero:
                price = UnityEngine.Random.Range(110, 131);
                break;
            case Rate.Legend:
                price = UnityEngine.Random.Range(150, 181);
                break;
        }

        TMP_Text priceText = parent.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
        if (priceText != null)
        {
            priceText.text = $"{price}";

            // 카드 부모 오브젝트에 버튼 추가 및 클릭 이벤트 연결
            Button button = parent.GetComponent<Button>();
            if (button == null)
            {
                button = parent.AddComponent<Button>();
            }

            button.onClick.AddListener(() => OnCardClicked(cardBasic, price, parent));
        }
        else
        {
            Debug.LogError("가격을 표시할 TMP_Text 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void OnCardClicked(CardBasic cardBasic, int price, GameObject parent)
    {

        if (DataManager.Instance.currentCoin >= price)
        {
            SettingManager.Instance.PlaySound(StoreUseCoinClip);

            // 동전 소리
            DataManager.Instance.currentCoin -= price;
            DataManager.Instance.AddCard(cardBasic); // 카드 추가

            // 구매한 카드 부모 오브젝트의 자식 카드들만 제거
            RemoveChildren(parent);

            UpdateCoin();
        }
        else
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

            Debug.LogError("코인이 부족합니다!");
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
        float duration = 0.2f; // 이동 시간
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            backButton.anchoredPosition = Vector2.Lerp(fromBackButton, toBackButton, t);
            lobbyButton.anchoredPosition = Vector2.Lerp(fromLobbyButton, toLobbyButton, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 설정
        backButton.anchoredPosition = toBackButton;
        lobbyButton.anchoredPosition = toLobbyButton;
    }

    public void OnLobbyButtonClick()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.CardPassClip);

        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);
        SceneFader.instance.LoadSceneWithFade(2);
    }

    public void ShowDeleteList()
    {
        if (DataManager.Instance.currentCoin >= needCoinAmount && !purchased)
        {
            purchased = true; // 한 번만 구매가 가능하게 함

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

            Debug.LogError("코인이 부족합니다!");
        }
    }

    // 코인을 지불하고 체력을 회복시키는 메서드
    public void HealPlayer()
    {
        if (DataManager.Instance.currentCoin >= healthprice)
        {
            DataManager.Instance.currentCoin -= healthprice;
            dungeonCoin.text = DataManager.Instance.currentCoin.ToString();
            DataManager.Instance.currenthealth = DataManager.Instance.maxHealth;
            dungeonHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";
            SettingManager.Instance.PlaySound(StoreUseCoinClip);
            UpdateCoin();
        }
        else
        {
            SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

            Debug.LogError("코인이 부족합니다!");
        }
    }

    // ScrollView의 활성화/비활성화 공통 메서드
    private void ToggleScrollView(GameObject scrollView, Action updateList)
    {
        if (scrollView != null)
        {
            if (scrollView.activeSelf)
            {
                // 비활성화
                SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);
            }
            else
            {
                // 활성화
                updateList?.Invoke();
                SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardPassClip);
            }

            scrollView.SetActive(!scrollView.activeSelf);
        }
    }

    // unUsedScrollView 활성화/비활성화 메서드
    public void ToggleDungeonDeckScrollView()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);
        ToggleScrollView(
            deckPanel,
            cardListManager.UpdateDungeonDeckList
        );
    }
}
