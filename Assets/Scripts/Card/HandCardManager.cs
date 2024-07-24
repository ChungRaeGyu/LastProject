using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandCardManager : MonoBehaviour
{
    public List<Transform> cardTransforms; // 손에 있는 카드들의 Transform 리스트

    public float cardInterval = 100f; // 카드 사이의 간격
    public float maxRotation = 10f; // 최대 회전 각도

    private void Start()
    {
        ArrangeHandCards();
    }

    private void ArrangeHandCards()
    {
        int cardCount = cardTransforms.Count;
        if (cardCount == 0) return;

        float totalWidth = cardInterval * (cardCount - 1); // 모든 카드를 포함한 총 너비 계산
        float startOffset = -totalWidth / 2f; // 첫 번째 카드의 시작 오프셋

        for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = cardTransforms[i];
            float xPosition = startOffset + i * cardInterval;
            float rotationZ = maxRotation * Mathf.Sin((float)i / (cardCount - 1) * Mathf.PI); // 카드의 회전 각도 계산

            // 카드의 위치와 회전을 설정
            cardTransform.localPosition = new Vector3(xPosition, 0f, 0f);
            cardTransform.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
        }
    }
}
