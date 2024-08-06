using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{    
    //��������ú���
    public int frozenTurnsRemaining = 0; // �� ���°� ������ �� ��
    public int weakerTurnsRemaining = 0; // ��ȭ ���°� ������ �� ��
    public int defDownTurnsRemaining = 0; //��� ���°� ������ �� �� 
    public int burnTurnsRemaining = 0; //ȭ��
    public int poisonTurnsRemaining = 0; //�ߵ� 
    public int bleedingTurnsRemaining = 0; //����

    public bool isFrozen; // ������� Ȯ���ϴ� �뵵

    public List<Condition> conditionInstances = new List<Condition>();

    public Animator animator;

    [Header("DeBuff_InputScript")]
    public GameObject deBuff;
    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public virtual IEnumerator Turn()
    {
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
    private void SpawnConditionText(string conditionText, Vector3 position)
    {
        Color? textColor = conditionText == "����" ? new Color(0.53f, 0.81f, 0.92f) : (Color?)null;
        SpawnText(conditionText, position, textColor);
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
}
