using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public CharacterStats stats;

    private void Awake()
    {
        if (stats == null)
        {
            Debug.Log("CharacterStats�� " + gameObject.name + "�� �Ҵ���� �ʾҴ�.");
        }
    }

    public abstract void Attack(Character target);

    public virtual void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
    }

    protected virtual void Update()
    {
        if (stats.IsDead())
        {
            Destroy(gameObject);
        }
    }
}
