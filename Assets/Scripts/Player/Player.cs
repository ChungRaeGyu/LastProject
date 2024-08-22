using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class Player : PlayerCharacter
{
    public HpBar healthBarPrefab;
    public int maxCost = 3;

    public int currentCost { get; private set; }
    private HpBar healthBarInstance;

    private Transform hpBarPos;
    private Transform conditionPos;

    public Transform playerCondition;

    protected override void Start()
    {
        base.Start();

        InitializeCost();

        // ConditionBox 프리팹을 conditionCanvas의 자식으로 생성하고 playerCondition에 할당
        playerCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        if (DataManager.Instance.currenthealth != 0)
        {
            // 플레이어가 생성되면 DataManager에서 체력과 같은 Player 정보를 가져온다.
            InitializeStats(DataManager.Instance.currenthealth);
        }

        hpBarPos = transform.GetChild(1);
        conditionPos = transform.GetChild(2);

        // GameManager를 통해 캔버스 참조
        Canvas healthBarcanvas = UIManager.instance.healthBarCanvas;
        // healthBarPrefab을 healthBarcanvas의 자식으로 생성
        healthBarInstance = Instantiate(healthBarPrefab, healthBarcanvas.transform);
        healthBarInstance.Initialized(playerStats.maxhealth, currenthealth, hpBarPos);

        // ConditionBox 프리팹을 conditionCanvas의 자식으로 생성하고 playerCondition에 할당
        playerCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        AddCondition(playerCondition, currentDefense, GameManager.instance.defenseconditionPrefab, ConditionType.Defense);
    }

    private void Update()
    {
        playerCondition.position = conditionPos.position;
    }
    /*
    protected override Transform GetConditionTransfrom()
    {
        return conditionPos;
    }
    */
    public override void InitializeStats(int currenthealthData)
    {
        base.InitializeStats(currenthealthData);
    }

    public override int SavePlayerStats()
    {
        return base.SavePlayerStats();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (healthBarInstance != null)
        {
            healthBarInstance.ResetHealthSlider(currenthealth);
            healthBarInstance.UpdatehealthText();
        }
    }

    public void UseCost(int amount)
    {
        currentCost -= amount;

        UpdateCostText();
    }

    public void AddCost(int amount)
    {
        currentCost += amount;
        UpdateCostText();
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        if (healthBarInstance != null)
        {
            healthBarInstance.ResetHealthSlider(currenthealth);
            healthBarInstance.UpdatehealthText();
        }
    }

    private void UpdateCostText()
    {
        TMP_Text costText = UIManager.instance.costText;
        if (costText != null)
        {
            costText.text = $"{currentCost}/{maxCost}";
        }
    }

    public void InitializeCost()
    {
        currentCost = maxCost;
        UpdateCostText();
    }

    // 새로운 Condition 인스턴스를 생성하고 리스트에 추가한 후, 위치를 업데이트
    /*
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab, ConditionType type)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, conditionPos, type); // 위치 초기화 후에 스택 값 설정
        }
    }*/

    // 리스트에서 Condition 인스턴스를 제거하고 위치를 업데이트
    public void RemoveCondition(Condition condition)
    {
        if (conditionInstances.Contains(condition))
        {
            conditionInstances.Remove(condition);
            Destroy(condition.gameObject);
            //UpdateConditionPositions();
        }
    }

    public void UpdateConditions()
    {
        foreach (var condition in conditionInstances)
        {
            // Condition 업데이트 로직 구현
        }
    }

    // 방어력 Condition의 스택 수를 증가
    public void IncrementDefenseConditionStack(int amount)
    {

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Defense);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(amount);
        }
        else
        {
            AddCondition(GetConditionPos(), amount, GameManager.instance.defenseconditionPrefab, ConditionType.Defense);
        }
    }

    protected override Transform GetConditionPos()
    {
        return playerCondition;
    }
    protected override Transform GetConditionTransfrom()
    {
        return conditionPos;
    }
}
