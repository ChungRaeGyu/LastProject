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
    public bool setCardEnd { get; private set; } // �� ���� ��ġ�ϷḦ Ȯ��

    public TMP_Text cardCountText; // ���� ���� ī�� ���� ǥ���� �ؽ�Ʈ UI
    public TMP_Text usedCardCountText; // ���� ī�� ������ ǥ���� �ؽ�Ʈ UI
    public Canvas handCanvas; // HandCanvas ����

    private List<Transform> cards = new List<Transform>();

    // ī�� �߰� �� ȣ���� �޼���
    public void AddCard(Transform card)
    {
        cards.Add(card);
        card.SetParent(handCanvas.transform, false); // HandCanvas�� �ڽ����� ����
        UpdateHandLayout();
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }

    // ī�� ���� �� ȣ���� �޼���
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);
        UpdateHandLayout();
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }

    // ���� ��ġ ������Ʈ
    private void UpdateHandLayout()
    {
        setCardEnd = false;

        int numCards = cards.Count;

        if (numCards == 1)
        {
            Transform card = cards[0];
            PRS prs = CalculatePRS(0.5f, 0, 0);
            StartCoroutine(MoveCard(card, prs.pos, prs.rot, moveDuration));
            SetCardOrderInLayer(card, 0);
            card.GetComponent<CardDrag>().SetOriginalPosition(prs.pos, prs.rot);
            card.GetComponent<CardZoom>().SetOriginalPosition(prs.pos, prs.rot);
            setCardEnd = true;
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

        setCardEnd = true;
    }

    // ī�� �̵� �ڷ�ƾ
    private IEnumerator MoveCard(Transform card, Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 initialPosition = card.position;
        Quaternion initialRotation = card.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (card == null) // ī�尡 �ı��Ǿ����� Ȯ��
            {
                yield break; // �ڷ�ƾ ����
            }
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
        TextMeshPro[] labels = card.GetComponentsInChildren<TextMeshPro>();

        // SpriteRenderer�� sortingOrder ����
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }

        // �� TextMeshPro�� MeshRenderer sortingOrder ����
        foreach (TextMeshPro label in labels)
        {
            MeshRenderer meshRenderer = label.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.sortingOrder = order;
            }
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

    // ���� ī�� �� �ؽ�Ʈ ������Ʈ
    private void UpdatUsedCardCountText()
    {
        if (usedCardCountText != null && DataManager.Instance != null)
        {
            usedCardCountText.text = DataManager.Instance.usedCards.Count.ToString();
        }
    }

    public void MoveUnusedCardsToUsed()
    {
        List<Transform> cardsToMove = new List<Transform>(cards); // ����Ʈ ����
        foreach (Transform card in cardsToMove)
        {
            CardBasic cardBasic = card.GetComponent<CardBasic>();
            if (cardBasic != null)
            {
                DataManager.Instance.AddUsedCard(cardBasic.cardBasic);
                RemoveCard(card);
                Destroy(card.gameObject); // ī�带 ������ �� ���� ������Ʈ�� �ı�
            }
        }

        UpdateHandLayout();
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }
}
