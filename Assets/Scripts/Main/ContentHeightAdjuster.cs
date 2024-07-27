using UnityEngine;
using UnityEngine.UI;

public class ContentHeightAdjuster : MonoBehaviour
{
    public RectTransform contentRectTransform; // Content 오브젝트의 RectTransform
    public float baseHeight = 1050f; // 기본 높이 값
    public float incrementHeight = 350f; // 카드 수에 따른 증가 높이 값
    public int cardsPerIncrement = 5; // 증가마다 고려할 카드 개수

    // Content 높이 조정 메서드
    public void AdjustContentHeight()
    {
        // 자식 오브젝트의 개수를 cardCount로 설정
        int cardCount = transform.childCount;

        // 기본 높이 설정
        float newHeight = baseHeight;

        // 카드 수가 10개 이하인 경우 기본 높이 유지
        if (cardCount > 10)
        {
            // 카드 수에 따라 증가 높이 계산
            newHeight = baseHeight + Mathf.Ceil((cardCount - 10) / (float)cardsPerIncrement) * incrementHeight;
        }

        // Content RectTransform의 높이 설정
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, newHeight);
    }
}
