using UnityEngine;
using UnityEngine.UI;

public class ScrollRectLimiter : MonoBehaviour
{
    public ScrollRect scrollRect; // ScrollRect 컴포넌트
    public float minY = 0f; // 스크롤 제한 최소 Y 값 (0 ~ 1 사이의 값)
    public float maxY = 1f; // 스크롤 제한 최대 Y 값 (0 ~ 1 사이의 값)

    void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    void OnScrollValueChanged(Vector2 scrollPosition)
    {
        if (scrollRect.vertical)
        {
            float newY = Mathf.Clamp(scrollRect.verticalNormalizedPosition, minY, maxY);
            if (Mathf.Abs(scrollRect.verticalNormalizedPosition - newY) > 0.01f)
            {
                scrollRect.verticalNormalizedPosition = newY;
            }
        }
    }
}
