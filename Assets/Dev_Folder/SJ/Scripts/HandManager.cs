using UnityEngine;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    public Transform leftPosition; // 손패의 가장 왼쪽 위치 오브젝트
    public Transform rightPosition; // 손패의 가장 오른쪽 위치 오브젝트
    public float cardSpacing = 0.5f; // 카드 간의 간격
    public float maxAngle = 15; // 최대 회전 각도
    public float maxVerticalOffset = 0.5f; // 최대 수직 오프셋

    private List<Transform> cards = new List<Transform>();

    // 카드 추가 시 호출할 메서드
    public void AddCard(Transform card)
    {
        cards.Add(card);
        UpdateHandLayout();
    }

    // 카드 제거 시 호출할 메서드
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);
        UpdateHandLayout();
    }

    // 손패 배치 업데이트
    private void UpdateHandLayout()
    {
        int numCards = cards.Count;
        float centerT = 0.5f; // 중간 지점

        // 카드가 1개일 때
        if (numCards == 1)
        {
            Transform card = cards[0];
            Vector3 centerPos = Vector3.Lerp(leftPosition.position, rightPosition.position, centerT); // 가운데 위치 계산
            card.position = centerPos;
            card.rotation = Quaternion.Euler(0f, 0f, 0f); // 각도는 0으로 설정
            return; // 함수 종료
        }

        // 카드가 2개일 때
        if (numCards == 2)
        {
            Transform card1 = cards[0];
            Transform card2 = cards[1];

            Vector3 centerPos = Vector3.Lerp(leftPosition.position, rightPosition.position, centerT); // 가운데 위치 계산

            // 적절한 간격으로 배치
            Vector3 offset = (rightPosition.position - leftPosition.position).normalized * cardSpacing;

            card1.position = centerPos - offset / 2;
            card1.rotation = Quaternion.Euler(0f, 0f, maxAngle / 2);

            card2.position = centerPos + offset / 2;
            card2.rotation = Quaternion.Euler(0f, 0f, -maxAngle / 2);

            return; // 함수 종료
        }

        // 카드가 3개일 때
        if (numCards == 3)
        {
            Transform card1 = cards[0];
            Transform card2 = cards[1];
            Transform card3 = cards[2];

            Vector3 centerPos = Vector3.Lerp(leftPosition.position, rightPosition.position, centerT); // 가운데 위치 계산

            // 적절한 간격으로 배치
            Vector3 offset = (rightPosition.position - leftPosition.position).normalized * cardSpacing;

            card1.position = centerPos - offset;
            card1.rotation = Quaternion.Euler(0f, 0f, maxAngle);

            card2.position = centerPos;
            card2.rotation = Quaternion.Euler(0f, 0f, 0f);

            card3.position = centerPos + offset;
            card3.rotation = Quaternion.Euler(0f, 0f, -maxAngle);

            return; // 함수 종료
        }

        // 카드가 4개 이상일 때
        float totalWidth = Vector3.Distance(leftPosition.position, rightPosition.position);
        float spacing = totalWidth / (Mathf.Max(numCards - 1, 1)); // 카드 간의 간격 계산

        for (int i = 0; i < numCards; i++)
        {
            Transform card = cards[i];
            float t = (float)i / Mathf.Max(numCards - 1, 1); // 0에서 1 사이의 값을 가지는 t 계산
            float angle = Mathf.Lerp(maxAngle, -maxAngle, t); // 최소 각도에서 최대 각도까지 보간

            // 카드의 위치 계산 (오른쪽에서 추가)
            Vector3 rightPos = rightPosition.position;
            Vector3 newPos = rightPos - (rightPos - leftPosition.position).normalized * spacing * (numCards - 1 - i);

            // 수직 위치 보정
            float verticalOffset = Mathf.Lerp(0, -maxVerticalOffset, Mathf.Abs(t - 0.5f) * 1.5f); // 중간에 있는 카드일수록 높이가 낮아지도록 설정
            newPos.y += verticalOffset;

            // 카드의 각도와 위치 설정
            card.position = newPos;
            card.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
