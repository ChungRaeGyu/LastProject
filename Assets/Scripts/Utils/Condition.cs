using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Condition : MonoBehaviour
{
    private Transform conditionPos;
    public TMP_Text stackText;
    public int stackCount;
    public ConditionType conditionType;

    private void Update()
    {
        if (conditionPos == null)
            Destroy(gameObject);
    }

    // ��ġ�� �ʱ� ���� �� �� Ÿ�� ����
    public void Initialized(int initialStackCount, Transform transform, ConditionType type)
    {
        conditionPos = transform;
        stackCount = initialStackCount;
        conditionType = type;

        UpdateStackText();
    }

    // ���� ���� �ؽ�Ʈ�� ������Ʈ
    public void UpdateStackText()
    {
        if (stackText != null)
        {
            stackText.text = stackCount.ToString();
        }
    }

    // ���� ���� �����ϰ� �ؽ�Ʈ�� ������Ʈ
    public void SetStackCount(int count)
    {
        stackCount = count;
        UpdateStackText();
    }

    // ���� ���� ������Ű�� �ؽ�Ʈ�� ������Ʈ
    public void IncrementStackCount(int amount = 1) // �⺻���� 1
    {
        stackCount += amount;
        UpdateStackText();
    }

    // ���� ���� ���ҽ�Ű��, ������ 0 �����̸� ��ü�� �ı�
    public void DecrementStackCount(MonsterCharacter monsterCharacter, int amount = 1) // �⺻���� 1
    {
        stackCount -= amount;
        UpdateStackText();

        if (stackCount <= 0)
        {
            monsterCharacter.conditionInstances.Remove(this);
            Destroy(gameObject);
        }
    }
}

public enum ConditionType
{
    Defense,
    Frozen,
    Weaker,
    DefDown,
    Burn,
    Poison,
    Bleeding
}
