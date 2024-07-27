using UnityEngine;
using UnityEngine.UI;

public class ContentHeightAdjuster : MonoBehaviour
{
    public RectTransform contentRectTransform; // Content ������Ʈ�� RectTransform
    public float baseHeight = 1050f; // �⺻ ���� ��
    public float incrementHeight = 350f; // ī�� ���� ���� ���� ���� ��
    public int cardsPerIncrement = 5; // �������� ����� ī�� ����

    // Content ���� ���� �޼���
    public void AdjustContentHeight()
    {
        // �ڽ� ������Ʈ�� ������ cardCount�� ����
        int cardCount = transform.childCount;

        // �⺻ ���� ����
        float newHeight = baseHeight;

        // ī�� ���� 10�� ������ ��� �⺻ ���� ����
        if (cardCount > 10)
        {
            // ī�� ���� ���� ���� ���� ���
            newHeight = baseHeight + Mathf.Ceil((cardCount - 10) / (float)cardsPerIncrement) * incrementHeight;
        }

        // Content RectTransform�� ���� ����
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, newHeight);
    }
}
