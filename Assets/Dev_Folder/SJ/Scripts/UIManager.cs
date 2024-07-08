using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject rewardCardsPrefab;
    public Transform CardSelectPanelCanvas;
    public GameObject cardSelectPanel;

    private void Start()
    {
        cardSelectPanel.SetActive(false);
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

    // 카드를 고르고 나면 cardSelectPanel을 꺼준다.
    // 카드 고르기 버튼도 꺼준다. (GameManager에 있음)
}
