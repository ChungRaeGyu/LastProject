using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;
    private int bossTurnCount = 0;
    private bool bossHeal = false;
    private bool strongAttack = false;
    private System.Random random = new System.Random();
    private CountPos countPos;

    [SerializeField] private Condition defenseconditionPrefab; // �������� ������ �� �ֵ��� SerializeField �߰�

    private List<Condition> conditionInstances = new List<Condition>();
    public float conditionSpacing = 1f; // �� ����� ���� ����

    private void Start()
    {
        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth, monsterStats.maxhealth, transform.GetChild(1));
        }

        AddCondition(UIManager.instance.conditionCanvas.transform, monsterStats.defense);
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

    public void StartMonsterTurn()
    {
        StartCoroutine(MonsterTurn());
    }

    public override IEnumerator MonsterTurn()
    {
        bossTurnCount++;
        Debug.Log("----- ������ " + bossTurnCount + "�� ° -----");
        if (monsterStats.maxhealth < monsterStats.maxhealth / 2 && !bossHeal) // �� �� ���Ϸ� ������ �� 30 ȸ�� '�� ��'�� �ϱ�
        {
            monsterStats.maxhealth += 30;
            bossHeal = true;
            Debug.Log(this.name + "��" + 30 + "��ŭ ȸ���ߴ�!");
        }

        if (bossTurnCount <= 4 && !strongAttack) // 3�ϵ��� ���ݷ� 2�� ����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
            strongAttack = true;
            Debug.Log(this.name + "�ʹ� ����" + monsterStats.attackPower * 2 + "������");
        }

        else if (bossTurnCount % 10 == 0) // 10�� �� ���ݷ� 3�� ����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 3);
            Debug.Log(this.name + "�� ���� ������ �ߴ�!" + monsterStats.attackPower * 3 + "������");
        }

        else if (random.Next(0, 100) < 15) // 15% Ȯ���� ���ݷ� 2�� ����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
            Debug.Log(this.name + "�� ���� Ȯ���� ���Ѱ���!");
        }

        else // �⺻����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower);
        }
        yield return new WaitForSeconds(1f); // ������ ���� ���

        if (animator != null) // �ִϸ��̼�
        {
            animator.SetTrigger("Attack");
        }
        // ���� �Ŀ� �ʿ��� �ٸ� ����

        // ���� �Ŀ� ���� ���� ���� GameManager�� �˸�
        GameManager.instance.EndMonsterTurn();
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

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}
