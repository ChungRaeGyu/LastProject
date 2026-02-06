using TMPro;
using UnityEngine;


public abstract class Condition : MonoBehaviour
{
    public abstract void Turn(Character character);
    public virtual void Utility(Character character)
    {

    }

    private Transform conditionPos;
    public TMP_Text stackText;
    public int stackCount;
    public ConditionType conditionType;
    private void Update()
    {
        if (!conditionPos.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }

    public void Initialized(int initialStackCount, Transform transform)
    {
        conditionPos = transform;
        stackCount = initialStackCount;

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

    // 스택 수를 증가시키고 텍스트를 업데이트
    public void IncrementStackCount(int amount = 1) // 기본값은 1
    {
        stackCount += amount;
        UpdateStackText();
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
