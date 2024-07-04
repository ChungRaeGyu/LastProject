using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Transform leftPosition; // ������ ���� ���� ��ġ ������Ʈ
    public Transform rightPosition; // ������ ���� ������ ��ġ ������Ʈ
    public float cardSpacing = 0.5f; // ī�� ���� ����
    public float maxAngle = 10f; // �ִ� ȸ�� ����
    public float maxVerticalOffset = 0.5f; // �ִ� ���� ������
    public float moveDuration = 0.5f; // ī�� �̵� ���� �ð�

    public TMP_Text cardCountText; // ���� ���� ī�� ���� ǥ���� �ؽ�Ʈ UI

    private List<Transform> cards = new List<Transform>();

    // ī�� �߰� �� ȣ���� �޼���
    public void AddCard(Transform card)
    {
        cards.Add(card);
        UpdateHandLayout();
        UpdateCardCountText();
    }

    // ī�� ���� �� ȣ���� �޼���
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);
        UpdateHandLayout();
        UpdateCardCountText();
    }

    // ���� ��ġ ������Ʈ
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
            float t = (float)i / (numCards - 1); // 0���� 1 ������ ���� ������ t ���
            float angle = Mathf.Lerp(maxAngle, -maxAngle, t); // �ּ� �������� �ִ� �������� ����
            PRS prs = CalculatePRS(t, angle, Mathf.Abs(t - 0.5f) * 1.5f);
            StartCoroutine(MoveCard(card, prs.pos, prs.rot, moveDuration));
            SetCardOrderInLayer(card, i);
            card.GetComponent<CardDrag>().SetOriginalPosition(prs.pos, prs.rot);
            card.GetComponent<CardZoom>().SetOriginalPosition(prs.pos, prs.rot);
        }
    }

    // ī�� �̵� �ڷ�ƾ
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

    // PRS ����ϱ�
    private PRS CalculatePRS(float t, float angle, float verticalOffsetFactor)
    {
        Vector3 leftPos = leftPosition.position;
        Vector3 rightPos = rightPosition.position;
        Vector3 newPos = Vector3.Lerp(leftPos, rightPos, t);

        // ���� ��ġ ����
        float verticalOffset = Mathf.Lerp(0, -maxVerticalOffset, verticalOffsetFactor);
        newPos.y += verticalOffset;

        Quaternion newRot = Quaternion.Euler(0f, 0f, angle);
        Vector3 newScale = Vector3.one;

        return new PRS(newPos, newRot, newScale);
    }

    // ī���� Sprite Renderer�� Order in Layer ����
    private void SetCardOrderInLayer(Transform card, int order)
    {
        SpriteRenderer spriteRenderer = card.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }

    // ���� ���� ī�� �� �ؽ�Ʈ ������Ʈ
    private void UpdateCardCountText()
    {
        if (cardCountText != null && DataManager.Instance != null)
        {
            cardCountText.text = DataManager.Instance.deck.Count.ToString();
        }
    }
}
