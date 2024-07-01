using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public MonsterInfo monster;
    public CharacterStats stats;
    protected int currenthealth;

    private void Awake()
    {
        if (stats == null && monster == null)
        {
            Debug.Log("CharacterStats�� " + gameObject.name + "�� �Ҵ���� �ʾҴ�.");
        }

        currenthealth = stats.maxhealth;
        currenthealth = monster.maxhealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - stats.defense, 0);
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
