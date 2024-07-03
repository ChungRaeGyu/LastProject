using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PlayerCharacter : MonoBehaviour
{
    public PlayerStats playerStats;
    protected int currenthealth;
    public Animator animator;
    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int attack = Animator.StringToHash("Attack");

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
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }
}
