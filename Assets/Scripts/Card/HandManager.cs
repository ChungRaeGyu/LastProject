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
    public bool setCardEnd { get; private set; } // 손 패의 배치완료를 확인

    public TMP_Text cardCountText; // 덱의 남은 카드 수를 표시할 텍스트 UI
    public TMP_Text usedCardCountText; // 사용된 카드 개수를 표시할 텍스트 UI
    public TMP_Text dungeonDeckCardCountText; // 사용중인 덱의 총 카드 개수를 표시할 텍스트 UI
    public Canvas handCanvas; // HandCanvas 참조

    private List<Transform> cards = new List<Transform>();

    // 카드 추가 시 호출할 메서드
    public void AddCard(Transform card)
    {
        cards.Add(card);
        card.SetParent(handCanvas.transform, false); // HandCanvas의 자식으로 설정
        StartCoroutine(UpdateHandLayoutCoroutine(2f, GameManager.instance.skip));
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }

    // 카드 제거 시 호출할 메서드
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);
        StartCoroutine(UpdateHandLayoutCoroutine(2f, GameManager.instance.skip));
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }

    private IEnumerator UpdateHandLayoutCoroutine(float delay = 1f, bool skip = true)
    {
        // 손패 배치가 끝나지 않았음을 표시
        setCardEnd = false;

        // 손패에 있는 카드의 수를 가져옴
        int numCards = cards.Count;

        // 카드가 1장일 때의 배치 로직
        if (numCards == 1)
        {
            MoveAndSetupCard(cards[0], 0.5f, 0f, 0f, 0);
            if (!skip) yield return new WaitForSeconds(delay); // 1초 대기
            setCardEnd = true; // 손패 배치가 끝났음을 표시
            yield break; // 메서드 종료
        }

        // 카드가 2장일 때의 배치 로직
        if (numCards == 2)
        {
            MoveAndSetupCard(cards[0], 0.4f, 3f, 0.5f, 0, 0.1f);
            MoveAndSetupCard(cards[1], 0.6f, -3f, 0.5f, 1, 0.1f);
            if (!skip) yield return new WaitForSeconds(delay); // 1초 대기
            setCardEnd = true; // 손패 배치가 끝났음을 표시
            yield break; // 메서드 종료
        }

        // 카드가 3장일 때의 배치 로직
        if (numCards == 3)
        {
            MoveAndSetupCard(cards[0], 0.27f, 9f, 0.75f, 0, 0.2f);
            MoveAndSetupCard(cards[1], 0.5f, 0f, 0f, 1);
            MoveAndSetupCard(cards[2], 0.73f, -9f, 0.75f, 2, 0.2f);
            if (!skip) yield return new WaitForSeconds(delay); // 1초 대기
            setCardEnd = true; // 손패 배치가 끝났음을 표시
            yield break; // 메서드 종료
        }

        // 카드가 4장일 때의 배치 로직
        if (numCards == 4)
        {
            MoveAndSetupCard(cards[0], 0.15f, 8.5f, 1f, 0, 0.2f);
            MoveAndSetupCard(cards[1], 0.38f, 4f, 0.5f, 1, 0.1f);
            MoveAndSetupCard(cards[2], 0.62f, -4f, 0.5f, 2, 0.1f);
            MoveAndSetupCard(cards[3], 0.85f, -8.5f, 1f, 3, 0.2f);
            if (!skip) yield return new WaitForSeconds(delay); // 1초 대기
            setCardEnd = true; // 손패 배치가 끝났음을 표시
            yield break; // 메서드 종료
        }

        // 카드가 5장 이상일 때의 배치 로직
        for (int i = 0; i < numCards; i++)
        {
            float t = (float)i / (numCards - 1); // 0에서 1 사이의 값을 가지는 t 계산 (카드 위치 비율)
            float angle = Mathf.Lerp(maxAngle, -maxAngle, t); // 최소 각도에서 최대 각도까지 보간하여 각도 계산
            PRS prs = CalculatePRS(t, angle, Mathf.Abs(t - 0.5f) * 1.5f); // 카드의 위치와 회전 및 스케일 계산
            if (Mathf.Abs(t - 0.5f) < 0.01f) // 중앙 카드 확인 (t가 0.5에 가까운 경우)
            {
                prs.pos.y -= 0.15f; // 중앙 카드의 y값을 내림
            }
            MoveAndSetupCard(cards[i], prs, i);
        }

        // 1초 대기 후 손패 배치가 끝났음을 표시
        if (!skip) yield return new WaitForSeconds(delay);
        setCardEnd = true;
    }


    // 카드를 이동하고 설정하는 메서드
    private void MoveAndSetupCard(Transform card, float t, float angle, float verticalOffsetFactor, int order, float yOffset = 0f)
    {
        PRS prs = CalculatePRS(t, angle, verticalOffsetFactor); // 카드의 위치와 회전 및 스케일 계산
        prs.pos.y += yOffset; // y값을 올림
        MoveAndSetupCard(card, prs, order); // 카드 이동 및 설정
    }

    // 오버로드된 카드를 이동하고 설정하는 메서드
    private void MoveAndSetupCard(Transform card, PRS prs, int order)
    {
        StartCoroutine(MoveCard(card, prs.pos, prs.rot, moveDuration)); // 카드를 목표 위치로 이동
        SetCardOrderInLayer(card, order); // 카드의 레이어 순서를 설정
        card.GetComponent<CardDrag>().SetOriginalPosition(prs.pos, prs.rot); // 카드 드래그 컴포넌트에 원래 위치 설정
        card.GetComponent<CardZoom>().SetOriginalPosition(prs.pos, prs.rot); // 카드 확대/축소 컴포넌트에 원래 위치 설정
    }

    // 카드 이동 코루틴
    private IEnumerator MoveCard(Transform card, Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 initialPosition = card.position;
        Quaternion initialRotation = card.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (card == null) yield break;  // 카드가 파괴되었는지 확인

            card.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            card.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (card != null)  // 카드가 파괴되지 않았을 때 위치 설정
        {
            card.position = targetPosition;
            card.rotation = targetRotation;
        }
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
        TextMeshPro[] labels = card.GetComponentsInChildren<TextMeshPro>();

        // SpriteRenderer의 sortingOrder 설정
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }

        // 각 TextMeshPro의 MeshRenderer sortingOrder 설정
        foreach (TextMeshPro label in labels)
        {
            MeshRenderer meshRenderer = label.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.sortingOrder = order;
            }
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

    // 사용된 카드 수 텍스트 업데이트
    private void UpdatUsedCardCountText()
    {
        if (usedCardCountText != null && DataManager.Instance != null)
        {
            usedCardCountText.text = DataManager.Instance.usedCards.Count.ToString();
        }
    }

    public void MoveUnusedCardsToUsed()
    {
        List<Transform> cardsToMove = new List<Transform>(cards); // 리스트 복사
        foreach (Transform card in cardsToMove)
        {
            CardBasic cardBasic = card.GetComponent<CardBasic>();
            if (cardBasic != null)
            {
                DataManager.Instance.AddUsedCard(cardBasic.cardBasic);
                RemoveCard(card);
                Destroy(card.gameObject); // 카드를 제거할 때 게임 오브젝트도 파괴
            }
        }

        StartCoroutine(UpdateHandLayoutCoroutine(1f, GameManager.instance.skip));
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }

    public void HideAllCards()
    {
        foreach (Transform card in cards)
        {
            StartCoroutine(MoveCard(card, card.position + Vector3.down * 10f, card.rotation, moveDuration));
        }
    }

    public void HideAllCardsActive()
    {
        foreach (Transform card in cards)
        {
            // 카드 오브젝트를 비활성화
            card.gameObject.SetActive(false);
        }
    }

    public void ShowAllCardsActive()
    {
        foreach (Transform card in cards)
        {
            // 카드 오브젝트를 활성화
            card.gameObject.SetActive(true);
        }
    }
}
