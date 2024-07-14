using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Condition : MonoBehaviour
{
    private Transform conditionPos;
    public TMP_Text stackText;
    private int stackCount;

    // ��ġ�� �ʱ� ���� �� ����
    public void Initialized(int initialStackCount, Transform transform)
    {
        conditionPos = transform;
        stackCount = initialStackCount;

        UpdateStackText();
    }

    private void Update()
    {
        if (conditionPos != null)
            transform.position = conditionPos.position;
        else
            Destroy(gameObject);
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
    public void DecrementStackCount(int amount = 1) // �⺻���� 1
    {
        stackCount -= amount;
        UpdateStackText();

        if (stackCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
