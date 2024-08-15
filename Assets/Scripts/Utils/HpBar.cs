using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Transform hpBarPos;
    public Slider healthSlider;
    public TMP_Text healthText;

    public void Initialized(int maxhealth, int currentHealth, Transform transform)
    {
        hpBarPos = transform;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxhealth;
            healthSlider.value = currentHealth;
        }

        UpdatehealthText();
    }

    private void Update()
    {
        if (hpBarPos.gameObject.activeInHierarchy)
            transform.position = hpBarPos.position;
        else
            Destroy(gameObject);
    }

    public void ResetHealthSlider(int currenthealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currenthealth;
        }
    }

    public void UpdatehealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{healthSlider.value}/{healthSlider.maxValue}";
        }
    }
}
