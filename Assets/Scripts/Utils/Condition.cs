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

    // 위치와 초기 스택 수 및 타입 설정
    public void Initialized(int initialStackCount, Transform transform, ConditionType type)
    {
        conditionPos = transform;
        stackCount = initialStackCount;
        conditionType = type;

        UpdateStackText();
    }

    // 스택 수를 텍스트로 업데이트
    public void UpdateStackText()
    {
        if (stackText != null)
        {
            stackText.text = stackCount.ToString();
        }
    }

    // 스택 수를 설정하고 텍스트를 업데이트
    public void SetStackCount(int count)
    {
        stackCount = count;
        UpdateStackText();
    }

    // 스택 수를 증가시키고 텍스트를 업데이트
    public void IncrementStackCount(int amount = 1) // 기본값은 1
    {
        stackCount += amount;
        UpdateStackText();
    }

    // 스택 수를 감소시키고, 스택이 0 이하이면 객체를 파괴
    public void DecrementStackCount(MonsterCharacter monsterCharacter, int amount = 1) // 기본값은 1
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
