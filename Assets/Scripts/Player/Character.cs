using System;
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
        }
        else
        {
            BaseWeakerMethod();
        }
        if (defDownTurnsRemaining > 0)
        {
            defDownTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.DefDown);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
        }
        else
        {
            BasedefMethod();
        }
        if (burnTurnsRemaining > 0)
        {
            burnTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Burn);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakedamageCharacter(3);
        }
        if (poisonTurnsRemaining > 0)
        {
            poisonTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Poison);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakedamageCharacter(5);
        }
        if (bleedingTurnsRemaining > 0)
        {
            bleedingTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Bleeding);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakedamageCharacter(5);
        }
    }

    private void SpawnConditionText(string conditionText, Vector3 position)
    {
        Color? textColor = conditionText == "빙결" ? new Color(0.53f, 0.81f, 0.92f) : (Color?)null;
        SpawnText(conditionText, position, textColor);
    }

    protected void SpawnDamageText(int damageAmount, Vector3 position)
    {
        SpawnText(damageAmount.ToString(), position);
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
            float yOffset = 100f; // 얼마나 위로 위치할지 설정
            Vector3 newScreenPosition = new Vector3(screenPosition.x, screenPosition.y + yOffset, 10f);
            textInstance.transform.position = Camera.main.ScreenToWorldPoint(newScreenPosition);
        }
    }

    #region 디버프
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
            AddCondition(GetConditionPos(), turns, GameManager.instance.frozenConditionPrefab, ConditionType.Frozen);
        }
    }
    public void WeakForTurns(int turns, float ability)
    {
        //약화 : 몬스터의 공격력이 약해진다.
        weakerTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Weaker);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(GetConditionPos(), turns, GameManager.instance.weakerConditionPrefab, ConditionType.Weaker);
            //약화 
            WeakingMethod(ability);
        }
    }
    public void DefDownForTurns(int turns, float ability)
    {
        //취약 : 몬스터의 방어력이 약해진다.
        defDownTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.DefDown);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(GetConditionPos(), turns, GameManager.instance.defDownConditionPrefab, ConditionType.DefDown);
            DefDownValue(ability);
        }
    }

    public void burnForTunrs(int turns)
    {
        //도트 딜
        burnTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Burn);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(GetConditionPos(), turns, GameManager.instance.burnConditionPrefab, ConditionType.Burn);
        }

    }
    public void PoisonForTunrs(int turns)
    {
        //도트 딜
        poisonTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Poison);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(GetConditionPos(), turns, GameManager.instance.poisonConditionPrefab, ConditionType.Poison);
        }

    }
    public void BleedingForTunrs(int turns)
    {
        //도트 딜
        bleedingTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Bleeding);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(GetConditionPos(), turns, GameManager.instance.bleedingConditioinPrefab, ConditionType.Bleeding);
        }

    }

    #endregion


    // 새로운 Condition 인스턴스를 생성하고 리스트에 추가한 후, 위치를 업데이트
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab, ConditionType type)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, GetConditionTransfrom(), type); // 위치 초기화 후에 스택 값 설정
        }
    }
    #region 상속을 위한 메소드들
    protected virtual Transform GetConditionPos()
    {
        return null;
    }
    protected virtual Transform GetConditionTransfrom()
    {
        return null;
    }
    protected virtual void BaseWeakerMethod()
    {

    }
    protected virtual void WeakingMethod(float ability)
    {

    }
    protected virtual void BasedefMethod()
    {

    }
    protected virtual void DefDownValue(float ability)
    {

    }
    protected virtual void TakedamageCharacter(int damage)
    {

    }
    #endregion
}
