using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    public Slider healthSlider;

    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = stats.maxhealth;
            healthSlider.value = currenthealth;
        }
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
}
