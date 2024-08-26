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
    // ���� ���� �ؽ�Ʈ�� ������Ʈ
    public void UpdateStackText()
    {
        if (stackText != null)
        {
            stackText.text = stackCount.ToString();
        }
    }

    // ���� ���� ������Ű�� �ؽ�Ʈ�� ������Ʈ
    public void IncrementStackCount(int amount = 1) // �⺻���� 1
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
