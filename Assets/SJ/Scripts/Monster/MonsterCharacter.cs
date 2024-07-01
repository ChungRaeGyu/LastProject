using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCharacter : MonoBehaviour
{
    public MonsterStats monsterStats;
    protected int currenthealth;

    private void Awake()
    {
        if (monsterStats == null)
        {
            Debug.Log("MonsterStats가 " + gameObject.name + "에 할당되지 않았다.");
        }

        currenthealth = monsterStats.maxhealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
        currenthealth -= actualDamage;
    }

    protected virtual void Update()
    {
        if (IsDead())
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
