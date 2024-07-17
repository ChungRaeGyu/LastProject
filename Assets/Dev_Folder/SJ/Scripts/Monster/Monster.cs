using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private System.Random random = new System.Random();

    [SerializeField] private Condition defenseconditionPrefab; // �������� ������ �� �ֵ��� SerializeField �߰�

    private List<Condition> conditionInstances = new List<Condition>();
    public float conditionSpacing = 1f; // �� ����� ���� ����

    private void Start()
    {
        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(0, 6); // 0~5 ���� ���� hp��� ȿ��

            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth + hpUp, monsterStats.maxhealth + hpUp, transform.GetChild(1));
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
        //������ �Ǵ� �ǰ�?
        Debug.Log("StartMonsterTurn���� : " + IsDead());

        StartCoroutine(MonsterTurn());
    }

    public override IEnumerator MonsterTurn()
    {
        //counter++;
        //if(counter >= 2 && !counterOnOff) // 2�Ͽ� ����
        //{
        //    Debug.Log(this.name + "����� �� �ɾ���! " + 3 + " �� �������� �Ծ���!");
        //    counterOnOff = true; // ����� Ȱ��ȭ
        //    GameManager.instance.player.TakeDamage(playerStats.maxhealth - 3); // player�ʿ� �̹��� ����
        //    if(counter >= 5 && !counterOnOff) // 2,3,4 �� ���� ��Ʈ ������
        //    {
        //        counterOnOff = false; // ����� ��Ȱ��ȭ
        //        counter = 0; // ī���� �ʱ�ȭ �ٽ� 0�Ϻ���
        //        Debug.Log(this.name + "����� ��! ");
        //    }
        //}
        GameManager.instance.player.TakeDamage(monsterStats.attackPower);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(2f); // ������ ���� ���

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
