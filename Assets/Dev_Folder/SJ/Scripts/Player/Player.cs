using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : PlayerCharacter
{
    public HpBar healthBarPrefab;
    public TextMeshProUGUI costText;
    public int maxCost = 3;

    public int currentCost { get; private set; }
    private HpBar healthBarInstance;

    public Canvas canvas;

    private void Start()
    {
        InitializeCost();

        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab을 canvas의 자식으로 생성
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);

            healthBarInstance.Initialized(playerStats.maxhealth, currenthealth, transform.GetChild(1));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        healthBarInstance.ResetHealthSlider(currenthealth);
        healthBarInstance.UpdatehealthText();
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
}
