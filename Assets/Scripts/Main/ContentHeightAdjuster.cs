using UnityEngine;
using UnityEngine.UI;

public class ContentHeightAdjuster : MonoBehaviour
{
    public RectTransform contentRectTransform; // Content ������Ʈ�� RectTransform
    public float baseHeight = 1050f; // �⺻ ���� ��
    public float incrementHeight = 350f; // ī�� ���� ���� ���� ���� ��
    public int cardsPerIncrement = 5; // �������� ����� ī�� ����
    public int cardCount;

    // Content ���� ���� �޼���
    public void AdjustContentHeight()
    {
        // �⺻ ���� ����
        float newHeight = baseHeight;

        // ī�� ���� 10�� ������ ��� �⺻ ���� ����
        if (cardCount > 10)
        {
            Debug.Log($"cardCount : {cardCount}");
            // ī�� ���� ���� ���� ���� ���
            newHeight = baseHeight + Mathf.Ceil((cardCount - 10) / (float)cardsPerIncrement) * incrementHeight;
            Debug.Log($"�ݿø� ī�� ���� : {Mathf.Ceil((cardCount - 10) / (float)cardsPerIncrement)}");
            Debug.Log($"newHeight : {newHeight}");
        }

        // Content RectTransform�� ���� ����
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, newHeight);
    }
}
