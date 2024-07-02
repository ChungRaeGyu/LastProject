using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCharacter : MonoBehaviour
{
    public MonsterStats monsterStats;
    public int currenthealth ;

    private void Awake()
    {
        if (monsterStats == null)
        {
            Debug.Log("MonsterStats�� " + gameObject.name + "�� �Ҵ���� �ʾҴ�.");
        }

        currenthealth = monsterStats.maxhealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
        currenthealth -= actualDamage;

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
