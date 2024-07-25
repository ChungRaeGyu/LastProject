using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;

    public TMP_Text damageText;
    private Color initialColor;
    public Color currentColor;

    private void Awake()
    {
        initialColor = damageText.color;
        currentColor = initialColor;
    }

    public void SetText(string text)
    {
        damageText.text = text;
    }

    private void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        // 투명도 감소
        currentColor.a -= Time.deltaTime / fadeDuration;
        currentColor.a = Mathf.Clamp01(currentColor.a); // a 값을 0과 1 사이로 제한
        damageText.color = currentColor;

        // 투명도가 0 이하가 되면 오브젝트 삭제
        if (currentColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
