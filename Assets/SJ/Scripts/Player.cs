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
        if (healthSlider != null)
        {
            healthSlider.maxValue = stats.maxhealth;
            healthSlider.value = currenthealth;
        }

        currentCost = maxCost;
        UpdateCostText();
    }

    public override void Attack(Character target)
    {

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
}
