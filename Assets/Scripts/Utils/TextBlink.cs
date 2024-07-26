using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{
    public TMP_Text textComponent;
    public float blinkDuration = 1f; // ±ôºýÀÓ ÁÖ±â

    private float timer;
    private bool isFadingIn = true;

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / blinkDuration;
        if (isFadingIn)
        {
            SetTextAlpha(Mathf.Lerp(0f, 1f, t));
        }
        else
        {
            SetTextAlpha(Mathf.Lerp(1f, 0f, t));
        }

        if (timer >= blinkDuration)
        {
            timer = 0f;
            isFadingIn = !isFadingIn;
        }
    }

    void SetTextAlpha(float alpha)
    {
        Color color = textComponent.color;
        color.a = alpha;
        textComponent.color = color;
    }
}
