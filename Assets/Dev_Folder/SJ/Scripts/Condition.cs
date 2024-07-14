using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Condition : MonoBehaviour
{
    private Transform conditionPos;
    public TMP_Text stackText;
    private int stackCount;

    // 위치와 초기 스택 수 설정
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
    public void DecrementStackCount(int amount = 1) // 기본값은 1
    {
        stackCount -= amount;
        UpdateStackText();

        if (stackCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
