using UnityEngine;

public abstract class PlayerCharacter : MonoBehaviour
{
    public PlayerStats playerStats;
    protected int currenthealth;
    public Animator animator;
    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int attack = Animator.StringToHash("Attack");
    public GameObject damageTextPrefab;

    private void Awake()
    {
        currenthealth = playerStats.maxhealth;

        animator = GetComponentInChildren<Animator>();
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - playerStats.defense, 0);
        currenthealth -= actualDamage;

        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        if (IsDead())
        {
            Die();
        }
    }

    private void SpawnDamageText(int damageAmount, Vector3 position)
    {
        if (damageTextPrefab != null)
        {
            GameObject damageTextInstance = Instantiate(damageTextPrefab, position, Quaternion.identity);
            DamageText damageText = damageTextInstance.GetComponent<DamageText>();
            damageText.SetText(damageAmount.ToString());

            // 위치를 화면 좌표로
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            damageTextInstance.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
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
