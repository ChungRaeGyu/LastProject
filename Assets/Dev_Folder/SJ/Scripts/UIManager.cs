using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 싱글톤
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
    #endregion

    public GameObject rewardCardsPrefab;
    public Transform CardSelectPanelCanvas;
    public GameObject cardSelectPanel;

    [Header("BUTTON")]
    public Button lobbyButton; // 로비로 가는 버튼
    public Button turnEndButton; // 턴 종료 버튼
    public Button openCardSelectionButton; // 카드 선택 창 열기 버튼

    [Header("Reward")]
    public Image fadeRewardPanel; // 보상 패널 열릴 때 어두워지게
    public GameObject rewardPanel; // 보상 패널

    [Header("UI")]
    public Canvas healthBarCanvas; // 캔버스 참조
    public TMP_Text costText;
    public TMP_Text TurnText;

    private void Start()
    {
        cardSelectPanel.SetActive(false);
        UIClear(false, true, false, false, false);
    }

    public void SpawnRewardCards()
    {
        cardSelectPanel.SetActive(true);

        // 중앙 카드 생성
        GameObject centerCard = Instantiate(rewardCardsPrefab, CardSelectPanelCanvas);
        centerCard.transform.localPosition = Vector3.zero;

        // 왼쪽 카드 생성
        GameObject leftCard = Instantiate(rewardCardsPrefab, CardSelectPanelCanvas);
        leftCard.transform.localPosition = Vector3.zero;
        StartCoroutine(MoveCard(leftCard, new Vector3(-400, 0, 0)));

        // 오른쪽 카드 생성
        GameObject rightCard = Instantiate(rewardCardsPrefab, CardSelectPanelCanvas);
        rightCard.transform.localPosition = Vector3.zero;
        StartCoroutine(MoveCard(rightCard, new Vector3(400, 0, 0)));
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

    // 카드를 고르고 나면 cardSelectPanel을 꺼준다.
    // 카드 고르기 버튼도 꺼준다. (GameManager에 있음)
}
