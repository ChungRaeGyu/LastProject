using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PlayerCharacter : MonoBehaviour
{
    public PlayerStats playerStats;
    public int currenthealth;
    public int currentDefense;
    public Animator animator;
    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int attack = Animator.StringToHash("Attack");
    private static readonly int die = Animator.StringToHash("Die");
    protected bool isDying = false; // �״� ������ ���θ� �����ϴ� ����

    private void Awake()
    {
        currentDefense = playerStats.defense;
        currenthealth = playerStats.maxhealth;
        animator = GetComponentInChildren<Animator>();
    }

    public virtual void InitializeStats(int currenthealthData)
    {
        currenthealth = currenthealthData;
    }

    public virtual int SavePlayerStats()
    {
        return currenthealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - currentDefense, 0);
        currenthealth -= actualDamage;

        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        if (IsDead())
        {
            StartCoroutine(Die());
        }
    }

    public virtual void Heal(int amount)
    {
        currenthealth += amount;
    }

    private void SpawnDamageText(int damageAmount, Vector3 position)
    {
        if (GameManager.instance.damageTextPrefab != null)
        {
            GameObject damageTextInstance = Instantiate(GameManager.instance.damageTextPrefab, position, Quaternion.identity);
            DamageText damageText = damageTextInstance.GetComponent<DamageText>();
            damageText.SetText(damageAmount.ToString());

            // ��ġ�� ȭ�� ��ǥ��
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            damageTextInstance.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
        }
    }

    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual IEnumerator Die()
    {
        if (isDying) yield break; // �̹� �״� ���̸� �ߺ� ������ ����
        isDying = true;

        if (animator != null)
        {
            animator.SetTrigger(die);
        }

        // �ִϸ��̼��� ���� ������ ���
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // ����г�(�й�)�� ������
        UIManager.instance.ShowDefeatPanel();
        UIManager.instance.ApplyDeathPenalty();
    }
}
