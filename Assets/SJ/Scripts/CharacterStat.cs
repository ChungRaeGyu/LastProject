using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character/Stats", order = 0)]
public class CharacterStats : ScriptableObject
{
    [Header("Base Stats")]
    public int health;
    public int defense;
    public int attackPower;
    public float actionGauge;
    public float maxGauge = 100f;

    [HideInInspector] public float currentGauge = 0f;

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

    public void ResetGauge()
    {
        currentGauge = 0f;
    }
}
