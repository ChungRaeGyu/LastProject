using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    public int health;
    public int defense;
    public int attackPower;
    public float actionGauge;
    
    public CharacterStats(int health, int defense, int attackPower, float actionGauge)
    {
        this.health = health;
        this.defense = defense;
        this.attackPower = attackPower;
        this.actionGauge = actionGauge;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - defense, 0);
        health -= actualDamage;
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
