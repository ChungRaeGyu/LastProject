using UnityEngine;
using UnityEngine.UI;

public class ScrollRectLimiter : MonoBehaviour
{
    public ScrollRect scrollRect; // ScrollRect ������Ʈ
    public float minY = 0f; // ��ũ�� ���� �ּ� Y �� (0 ~ 1 ������ ��)
    public float maxY = 1f; // ��ũ�� ���� �ִ� Y �� (0 ~ 1 ������ ��)

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
