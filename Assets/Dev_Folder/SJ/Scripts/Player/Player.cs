using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : PlayerCharacter
{
    public Slider healthSlider;
    public TextMeshProUGUI costText;
    public int maxCost = 3;

    public int currentCost { get; private set; }

    private void Start()
    {
        InitializeCost();

        if (healthSlider != null)
        {
            healthSlider.maxValue = playerStats.maxhealth;
            healthSlider.value = currenthealth;
        }
    }

    public void UseCost(int amount)
    {
        currentCost = Mathf.Clamp(currentCost - amount, 0, maxCost);
        UpdateCostText();
    }

    public void AddCost(int amount)
    {
        currentCost = Mathf.Clamp(currentCost + amount, 0, maxCost);
        UpdateCostText();
    }

    public void Heal(int amount)
    {
        currenthealth += amount;
    }

    private void UpdateCostText()
    {
        if (costText != null)
        {
            costText.text = $"{currentCost}/{maxCost}";
        }
    }

    public void InitializeCost()
    {
        currentCost = maxCost;
        UpdateCostText();
    }

    public void ResetHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currenthealth;
        }
    }

    private void Update()
    {
        ResetHealthSlider();
    }
}
