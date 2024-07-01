using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
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
            healthSlider.maxValue = stats.maxhealth;
            healthSlider.value = currenthealth;
        }
    }

    public override void Attack(Character target)
    {
        // 플레이어의 공격 동작 구현
    }

    protected override void Update()
    {
        base.Update();

        if (healthSlider != null)
        {
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

    private void UpdateCostText()
    {
        if (costText != null)
        {
            costText.text = $"{currentCost}/{maxCost}";
        }
    }

    private void InitializeCost()
    {
        currentCost = maxCost;
        UpdateCostText();
    }
}
