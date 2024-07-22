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
    protected bool isDying = false; // 죽는 중인지 여부를 저장하는 변수

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

            // 위치를 화면 좌표로
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
        if (isDying) yield break; // 이미 죽는 중이면 중복 실행을 막음
        isDying = true;

        if (animator != null)
        {
            animator.SetTrigger(die);
        }

        // 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 결과패널(패배)를 보여줌
        UIManager.instance.ShowDefeatPanel();
        UIManager.instance.ApplyDeathPenalty();
    }
}
