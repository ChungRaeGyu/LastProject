using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterCharacter : MonoBehaviour
{
    public MonsterStats monsterStats;
    public int currenthealth { get; private set; }
    public Animator animator;

    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int Attack = Animator.StringToHash("Attack");

    public GameObject damageTextPrefab;

    private int frozenTurnsRemaining = 0; // 얼린 상태가 유지될 턴 수

    private void Awake()
    {
        if (monsterStats == null)
        {
            Debug.Log("MonsterStats가 " + gameObject.name + "에 할당되지 않았다.");
        }

        currenthealth = monsterStats.maxhealth;

        animator = GetComponentInChildren<Animator>();
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
        currenthealth -= actualDamage;

        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        DieAction();
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

    public virtual IEnumerator MonsterTurn()
    {
        if (frozenTurnsRemaining > 0)
        {
            frozenTurnsRemaining--;
            Debug.Log($"{gameObject.name}는 얼어있습니다. 남은 얼린 턴 수: {frozenTurnsRemaining}");
            yield return new WaitForSeconds(2f); // 연출을 위한 대기
            GameManager.instance.EndMonsterTurn(); // 몬스터 턴 종료 알림
            yield break;
        }

        yield return null;
    }

    public void FreezeForTurns(int turns)
    {
        frozenTurnsRemaining += turns;
        Debug.Log($"{gameObject.name}가 {turns}턴 동안 얼렸습니다.");
    }

    public void DieAction()
    {
        if (IsDead())
        {
            Die();
            DataManager.Instance.IncreaseMonstersKilledCount(); // DataManager에서 몬스터 카운트 증가
        }
    }
}
