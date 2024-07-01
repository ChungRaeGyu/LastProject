using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public CharacterStats stats;
    protected int currenthealth;

    private void Awake()
    {
        if (stats == null)
        {
            Debug.Log("CharacterStats�� " + gameObject.name + "�� �Ҵ���� �ʾҴ�.");
        }

        currenthealth = stats.maxhealth;
    }

    public abstract void Attack(Character target);

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
