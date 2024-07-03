using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;

    private TMP_Text textMesh;
    private Color textColor;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TMP_Text>();
        textColor = textMesh.color;
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        textColor.a -= Time.deltaTime / fadeDuration;
        textMesh.color = textColor;

        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
