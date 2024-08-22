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
    public TMP_Text dungeonDeckCardCountText; // ������� ���� �� ī�� ������ ǥ���� �ؽ�Ʈ UI
    public Canvas handCanvas; // HandCanvas ����

    private List<Transform> cards = new List<Transform>();

    // ī�� �߰� �� ȣ���� �޼���
    public void AddCard(Transform card)
    {
        cards.Add(card);
        card.SetParent(handCanvas.transform, false); // HandCanvas�� �ڽ����� ����
        StartCoroutine(UpdateHandLayoutCoroutine(2f, GameManager.instance.skip));
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }

    // ī�� ���� �� ȣ���� �޼���
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);
        StartCoroutine(UpdateHandLayoutCoroutine(2f, GameManager.instance.skip));
        UpdateCardCountText();
        UpdatUsedCardCountText();
    }

    private IEnumerator UpdateHandLayoutCoroutine(float delay = 1f, bool skip = true)
    {
        // ���� ��ġ�� ������ �ʾ����� ǥ��
        setCardEnd = false;

        // ���п� �ִ� ī���� ���� ������
        int numCards = cards.Count;

        // ī�尡 1���� ���� ��ġ ����
        if (numCards == 1)
        {
            MoveAndSetupCard(cards[0], 0.5f, 0f, 0f, 0);
            if (!skip) yield return new WaitForSeconds(delay); // 1�� ���
            setCardEnd = true; // ���� ��ġ�� �������� ǥ��
            yield break; // �޼��� ����
        }

        // ī�尡 2���� ���� ��ġ ����
        if (numCards == 2)
        {
            MoveAndSetupCard(cards[0], 0.4f, 3f, 0.5f, 0, 0.1f);
            MoveAndSetupCard(cards[1], 0.6f, -3f, 0.5f, 1, 0.1f);
            if (!skip) yield return new WaitForSeconds(delay); // 1�� ���
            setCardEnd = true; // ���� ��ġ�� �������� ǥ��
            yield break; // �޼��� ����
        }

        // ī�尡 3���� ���� ��ġ ����
        if (numCards == 3)
        {
            MoveAndSetupCard(cards[0], 0.27f, 9f, 0.75f, 0, 0.2f);
            MoveAndSetupCard(cards[1], 0.5f, 0f, 0f, 1);
            MoveAndSetupCard(cards[2], 0.73f, -9f, 0.75f, 2, 0.2f);
            if (!skip) yield return new WaitForSeconds(delay); // 1�� ���
            setCardEnd = true; // ���� ��ġ�� �������� ǥ��
            yield break; // �޼��� ����
        }

        // ī�尡 4���� ���� ��ġ ����
        if (numCards == 4)
        {
            MoveAndSetupCard(cards[0], 0.15f, 8.5f, 1f, 0, 0.2f);
            MoveAndSetupCard(cards[1], 0.38f, 4f, 0.5f, 1, 0.1f);
            MoveAndSetupCard(cards[2], 0.62f, -4f, 0.5f, 2, 0.1f);
            MoveAndSetupCard(cards[3], 0.85f, -8.5f, 1f, 3, 0.2f);
            if (!skip) yield return new WaitForSeconds(delay); // 1�� ���
            setCardEnd = true; // ���� ��ġ�� �������� ǥ��
            yield break; // �޼��� ����
        }

        // ī�尡 5�� �̻��� ���� ��ġ ����
        for (int i = 0; i < numCards; i++)
        {
            float t = (float)i / (numCards - 1); // 0���� 1 ������ ���� ������ t ��� (ī�� ��ġ ����)
            float angle = Mathf.Lerp(maxAngle, -maxAngle, t); // �ּ� �������� �ִ� �������� �����Ͽ� ���� ���
            PRS prs = CalculatePRS(t, angle, Mathf.Abs(t - 0.5f) * 1.5f); // ī���� ��ġ�� ȸ�� �� ������ ���
            if (Mathf.Abs(t - 0.5f) < 0.01f) // �߾� ī�� Ȯ�� (t�� 0.5�� ����� ���)
            {
                prs.pos.y -= 0.15f; // �߾� ī���� y���� ����
            }
            MoveAndSetupCard(cards[i], prs, i);
        }

        // 1�� ��� �� ���� ��ġ�� �������� ǥ��
        if (!skip) yield return new WaitForSeconds(delay);
        setCardEnd = true;
    }


    // ī�带 �̵��ϰ� �����ϴ� �޼���
    private void MoveAndSetupCard(Transform card, float t, float angle, float verticalOffsetFactor, int order, float yOffset = 0f)
    {
        PRS prs = CalculatePRS(t, angle, verticalOffsetFactor); // ī���� ��ġ�� ȸ�� �� ������ ���
        prs.pos.y += yOffset; // y���� �ø�
        MoveAndSetupCard(card, prs, order); // ī�� �̵� �� ����
    }

    // �����ε�� ī�带 �̵��ϰ� �����ϴ� �޼���
    private void MoveAndSetupCard(Transform card, PRS prs, int order)
    {
        StartCoroutine(MoveCard(card, prs.pos, prs.rot, moveDuration)); // ī�带 ��ǥ ��ġ�� �̵�
        SetCardOrderInLayer(card, order); // ī���� ���̾� ������ ����
        card.GetComponent<CardDrag>().SetOriginalPosition(prs.pos, prs.rot); // ī�� �巡�� ������Ʈ�� ���� ��ġ ����
        card.GetComponent<CardZoom>().SetOriginalPosition(prs.pos, prs.rot); // ī�� Ȯ��/��� ������Ʈ�� ���� ��ġ ����
    }

    // ī�� �̵� �ڷ�ƾ
    private IEnumerator MoveCard(Transform card, Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 initialPosition = card.position;
        Quaternion initialRotation = card.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (card == null) yield break;  // ī�尡 �ı��Ǿ����� Ȯ��

            card.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            card.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (card != null)  // ī�尡 �ı����� �ʾ��� �� ��ġ ����
        {
            card.position = targetPosition;
            card.rotation = targetRotation;
        }
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
            // ī�� ������Ʈ�� ��Ȱ��ȭ
            card.gameObject.SetActive(false);
        }
    }

    public void ShowAllCardsActive()
    {
        foreach (Transform card in cards)
        {
            // ī�� ������Ʈ�� Ȱ��ȭ
            card.gameObject.SetActive(true);
        }
    }
}
