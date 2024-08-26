using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public bool isFrozen; // ������� Ȯ���ϴ� �뵵

    public List<Condition> conditionInstances = new List<Condition>();

    public Animator animator;

    private Condition tempCondition;
    [Header("DeBuff_InputScript")]
    public GameObject deBuff;
    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public virtual IEnumerator Turn()
    {
        for(int i=0; i < conditionInstances.Count; i++)
        {
            conditionInstances[i].Turn(this); // stackCount ���ҷ����� �� ����Ǻ� ����
            //condition�� 0�̸� ���� 
            if (conditionInstances[i].stackCount <= 0)
            {
                Destroy(conditionInstances[i].gameObject);
                conditionInstances.Remove(conditionInstances[i]);
                i--;
            }
            yield return null;
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
    public void AddConditions(Condition conditionPrefab,int turns)
    {
        if (CheckCondition(conditionPrefab))
        {
            tempCondition.IncrementStackCount(turns);
        }
        else
        {
            conditionPrefab.Utility(this);
            AddCondition(GetConditionPos(), turns, conditionPrefab);
        }
    }
    private bool CheckCondition(Condition conditionPrefab)
    {
        foreach(Condition condition in conditionInstances)
        {
            if (condition.conditionType == conditionPrefab.conditionType)
            {
                tempCondition = condition;
                return true;
            }
        }
        return false;
    }

    public void AnimationStop()
    {
        animator.StopPlayback();
        GameManager.instance.DestroyDeBuffAnim(deBuff); //����������Ʈ ���� �ϴ� ��
    }
    #region �����

    #endregion

    // ���ο� Condition �ν��Ͻ��� �����ϰ� ����Ʈ�� �߰��� ��, ��ġ�� ������Ʈ
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, GetConditionTransfrom()); // ��ġ �ʱ�ȭ �Ŀ� ���� �� ����
        }
    }
    #region ����� ���� �޼ҵ��
    protected abstract Transform GetConditionPos();
    protected abstract Transform GetConditionTransfrom();
    public abstract void BaseWeakerMethod();
    public abstract void WeakingMethod(float ability);
    public abstract void BasedefMethod();

    public abstract void DefDownValue(float ability);
    public abstract void TakedamageCharacter(int damage);
    #endregion
}
