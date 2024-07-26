using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterCharacter : MonoBehaviour
{
    public MonsterStats monsterStats;
    public int currenthealth { get; private set; }
    public Animator animator;

    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int Attack = Animator.StringToHash("Attack");

    private List<Condition> conditionInstances = new List<Condition>();

    public Transform hpBarPos; // HP 바 위치
    public Transform conditionPos; // 컨디션 위치

    public TMP_Text monsterName;

    [HideInInspector]
    public Transform MonsterCondition;
    public int frozenTurnsRemaining = 0; // 얼린 상태가 유지될 턴 수

    public bool isFrozen; // 얼었는지 확인하는 용도


    [Header("DeBuff_InputScript")]
    public GameObject deBuff;

    public Action deBuffAnim;
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

    private void SpawnText(string text, Vector3 position, Color? color = null)
    {
        if (GameManager.instance.damageTextPrefab != null)
        {
            GameObject textInstance = Instantiate(GameManager.instance.damageTextPrefab, position, Quaternion.identity);
            DamageText damageText = textInstance.GetComponent<DamageText>();
            damageText.SetText(text);

            // 색상 설정
            if (color.HasValue)
            {
                damageText.currentColor = color.Value;
            }

            // 화면 좌표에서 위로 이동
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            float yOffset = 200f; // 얼마나 위로 위치할지 설정
            Vector3 newScreenPosition = new Vector3(screenPosition.x, screenPosition.y + yOffset, 10f);
            textInstance.transform.position = Camera.main.ScreenToWorldPoint(newScreenPosition);
        }
    }

    private void SpawnDamageText(int damageAmount, Vector3 position)
    {
        SpawnText(damageAmount.ToString(), position);
    }

    private void SpawnConditionText(string conditionText, Vector3 position)
    {
        Color? textColor = conditionText == "빙결" ? new Color(0.53f, 0.81f, 0.92f) : (Color?)null;
        SpawnText(conditionText, position, textColor);
    }


    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual void Die()
    {
        if(isFrozen)
            GameManager.instance.DeBuffAnim(deBuff);
        Destroy(gameObject);

    }

    public virtual IEnumerator MonsterTurn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

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
            if (frozenTurnsRemaining == 0)
            {
                animator.StopPlayback();
                GameManager.instance.DeBuffAnim(deBuff); //얼음오브젝트 삭제 하는 곳
            }
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
        isFrozen = true;
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
