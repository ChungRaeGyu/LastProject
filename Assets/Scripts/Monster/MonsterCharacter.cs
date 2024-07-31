using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterCharacter : MonoBehaviour
{
    public MonsterStats monsterStats;
    public int baseAttackPower;
    public int currenthealth;

    public Animator animator;

    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int Attack = Animator.StringToHash("Attack");

    public List<Condition> conditionInstances = new List<Condition>();

    public Transform hpBarPos; // HP �� ��ġ
    public Transform conditionPos; // ����� ��ġ
    public Transform monsterNextActionPos; // ���� �ൿ�� ��Ÿ�� ��ġ
    public Transform monsterNamePos; // �̸� ��ġ

    private Transform MonsterCondition;
    public Transform monsterNextAction { get; set; }
    private Transform monsterName;
    private Transform monsterNextActionList;

    private AdjustHeightBasedOnText attackDescriptionAdjustHeight;
    protected TMP_Text attackDescriptionText;

    private AdjustHeightBasedOnText util1DescriptionAdjustHeight;
    protected TMP_Text util1DescriptionText;

    private AdjustHeightBasedOnText util2DescriptionAdjustHeight;
    protected TMP_Text util2DescriptionText;

    //��������ú���
    public int frozenTurnsRemaining = 0; // �� ���°� ������ �� ��
    public int weakerTurnsRemaining = 0; // ��ȭ ���°� ������ �� ��
    public int defDownTurnsRemaining = 0; //��� ���°� ������ �� �� 
    public int burnTurnsRemaining = 0; //ȭ��
    public int poisonTurnsRemaining = 0; //�ߵ� 
    public int bleedingTurnsRemaining = 0; //����

    private float defDownValue;
    public bool isFrozen; // ������� Ȯ���ϴ� �뵵

    public bool boss;

    public System.Random random = new System.Random();

    [Header("DeBuff_InputScript")]
    public GameObject deBuff;

    public Action deBuffAnim;

    [Header("Description_NotCashing")]
    public Transform descriptionTransform;
    public GameObject attackDescriptionObject;
    public GameObject defenseDescriptionObject;
    public GameObject healDescriptionObject;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        currenthealth = monsterStats.maxhealth;
        // ���Ϳ� ������
        if (!boss)
        {
            int hpUp = random.Next(0, 10);
            currenthealth += hpUp;
        }
        baseAttackPower = monsterStats.attackPower;
        // ConditionBox �������� conditionCanvas�� �ڽ����� �����ϰ� playerCondition�� �Ҵ�
        MonsterCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        AddCondition(MonsterCondition, monsterStats.defense, GameManager.instance.defenseconditionPrefab, ConditionType.Defense);

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
        MonsterCondition.position = conditionPos.position;
        monsterNextAction.position = monsterNextActionPos.position;
        monsterName.position = monsterNamePos.position;
        monsterNextActionList.position = transform.position;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
        actualDamage = (int)(defDownTurnsRemaining > 0 ? actualDamage * (1 + defDownValue) : actualDamage);
        currenthealth -= actualDamage;

        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        DieAction();
    }

    private void SpawnText(string text, Vector3 position, Color? color = null)
    {
        if (GameManager.instance.damageTextPrefab != null)
        {
            GameObject textInstance = Instantiate(GameManager.instance.damageTextPrefab, position, Quaternion.identity);
            DamageText damageText = textInstance.GetComponent<DamageText>();
            damageText.SetText(text);

            // ���� ����
            if (color.HasValue)
            {
                damageText.currentColor = color.Value;
            }

            // ȭ�� ��ǥ���� ���� �̵�
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            float yOffset = 200f; // �󸶳� ���� ��ġ���� ����
            Vector3 newScreenPosition = new Vector3(screenPosition.x, screenPosition.y + yOffset, 10f);
            textInstance.transform.position = Camera.main.ScreenToWorldPoint(newScreenPosition);
        }
    }

    private void SpawnDamageText(int damageAmount, Vector3 position)
    {
        SpawnText(damageAmount.ToString(), position);
    }

    private void SpawnConditionText(string conditionText, Vector3 position)
    {
        Color? textColor = conditionText == "����" ? new Color(0.53f, 0.81f, 0.92f) : (Color?)null;
        SpawnText(conditionText, position, textColor);
    }


    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual void Die()
    {
        if (isFrozen)
            GameManager.instance.DestroyDeBuffAnim(deBuff);

        if (monsterNextAction != null)
        {
            Destroy(monsterNextAction.gameObject);
        }

        if (monsterName != null)
        {
            Destroy(monsterName.gameObject);
        }

        if (monsterNextActionList = null)
        {
            Destroy(monsterNextActionList.gameObject);
        }

        Destroy(gameObject);
    }

    public virtual IEnumerator MonsterTurn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        if (frozenTurnsRemaining > 0)
        {
            frozenTurnsRemaining--;
            Debug.Log($"{gameObject.name}�� ����ֽ��ϴ�. ���� �� �� ��: {frozenTurnsRemaining}");

            // SpawnDamageText�� "����" �ؽ�Ʈ ��쵵�� ����
            SpawnConditionText("����", transform.position);

            yield return new WaitForSeconds(2f); // ������ ���� ���
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            if (frozenTurnsRemaining == 0)
            {
                animator.StopPlayback();
                GameManager.instance.DestroyDeBuffAnim(deBuff); //����������Ʈ ���� �ϴ� ��
            }
            yield break;
        }
        else
        {
            isFrozen = false;
        }
        if (weakerTurnsRemaining > 0)
        {
            weakerTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Weaker);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            yield break;
        }
        else
        {
            monsterStats.attackPower = baseAttackPower;
        }
        if (defDownTurnsRemaining > 0)
        {
            weakerTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.DefDown);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            yield break;
        }
        if (burnTurnsRemaining > 0)
        {
            burnTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Burn);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(3);
        }
        if (poisonTurnsRemaining > 0)
        {
            poisonTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Poison);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(5);
        }
        if (bleedingTurnsRemaining > 0)
        {
            bleedingTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Bleeding);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(5);
        }
        yield return null;
    }

    protected IEnumerator PerformAttack(int damage, float attackDelay = 1.2f)
    {
        // �ִϸ��̼� Ʈ����
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(0.3f);

        GameManager.instance.player.TakeDamage(damage);

        //��� �ð�
        yield return new WaitForSeconds(attackDelay);
    }

    #region �����
    public virtual void FreezeForTurns(int turns)
    {
        isFrozen = true;
        frozenTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.frozenConditionPrefab, ConditionType.Frozen);
        }
        Debug.Log($"{gameObject.name}�� {turns}�� ���� ��Ƚ��ϴ�. ���� �� �� ��: {frozenTurnsRemaining}");
    }
    public void WeakForTurns(int turns, float ability)
    {
        //��ȭ : ������ ���ݷ��� ��������.
        weakerTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Weaker);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.weakerConditionPrefab, ConditionType.Weaker);
            //��ȭ 
            monsterStats.attackPower = (int)(monsterStats.attackPower * (1 - ability));
        }
    }
    public void DefDownForTurns(int turns, float ability)
    {
        //��� : ������ ������ ��������.
        defDownTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.DefDown);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.defDownConditionPrefab, ConditionType.DefDown);
            defDownValue = ability;
        }
    }

    public void burnForTunrs(int turns)
    {
        //��Ʈ ��
        burnTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Burn);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.burnConditionPrefab, ConditionType.Burn);
        }

    }
    public void PoisonForTunrs(int turns)
    {
        //��Ʈ ��
        burnTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Poison);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.bleedingConditioinPrefab, ConditionType.Poison);
        }

    }
    public void BleedingForTunrs(int turns)
    {
        //��Ʈ ��
        burnTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Bleeding);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.poisonConditionPrefab, ConditionType.Bleeding);
        }

    }

    #endregion


    // ���ο� Condition �ν��Ͻ��� �����ϰ� ����Ʈ�� �߰��� ��, ��ġ�� ������Ʈ
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab, ConditionType type)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, conditionPos, type); // ��ġ �ʱ�ȭ �Ŀ� ���� �� ����
        }
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
    public void DieAction()
    {
        if (IsDead())
        {
            monsterStats.attackPower = baseAttackPower;
            Die();
            DataManager.Instance.monstersKilledCount++; // DataManager���� ���� ī��Ʈ ����

            // ���Ͱ� ���� ���ο� -2���� 2 ������ ���� ���� �߰�
            int randomCoinAdjustment = UnityEngine.Random.Range(-2, 3); // -2���� 2������ �� (3�� ���Ե��� ����)
            int rewardCoin = monsterStats.Coin + randomCoinAdjustment;

            GameManager.instance.monsterTotalRewardCoin += rewardCoin; // �����ϰ� ������ ���� ������ �߰�
        }
    }
}
