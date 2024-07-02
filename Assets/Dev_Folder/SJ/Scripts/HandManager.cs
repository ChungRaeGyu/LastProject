using UnityEngine;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    public Transform leftPosition; // ������ ���� ���� ��ġ ������Ʈ
    public Transform rightPosition; // ������ ���� ������ ��ġ ������Ʈ
    public float cardSpacing = 0.5f; // ī�� ���� ����
    public float maxAngle = 15; // �ִ� ȸ�� ����
    public float maxVerticalOffset = 0.5f; // �ִ� ���� ������

    private List<Transform> cards = new List<Transform>();

    // ī�� �߰� �� ȣ���� �޼���
    public void AddCard(Transform card)
    {
        cards.Add(card);
        UpdateHandLayout();
    }

    // ī�� ���� �� ȣ���� �޼���
    public void RemoveCard(Transform card)
    {
        cards.Remove(card);
        UpdateHandLayout();
    }

    // ���� ��ġ ������Ʈ
    private void UpdateHandLayout()
    {
        int numCards = cards.Count;
        float centerT = 0.5f; // �߰� ����

        // ī�尡 1���� ��
        if (numCards == 1)
        {
            Transform card = cards[0];
            Vector3 centerPos = Vector3.Lerp(leftPosition.position, rightPosition.position, centerT); // ��� ��ġ ���
            card.position = centerPos;
            card.rotation = Quaternion.Euler(0f, 0f, 0f); // ������ 0���� ����
            return; // �Լ� ����
        }

        // ī�尡 2���� ��
        if (numCards == 2)
        {
            Transform card1 = cards[0];
            Transform card2 = cards[1];

            Vector3 centerPos = Vector3.Lerp(leftPosition.position, rightPosition.position, centerT); // ��� ��ġ ���

            // ������ �������� ��ġ
            Vector3 offset = (rightPosition.position - leftPosition.position).normalized * cardSpacing;

            card1.position = centerPos - offset / 2;
            card1.rotation = Quaternion.Euler(0f, 0f, maxAngle / 2);

            card2.position = centerPos + offset / 2;
            card2.rotation = Quaternion.Euler(0f, 0f, -maxAngle / 2);

            return; // �Լ� ����
        }

        // ī�尡 3���� ��
        if (numCards == 3)
        {
            Transform card1 = cards[0];
            Transform card2 = cards[1];
            Transform card3 = cards[2];

            Vector3 centerPos = Vector3.Lerp(leftPosition.position, rightPosition.position, centerT); // ��� ��ġ ���

            // ������ �������� ��ġ
            Vector3 offset = (rightPosition.position - leftPosition.position).normalized * cardSpacing;

            card1.position = centerPos - offset;
            card1.rotation = Quaternion.Euler(0f, 0f, maxAngle);

            card2.position = centerPos;
            card2.rotation = Quaternion.Euler(0f, 0f, 0f);

            card3.position = centerPos + offset;
            card3.rotation = Quaternion.Euler(0f, 0f, -maxAngle);

            return; // �Լ� ����
        }

        // ī�尡 4�� �̻��� ��
        float totalWidth = Vector3.Distance(leftPosition.position, rightPosition.position);
        float spacing = totalWidth / (Mathf.Max(numCards - 1, 1)); // ī�� ���� ���� ���

        for (int i = 0; i < numCards; i++)
        {
            Transform card = cards[i];
            float t = (float)i / Mathf.Max(numCards - 1, 1); // 0���� 1 ������ ���� ������ t ���
            float angle = Mathf.Lerp(maxAngle, -maxAngle, t); // �ּ� �������� �ִ� �������� ����

            // ī���� ��ġ ��� (�����ʿ��� �߰�)
            Vector3 rightPos = rightPosition.position;
            Vector3 newPos = rightPos - (rightPos - leftPosition.position).normalized * spacing * (numCards - 1 - i);

            // ���� ��ġ ����
            float verticalOffset = Mathf.Lerp(0, -maxVerticalOffset, Mathf.Abs(t - 0.5f) * 1.5f); // �߰��� �ִ� ī���ϼ��� ���̰� ���������� ����
            newPos.y += verticalOffset;

            // ī���� ������ ��ġ ����
            card.position = newPos;
            card.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
