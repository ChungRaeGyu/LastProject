using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterCharacter : MonoBehaviour
{
    public MonsterStats monsterStats;
    public int currenthealth { get; private set; }
    public Animator animator;

    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int Attack = Animator.StringToHash("Attack");

    private List<Condition> conditionInstances = new List<Condition>();

    public Transform hpBarPos; // HP �� ��ġ
    public Transform conditionPos; // ����� ��ġ

    public TMP_Text monsterName;

    [HideInInspector]
    public Transform MonsterCondition;
    public int frozenTurnsRemaining = 0; // �� ���°� ������ �� ��

    public bool isFrozen; // ������� Ȯ���ϴ� �뵵


    [Header("DeBuff_InputScript")]
    public GameObject deBuff;

    public Action deBuffAnim;
    private void Awake()
    {
        if (monsterStats == null)
        {
            Debug.Log("MonsterStats�� " + gameObject.name + "�� �Ҵ���� �ʾҴ�.");
        }

        currenthealth = monsterStats.maxhealth;

        animator = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        // ConditionBox �������� conditionCanvas�� �ڽ����� �����ϰ� playerCondition�� �Ҵ�
        MonsterCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        AddCondition(MonsterCondition, monsterStats.defense, GameManager.instance.defenseconditionPrefab, ConditionType.Defense);
    }

    private void Update()
    {
        MonsterCondition.position = conditionPos.position;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
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
        if(isFrozen)
            GameManager.instance.DeBuffAnim(deBuff);
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
            GameManager.instance.EndMonsterTurn(); // ���� �� ���� �˸�
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount();
            }
            if (frozenTurnsRemaining == 0)
            {
                animator.StopPlayback();
                GameManager.instance.DeBuffAnim(deBuff); //����������Ʈ ���� �ϴ� ��
            }
            yield break;
        }
        else
        {
            isFrozen = false;
        }

        yield return null;
    }

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
            // Condition ������Ʈ ���� ����
        }
    }

    public void DieAction()
    {
        if (IsDead())
        {
            Die();
            DataManager.Instance.IncreaseMonstersKilledCount(); // DataManager���� ���� ī��Ʈ ����
        }
    }
}
