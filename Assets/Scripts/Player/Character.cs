using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public bool isFrozen; // 얼었는지 확인하는 용도

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
            conditionInstances[i].Turn(this); // stackCount 감소로직과 각 컨디션별 로직
            //condition이 0이면 삭제 
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
        GameManager.instance.DestroyDeBuffAnim(deBuff); //얼음오브젝트 삭제 하는 곳
    }
    #region 디버프

    #endregion

    // 새로운 Condition 인스턴스를 생성하고 리스트에 추가한 후, 위치를 업데이트
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, GetConditionTransfrom()); // 위치 초기화 후에 스택 값 설정
        }
    }
    #region 상속을 위한 메소드들
    protected abstract Transform GetConditionPos();
    protected abstract Transform GetConditionTransfrom();
    public abstract void BaseWeakerMethod();
    public abstract void WeakingMethod(float ability);
    public abstract void BasedefMethod();

    public abstract void DefDownValue(float ability);
    public abstract void TakedamageCharacter(int damage);
    #endregion
}
