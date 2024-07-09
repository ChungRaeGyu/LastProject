using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region �̱���
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
    public Button lobbyButton; // �κ�� ���� ��ư
    public Button turnEndButton; // �� ���� ��ư
    public Button openCardSelectionButton; // ī�� ���� â ���� ��ư

    [Header("Reward")]
    public Image fadeRewardPanel; // ���� �г� ���� �� ��ο�����
    public GameObject rewardPanel; // ���� �г�

    [Header("UI")]
    public Canvas healthBarCanvas; // ĵ���� ����
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

        // �߾� ī�� ����
        GameObject centerCard = Instantiate(rewardCardsPrefab, CardSelectPanelCanvas);
        centerCard.transform.localPosition = Vector3.zero;

        // ���� ī�� ����
        GameObject leftCard = Instantiate(rewardCardsPrefab, CardSelectPanelCanvas);
        leftCard.transform.localPosition = Vector3.zero;
        StartCoroutine(MoveCard(leftCard, new Vector3(-400, 0, 0)));

        // ������ ī�� ����
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

    // ī�带 ���� ���� cardSelectPanel�� ���ش�.
    // ī�� ���� ��ư�� ���ش�. (GameManager�� ����)
}
