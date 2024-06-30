using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    public int health;
    public int defense;
    public int attackPower;
    public float actionGauge; // 게이지 차는 속도
    public float maxGauge = 100f; // 게이지의 최대치
    public float currentGauge = 0f;

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

    public void UpdateGauge(float deltaTime)
    {
        currentGauge += deltaTime * actionGauge;
        if (currentGauge >= maxGauge)
        {
            currentGauge = maxGauge;
        }
    }

    public bool IsGaugeFull()
    {
        return currentGauge >= maxGauge;
    }

    public float GetCurrentGauge()
    {
        return currentGauge;
    }

    public void ResetGauge()
    {
        currentGauge = 0f;
    }
}
