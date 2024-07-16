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
    [SerializeField] private Condition defenseconditionPrefab; // 프리팹을 설정할 수 있도록 SerializeField 추가

    private List<Condition> conditionInstances = new List<Condition>();
    public float conditionSpacing = 1f; // 각 컨디션 간의 간격

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
        frozenTurnsRemaining = turns;
        Debug.Log($"{gameObject.name}가 {turns}턴 동안 얼렸습니다.");
    }

    // 새로운 Condition 인스턴스를 생성하고 리스트에 추가한 후, 위치를 업데이트
    public void AddCondition(Transform parent, int initialStackCount)
    {
        if (defenseconditionPrefab != null)
        {
            Condition newCondition = Instantiate(defenseconditionPrefab, parent);
            conditionInstances.Add(newCondition);
            UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, newCondition.transform); // 위치 초기화 후에 스택 값 설정
        }
    }

    // 리스트에서 Condition 인스턴스를 제거하고 위치를 업데이트
    public void RemoveCondition(Condition condition)
    {
        if (conditionInstances.Contains(condition))
        {
            conditionInstances.Remove(condition);
            Destroy(condition.gameObject);
            UpdateConditionPositions();
        }
    }

    // 모든 Condition 인스턴스를 제거 (모든 해로운 효과 한번에 제거용도, 안써도 됨)
    public void ClearConditions()
    {
        foreach (var condition in conditionInstances)
        {
            Destroy(condition.gameObject);
        }
        conditionInstances.Clear();
    }

    // 각 Condition의 위치를 transform.GetChild(2)를 기준으로 우측으로 하나씩 나열 (위치 업데이트 용도)
    public void UpdateConditionPositions()
    {
        for (int i = 0; i < conditionInstances.Count; i++)
        {
            Vector3 newPosition = transform.GetChild(2).position + new Vector3(conditionSpacing * i, 0, 0);
            conditionInstances[i].transform.position = newPosition;
        }
    }

    public void UpdateConditions()
    {
        foreach (var condition in conditionInstances)
        {
            // Condition 업데이트 로직 구현
        }
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
