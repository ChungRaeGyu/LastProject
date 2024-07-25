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
        // ���� �̵�
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        // ���� ����
        currentColor.a -= Time.deltaTime / fadeDuration;
        currentColor.a = Mathf.Clamp01(currentColor.a); // a ���� 0�� 1 ���̷� ����
        damageText.color = currentColor;

        // ������ 0 ���ϰ� �Ǹ� ������Ʈ ����
        if (currentColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
