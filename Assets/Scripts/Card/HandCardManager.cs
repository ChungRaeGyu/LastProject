using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandCardManager : MonoBehaviour
{
    public List<Transform> cardTransforms; // �տ� �ִ� ī����� Transform ����Ʈ

    public float cardInterval = 100f; // ī�� ������ ����
    public float maxRotation = 10f; // �ִ� ȸ�� ����

    private void Start()
    {
        ArrangeHandCards();
    }

    private void ArrangeHandCards()
    {
        int cardCount = cardTransforms.Count;
        if (cardCount == 0) return;

        float totalWidth = cardInterval * (cardCount - 1); // ��� ī�带 ������ �� �ʺ� ���
        float startOffset = -totalWidth / 2f; // ù ��° ī���� ���� ������

        for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = cardTransforms[i];
            float xPosition = startOffset + i * cardInterval;
            float rotationZ = maxRotation * Mathf.Sin((float)i / (cardCount - 1) * Mathf.PI); // ī���� ȸ�� ���� ���

            // ī���� ��ġ�� ȸ���� ����
            cardTransform.localPosition = new Vector3(xPosition, 0f, 0f);
            cardTransform.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
        }
    }
}
