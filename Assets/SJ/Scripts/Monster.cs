using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    public Slider gaugeSlider;
    public Slider healthSlider;

    private void Start()
    {
        if (gaugeSlider != null)
        {
            gaugeSlider.maxValue = stats.maxGauge;
        }
        if (healthSlider != null)
        {
            healthSlider.maxValue = stats.health;
        }
    }

    public override void Attack(Character target)
    {
        target.TakeDamage(stats.attackPower);
    }

    protected override void Update()
    {
        base.Update();

        stats.UpdateGauge(Time.deltaTime);
        if (gaugeSlider != null)
        {
            gaugeSlider.value = stats.currentGauge;
        }
        if (healthSlider != null)
        {
            healthSlider.value = stats.health;
        }
    }
}
