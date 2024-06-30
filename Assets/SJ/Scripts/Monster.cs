using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    public Slider gaugeSlider;
    public Slider healthSlider;

    private void Start()
    {
        gaugeSlider.maxValue = stats.maxGauge;
        healthSlider.maxValue = stats.health;
    }

    public Monster(int health, int defense, int attackPower, float actionGauge) : base(health, defense, attackPower, actionGauge)
    {

    }

    public override void Attack(Character target)
    {
        target.TakeDamage(stats.attackPower);
    }

    protected override void Update()
    {
        base.Update();

        stats.UpdateGauge(Time.deltaTime);
        gaugeSlider.value = stats.currentGauge;
        healthSlider.value = stats.health;
    }
}
