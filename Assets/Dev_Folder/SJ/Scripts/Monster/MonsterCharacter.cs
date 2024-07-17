using System;
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

    private List<Condition> conditionInstances = new List<Condition>();

    public Transform hpBarPos; // HP 바 위치
    public Transform conditionPos; // 컨디션 위치

    [HideInInspector]
    public Transform MonsterCondition;

    public int frozenTurnsRemaining = 0; // 얼린 상태가 유지될 턴 수
    protected bool isFrozen; // 얼었는지 확인하는 용도

    private void Awake()
    {
        if (monsterStats == null)
        {
            Debug.Log("MonsterStats가 " + gameObject.name + "에 할당되지 않았다.");
        }

        currenthealth = monsterStats.maxhealth;

        animator = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        // ConditionBox 프리팹을 conditionCanvas의 자식으로 생성하고 playerCondition에 할당
        MonsterCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        AddCondition(MonsterCondition, monsterStats.defense, GameManager.instance.defenseconditionPrefab, ConditionType.Defense);
    }

    private void Update()
    {
        MonsterCondition.position = conditionPos.position;
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

    private void SpawnConditionText(string conditionText, Vector3 position)
    {
        if (damageTextPrefab != null)
        {
            GameObject conditionTextInstance = Instantiate(damageTextPrefab, position, Quaternion.identity);
            DamageText damageText = conditionTextInstance.GetComponent<DamageText>();
            damageText.SetText(conditionText);

            // 빙결 상태일 때만 텍스트 파란색으로
            if (conditionText == "빙결")
            {
                damageText.stateColor = new Color(0.53f, 0.81f, 0.92f);
            }

            // 위치를 화면 좌표로
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            conditionTextInstance.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
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

            // SpawnDamageText로 "빙결" 텍스트 띄우도록 개조
            SpawnConditionText("빙결", transform.position);

            yield return new WaitForSeconds(2f); // 연출을 위한 대기
            GameManager.instance.EndMonsterTurn(); // 몬스터 턴 종료 알림
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount();
            }
            isFrozen = true;
            yield break;
        }
        else
        {
            isFrozen = false;
        }

        yield return null;
    }

    public virtual void FreezeForTurns(int turns)
    {
        frozenTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.frozenConditionPrefab, ConditionType.Frozen);
        }

        Debug.Log($"{gameObject.name}가 {turns}턴 동안 얼렸습니다. 남은 얼린 턴 수: {frozenTurnsRemaining}");
    }

    // 새로운 Condition 인스턴스를 생성하고 리스트에 추가한 후, 위치를 업데이트
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab, ConditionType type)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, conditionPos, type); // 위치 초기화 후에 스택 값 설정
        }
    }

    // 리스트에서 Condition 인스턴스를 제거하고 위치를 업데이트
    public void RemoveCondition(Condition condition)
    {
        if (conditionInstances.Contains(condition))
        {
            conditionInstances.Remove(condition);
            Destroy(condition.gameObject);
            //UpdateConditionPositions();
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
