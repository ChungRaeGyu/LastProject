using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : PlayerCharacter
{
    public HpBar healthBarPrefab;
    public Condition defenseconditionPrefab;
    public int maxCost = 3;

    public int currentCost { get; private set; }
    private HpBar healthBarInstance;
    private List<Condition> conditionInstances = new List<Condition>();
    public float conditionSpacing = 1f; // �� ����� ���� ����

    private void Start()
    {
        InitializeCost();

        if (DataManager.Instance.currenthealth != 0)
        {
            // �÷��̾ �����Ǹ� DataManager���� ü�°� ���� Player ������ �����´�.
            InitializeStats(DataManager.Instance.currenthealth);
        }

        // GameManager�� ���� ĵ���� ����
        Canvas healthBarcanvas = UIManager.instance.healthBarCanvas;
        // healthBarPrefab�� healthBarcanvas�� �ڽ����� ����
        healthBarInstance = Instantiate(healthBarPrefab, healthBarcanvas.transform);
        healthBarInstance.Initialized(playerStats.maxhealth, currenthealth, transform.GetChild(1));

        // ���÷� �ʱ� ���� �� 1�� ���ο� ��0����� �߰�
        AddCondition(UIManager.instance.conditionCanvas.transform, playerStats.defense);
    }

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

        //// �׽�Ʈ�� - ����� �߰�
        //AddCondition(UIManager.instance.conditionCanvas.transform, 1);
    }

    public void UseCost(int amount)
    {
        currentCost = Mathf.Clamp(currentCost - amount, 0, maxCost);
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
    public void AddCondition(Transform parent, int initialStackCount)
    {
        if (defenseconditionPrefab != null)
        {
            Condition newCondition = Instantiate(defenseconditionPrefab, parent);
            conditionInstances.Add(newCondition);
            UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, newCondition.transform); // ��ġ �ʱ�ȭ �Ŀ� ���� �� ����
        }
    }

    // ����Ʈ���� Condition �ν��Ͻ��� �����ϰ� ��ġ�� ������Ʈ
    public void RemoveCondition(Condition condition)
    {
        if (conditionInstances.Contains(condition))
        {
            conditionInstances.Remove(condition);
            Destroy(condition.gameObject);
            UpdateConditionPositions();
        }
    }

    // ��� Condition �ν��Ͻ��� ���� (��� �طο� ȿ�� �ѹ��� ���ſ뵵, �Ƚᵵ ��)
    public void ClearConditions()
    {
        foreach (var condition in conditionInstances)
        {
            Destroy(condition.gameObject);
        }
        conditionInstances.Clear();
    }

    // �� Condition�� ��ġ�� transform.GetChild(2)�� �������� �������� �ϳ��� ���� (��ġ ������Ʈ �뵵)
    public void UpdateConditionPositions()
    {
        for (int i = 0; i < conditionInstances.Count; i++)
        {
            Vector3 newPosition = transform.GetChild(2).position + new Vector3(conditionSpacing * i, 0, 0);
            conditionInstances[i].transform.position = newPosition;
        }
    }

    public void UpdateConditions()
    {
        foreach (var condition in conditionInstances)
        {
            // Condition ������Ʈ ���� ����
        }
    }
}
