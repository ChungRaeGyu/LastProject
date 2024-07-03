using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Transform leftPosition; // 손패의 가장 왼쪽 위치 오브젝트
    public Transform rightPosition; // 손패의 가장 오른쪽 위치 오브젝트
    public float cardSpacing = 0.5f; // 카드 간의 간격
    public float maxAngle = 10f; // 최대 회전 각도
    public float maxVerticalOffset = 0.5f; // 최대 수직 오프셋
    public float moveDuration = 0.5f; // 카드 이동 지속 시간

    public TMP_Text cardCountText; // 덱의 남은 카드 수를 표시할 텍스트 UI

    private List<Transform> cards = new List<Transform>();

    // 카드 추가 시 호출할 메서드
    public void AddCard(Transform card)
    {
        cards.Add(card);
        UpdateHandLayout();
        UpdateCardCountText();
    }

    // 카드 제거 시 호출할 메서드
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);
        UpdateHandLayout();
        UpdateCardCountText();
    }

    // 손패 배치 업데이트
    private void UpdateHandLayout()
    {
        int numCards = cards.Count;

        if (numCards == 1)
        {
            Transform card = cards[0];
            PRS prs = CalculatePRS(0.5f, 0, 0);
            StartCoroutine(MoveCard(card, prs.pos, prs.rot, moveDuration));
            SetCardOrderInLayer(card, 0);
            card.GetComponent<CardDrag>().SetOriginalPosition(prs.pos, prs.rot);
            card.GetComponent<CardZoom>().SetOriginalPosition(prs.pos, prs.rot);
            return;
        }

        for (int i = 0; i < numCards; i++)
        {
            Transform card = cards[i];
            float t = (float)i / (numCards - 1); // 0에서 1 사이의 값을 가지는 t 계산
            float angle = Mathf.Lerp(maxAngle, -maxAngle, t); // 최소 각도에서 최대 각도까지 보간
            PRS prs = CalculatePRS(t, angle, Mathf.Abs(t - 0.5f) * 1.5f);
            StartCoroutine(MoveCard(card, prs.pos, prs.rot, moveDuration));
            SetCardOrderInLayer(card, i);
            card.GetComponent<CardDrag>().SetOriginalPosition(prs.pos, prs.rot);
            card.GetComponent<CardZoom>().SetOriginalPosition(prs.pos, prs.rot);
        }
    }

    // 카드 이동 코루틴
    private IEnumerator MoveCard(Transform card, Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 initialPosition = card.position;
        Quaternion initialRotation = card.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            card.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            card.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.position = targetPosition;
        card.rotation = targetRotation;
    }

    // PRS 계산하기
    private PRS CalculatePRS(float t, float angle, float verticalOffsetFactor)
    {
        Vector3 leftPos = leftPosition.position;
        Vector3 rightPos = rightPosition.position;
        Vector3 newPos = Vector3.Lerp(leftPos, rightPos, t);

        // 수직 위치 보정
        float verticalOffset = Mathf.Lerp(0, -maxVerticalOffset, verticalOffsetFactor);
        newPos.y += verticalOffset;

        Quaternion newRot = Quaternion.Euler(0f, 0f, angle);
        Vector3 newScale = Vector3.one;

        return new PRS(newPos, newRot, newScale);
    }

    // 카드의 Sprite Renderer의 Order in Layer 설정
    private void SetCardOrderInLayer(Transform card, int order)
    {
        SpriteRenderer spriteRenderer = card.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }

    // 덱에 남은 카드 수 텍스트 업데이트
    private void UpdateCardCountText()
    {
        if (cardCountText != null && DataManager.Instance != null)
        {
            cardCountText.text = DataManager.Instance.deck.Count.ToString();
        }
    }
}
