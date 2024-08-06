using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{    
    //디버프관련변수
    public int frozenTurnsRemaining = 0; // 얼린 상태가 유지될 턴 수
    public int weakerTurnsRemaining = 0; // 약화 상태가 유지될 턴 수
    public int defDownTurnsRemaining = 0; //방깍 상태가 유지될 턴 수 
    public int burnTurnsRemaining = 0; //화상
    public int poisonTurnsRemaining = 0; //중독 
    public int bleedingTurnsRemaining = 0; //출혈

    public bool isFrozen; // 얼었는지 확인하는 용도

    public List<Condition> conditionInstances = new List<Condition>();

    public Animator animator;

    [Header("DeBuff_InputScript")]
    public GameObject deBuff;
    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public virtual IEnumerator Turn()
    {
        if (frozenTurnsRemaining > 0)
        {
            frozenTurnsRemaining--;
            Debug.Log($"{gameObject.name}는 얼어있습니다. 남은 얼린 턴 수: {frozenTurnsRemaining}");

            // SpawnDamageText로 "빙결" 텍스트 띄우도록 개조
            SpawnConditionText("빙결", transform.position);

            yield return new WaitForSeconds(2f); // 연출을 위한 대기
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            if (frozenTurnsRemaining == 0)
            {
                animator.StopPlayback();
                GameManager.instance.DestroyDeBuffAnim(deBuff); //얼음오브젝트 삭제 하는 곳
            }
            yield break;
        }
        else
        {
            isFrozen = false;
        }
        if (weakerTurnsRemaining > 0)
        {
            weakerTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Weaker);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            yield break;
        }
        else
        {
            monsterStats.attackPower = baseAttackPower;
        }
        if (defDownTurnsRemaining > 0)
        {
            weakerTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.DefDown);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            yield break;
        }
        if (burnTurnsRemaining > 0)
        {
            burnTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Burn);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(3);
        }
        if (poisonTurnsRemaining > 0)
        {
            poisonTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Poison);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(5);
        }
        if (bleedingTurnsRemaining > 0)
        {
            bleedingTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Bleeding);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(5);
        }
        yield return null;
    }
    private void SpawnConditionText(string conditionText, Vector3 position)
    {
        Color? textColor = conditionText == "빙결" ? new Color(0.53f, 0.81f, 0.92f) : (Color?)null;
        SpawnText(conditionText, position, textColor);
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
}
