using UnityEngine;

public class Monster : Character
{
    public Monster(int health, int defense, int attackPower, float actionGauge) : base(health, defense, attackPower, actionGauge)
    {

    }

    public override void Attack(Character target)
    {
        target.TakeDamage(stats.attackPower);
    }

    private void Update()
    {

    }
}
