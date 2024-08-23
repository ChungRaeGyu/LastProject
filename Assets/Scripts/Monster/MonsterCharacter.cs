using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MonsterCharacter : Character
{
    public MonsterStats monsterStats;
    public int baseAttackPower;
    public int currenthealth;

    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int Attack = Animator.StringToHash("Attack");

    public Transform conditionPos;
    public Transform hpBarPos; // HP �� ��ġ
    public Transform monsterNextActionPos; // ���� �ൿ�� ��Ÿ�� ��ġ
    public Transform monsterNamePos; // �̸� ��ġ

    private Transform MonsterCondition; //�̰� �� ����
    public Transform monsterNextAction { get; set; }
    private Transform monsterName;
    private Transform monsterNextActionList;

    private AdjustHeightBasedOnText attackDescriptionAdjustHeight;
    protected TMP_Text attackDescriptionText;

    private AdjustHeightBasedOnText util1DescriptionAdjustHeight;
    protected TMP_Text util1DescriptionText;

    private AdjustHeightBasedOnText util2DescriptionAdjustHeight;
    protected TMP_Text util2DescriptionText;
    private float defDownValue;

    public float monsterTurnDelay = 0.5f;

    public bool boss;

    public System.Random random = new System.Random();

    public Action deBuffAnim;

    [Header("Description_NotCashing")]
    public Transform descriptionTransform;
    public GameObject attackDescriptionObject;
    public GameObject defenseDescriptionObject;
    public GameObject healDescriptionObject;
    public bool isDead;
    public bool showActionFrozenAction;

    public void Start()
    {
        currenthealth = monsterStats.maxhealth;
        // ���Ϳ� ������
        if (!boss)
        {
            int hpUp = random.Next(-3, 3);
            currenthealth += hpUp;
        }
        baseAttackPower = monsterStats.attackPower;
        // ConditionBox �������� conditionCanvas�� �ڽ����� �����ϰ� playerCondition�� �Ҵ�
        MonsterCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        AddCondition(MonsterCondition, monsterStats.defense, GameManager.instance.defenseconditionPrefab);

        monsterNextAction = Instantiate(GameManager.instance.attackActionPrefab, UIManager.instance.nextActionIconCanvas.transform).transform;
        monsterNextAction.gameObject.SetActive(false);

        monsterName = Instantiate(GameManager.instance.monsterNamePrefab, UIManager.instance.monsterNameCanvas.transform).transform;

        // TMP_Text ������Ʈ�� ã�Ƽ� ���� �̸� ����
        TMP_Text nameText = monsterName.GetComponentInChildren<TMP_Text>();
        if (nameText != null)
        {
            nameText.text = monsterStats.monsterName;
        }
        
        monsterNextActionList = Instantiate(GameManager.instance.monsterNextActionListPrefab, UIManager.instance.nextActionDescriptionCanvas.transform).transform;

        // ActionDescriptionPrefab�� Description ������Ʈ �ȿ� ����
        descriptionTransform = monsterNextActionList.GetChild(0); // GetChild(0)
        if (descriptionTransform != null)
        {
            // ���� ���� ������
            attackDescriptionObject = Instantiate(GameManager.instance.actionDescriptionPrefab, descriptionTransform);
            attackDescriptionAdjustHeight = attackDescriptionObject.GetComponent<AdjustHeightBasedOnText>();
            attackDescriptionText = attackDescriptionAdjustHeight.childText;
            attackDescriptionText.text = $"";

            // ��� ���� ������
            defenseDescriptionObject = Instantiate(GameManager.instance.actionDescriptionPrefab, descriptionTransform);
            util1DescriptionAdjustHeight = defenseDescriptionObject.GetComponent<AdjustHeightBasedOnText>();
            util1DescriptionText = util1DescriptionAdjustHeight.childText;
            util1DescriptionText.text = $"";

            // ȸ�� ���� ������
            GameObject healDescriptionObject = Instantiate(GameManager.instance.actionDescriptionPrefab, descriptionTransform);
            util2DescriptionAdjustHeight = healDescriptionObject.GetComponent<AdjustHeightBasedOnText>();
            util2DescriptionText = util2DescriptionAdjustHeight.childText;
            util2DescriptionText.text = $"";
        }
    }

    protected virtual void Update()
    {
        monsterNextAction.position = monsterNextActionPos.position;
        monsterName.position = monsterNamePos.position;
        monsterNextActionList.position = transform.position;
        MonsterCondition.position = conditionPos.position;

        //// ��� �ƹ��͵� ����� �ʴ´�.
        //if (isFrozen && showActionFrozenAction)
        //{
        //    showActionFrozenAction = false;
        //    attackDescriptionText.text = "";
        //}
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
        actualDamage = (int)(defDownValue > 0 ? actualDamage * (1 + defDownValue) : actualDamage);
        currenthealth -= actualDamage;
        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        DieAction();
    }


    public void DieAction()
    {
        if (IsDead())
        {
            monsterStats.attackPower = baseAttackPower;
            Die();
            DataManager.Instance.ClearMonstersKilledCount++; // DataManager���� ���� ī��Ʈ ����
            DataManager.Instance.DefeatMonstersKilledCount++; // DataManager���� ���� ī��Ʈ ����

            // ���Ͱ� ���� ���ο� -2���� 2 ������ ���� ���� �߰�
            int randomCoinAdjustment = UnityEngine.Random.Range(-2, 3); // -2���� 2������ �� (3�� ���Ե��� ����)
            int rewardCoin = monsterStats.Coin + randomCoinAdjustment;

            GameManager.instance.monsterTotalRewardCoin += rewardCoin; // �����ϰ� ������ ���� ������ �߰�
        }
    }
    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual void Die()
    {
        if (isFrozen)
            GameManager.instance.DestroyDeBuffAnim(deBuff);

        //�̸�, �����ൿ �׼� �����
        if (monsterNextAction != null)
        {
            monsterNextAction.gameObject.SetActive(false);
        }

        if (monsterName != null)
        {
            monsterName.gameObject.SetActive(false);
        }

        if (monsterNextActionList != null)
        {
            monsterNextActionList.gameObject.SetActive(false);
        }

        isDead = true;
        gameObject.SetActive(false);
        GameManager.instance.CheckAllMonstersDead();
    }
    public override void TakedamageCharacter(int damage)
    {
        TakeDamage(damage);
    }

    public override void BaseWeakerMethod()
    {
        monsterStats.attackPower = baseAttackPower;
    }
    public override void WeakingMethod(float ability)
    {
        Debug.Log("��ȭ");
        monsterStats.attackPower = (int)(monsterStats.attackPower * (1 - ability));
    }
    public override void BasedefMethod()
    {
        defDownValue = 0;
    }
    public override void DefDownValue(float ability)
    {
        Debug.Log("���");
        defDownValue = ability;
    }


    protected IEnumerator PerformAttack(int damage, float attackDelay = 1.2f)
    {
        // �ִϸ��̼� Ʈ����
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(0.3f);

        // �÷��̾� �´� �Ҹ�
        SettingManager.Instance.PlaySound(GameManager.instance.BaseAttackClip);
        Instantiate(GameManager.instance.hitEffect, GameManager.instance.player.transform.position, Quaternion.identity);

        GameManager.instance.player.TakeDamage(damage);

        //��� �ð�
        yield return new WaitForSeconds(attackDelay);
    }
    protected override Transform GetConditionPos()
    {
        return MonsterCondition;
    }
    protected override Transform GetConditionTransfrom()
    {
        return conditionPos;
    }

    #region �Ⱦ��� ��
    // ����Ʈ���� Condition �ν��Ͻ��� �����ϰ� ��ġ�� ������Ʈ
    public void RemoveCondition(Condition condition)
    {
        //���� ��
        if (conditionInstances.Contains(condition))
        {
            conditionInstances.Remove(condition);
            Destroy(condition.gameObject);
            //UpdateConditionPositions();
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

    public void UpdateConditions()
    {
        foreach (var condition in conditionInstances)
        {
            //Condition ������Ʈ ���� ����
        }
    }
    #endregion
}
