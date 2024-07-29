using TMPro;
using UnityEngine;

public class AdjustHeightBasedOnText : MonoBehaviour
{
    public RectTransform parentRectTransform;
    public TMP_Text childText;
    public int lineHeight = 50;

    void Start()
    {
        if (parentRectTransform == null)
        {
            parentRectTransform = GetComponent<RectTransform>();
        }

        if (childText == null)
        {
            childText = GetComponentInChildren<TMP_Text>();
        }

        AdjustHeight();
    }

    void Update()
    {
        AdjustHeight();
    }

    public void AdjustHeight()
    {
        int lineCount = GetLineCount(childText);
        float newHeight = lineCount * lineHeight;

        parentRectTransform.sizeDelta = new Vector2(parentRectTransform.sizeDelta.x, newHeight);
    }

    int GetLineCount(TMP_Text text)
    {
        // �ؽ�Ʈ�� �� ���� ��ȯ��
        return text.textInfo.lineCount;
    }
}
