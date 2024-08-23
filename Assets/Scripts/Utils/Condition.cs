using TMPro;
using UnityEngine;


public abstract class Condition : MonoBehaviour
{
    //

    public abstract void Turn(Character character);
    public virtual void Utility(Character character)
    {

    }
    
    //
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
    // ��ġ�� �ʱ� ���� �� �� Ÿ�� ����
    /* public void Initialized(int initialStackCount, Transform transform, ConditionType type)
     {
         conditionPos = transform;
         stackCount = initialStackCount;
         conditionType = type;

         UpdateStackText();
     }*/

    // ���� ���� �ؽ�Ʈ�� ������Ʈ
    public void UpdateStackText()
    {
        if (stackText != null)
        {
            stackText.text = stackCount.ToString();
        }
    }

/*    // ���� ���� �����ϰ� �ؽ�Ʈ�� ������Ʈ
    public void SetStackCount(int count)
    {
        stackCount = count;
        UpdateStackText();
    }*/

    // ���� ���� ������Ű�� �ؽ�Ʈ�� ������Ʈ
    public void IncrementStackCount(int amount = 1) // �⺻���� 1
    {
        stackCount += amount;
        UpdateStackText();
    }

    
    /*// ���� ���� ���ҽ�Ű��, ������ 0 �����̸� ��ü�� �ı�
    public void DecrementStackCount(Character character, int amount = 1) // �⺻���� 1
    {
        stackCount -= amount;
        UpdateStackText();
        if (stackCount <= 0)
        {
            character.conditionInstances.Remove(this);
            Destroy(gameObject);
        }
    }*/
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
