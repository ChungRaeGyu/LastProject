using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{    
    //��������ú���
    public int frozenTurnsRemaining = 0; // �� ���°� ������ �� ��
    public int weakerTurnsRemaining = 0; // ��ȭ ���°� ������ �� ��
    public int defDownTurnsRemaining = 0; //��� ���°� ������ �� �� 
    public int burnTurnsRemaining = 0; //ȭ��
    public int poisonTurnsRemaining = 0; //�ߵ� 
    public int bleedingTurnsRemaining = 0; //����

    public bool isFrozen; // ������� Ȯ���ϴ� �뵵

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
            // SpawnDamageText�� "����" �ؽ�Ʈ ��쵵�� ����
            SpawnConditionText("����", transform.position);

            yield return new WaitForSeconds(2f); // ������ ���� ���
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            if (frozenTurnsRemaining == 0)
            {
                animator.StopPlayback();
                GameManager.instance.DestroyDeBuffAnim(deBuff); //����������Ʈ ���� �ϴ� ��
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
        Color? textColor = conditionText == "����" ? new Color(0.53f, 0.81f, 0.92f) : (Color?)null;
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

            // ���� ����
            if (color.HasValue)
            {
                damageText.currentColor = color.Value;
            }

            // ȭ�� ��ǥ���� ���� �̵�
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            float yOffset = 100f; // �󸶳� ���� ��ġ���� ����
            Vector3 newScreenPosition = new Vector3(screenPosition.x, screenPosition.y + yOffset, 10f);
            textInstance.transform.position = Camera.main.ScreenToWorldPoint(newScreenPosition);
        }
    }

    #region �����
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
        //��ȭ : ������ ���ݷ��� ��������.
        weakerTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Weaker);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(GetConditionPos(), turns, GameManager.instance.weakerConditionPrefab, ConditionType.Weaker);
            //��ȭ 
            WeakingMethod(ability);
        }
    }
    public void DefDownForTurns(int turns, float ability)
    {
        //��� : ������ ������ ��������.
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
        //��Ʈ ��
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
        //��Ʈ ��
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
        //��Ʈ ��
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


    // ���ο� Condition �ν��Ͻ��� �����ϰ� ����Ʈ�� �߰��� ��, ��ġ�� ������Ʈ
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab, ConditionType type)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, GetConditionTransfrom(), type); // ��ġ �ʱ�ȭ �Ŀ� ���� �� ����
        }
    }
    #region ����� ���� �޼ҵ��
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
