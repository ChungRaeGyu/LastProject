using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public CharacterStats stats;

    public Character(int health, int defense, int attackPower, float actionGauge)
    {
        stats = new CharacterStats(health, defense, attackPower, actionGauge);
    }

    public abstract void Attack(Character target);

    public virtual void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
    }
}
