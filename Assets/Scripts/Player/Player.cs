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

        // ConditionBox �������� conditionCanvas�� �ڽ����� �����ϰ� playerCondition�� �Ҵ�
        playerCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        if (DataManager.Instance.currenthealth != 0)
        {
            // �÷��̾ �����Ǹ� DataManager���� ü�°� ���� Player ������ �����´�.
            InitializeStats(DataManager.Instance.currenthealth);
        }

        hpBarPos = transform.GetChild(1);
        conditionPos = transform.GetChild(2);

        // GameManager�� ���� ĵ���� ����
        Canvas healthBarcanvas = UIManager.instance.healthBarCanvas;
        // healthBarPrefab�� healthBarcanvas�� �ڽ����� ����
        healthBarInstance = Instantiate(healthBarPrefab, healthBarcanvas.transform);
        healthBarInstance.Initialized(playerStats.maxhealth, currenthealth, hpBarPos);

        // ConditionBox �������� conditionCanvas�� �ڽ����� �����ϰ� playerCondition�� �Ҵ�
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

    // ���ο� Condition �ν��Ͻ��� �����ϰ� ����Ʈ�� �߰��� ��, ��ġ�� ������Ʈ
    /*
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab, ConditionType type)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, conditionPos, type); // ��ġ �ʱ�ȭ �Ŀ� ���� �� ����
        }
    }*/

    // ����Ʈ���� Condition �ν��Ͻ��� �����ϰ� ��ġ�� ������Ʈ
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
            // Condition ������Ʈ ���� ����
        }
    }

    // ���� Condition�� ���� ���� ����
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
