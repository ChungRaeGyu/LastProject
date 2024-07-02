using UnityEngine;

public abstract class PlayerCharacter : MonoBehaviour
{
    public PlayerStats playerStats;
    protected int currenthealth;

    private void Awake()
    {
        if (playerStats == null)
        {
            Debug.Log("CharacterStats�� " + gameObject.name + "�� �Ҵ���� �ʾҴ�.");
        }

        currenthealth = playerStats.maxhealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - playerStats.defense, 0);
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
