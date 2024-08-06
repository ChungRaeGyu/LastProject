using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
    private int attackRandomValue;
    private bool bossheal = false;

    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(currenthealth, currenthealth, hpBarPos);
        }

        attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 2}</color>�� ���ط� �����Ϸ��� �մϴ�.";
    }

    protected override void Update()
    {
        base.Update();

        // ��� �ƹ��͵� ����� �ʴ´�.
        if (isFrozen)
        {
            attackDescriptionText.text = "";
        }

        if (currenthealth < monsterStats.maxhealth / 2 && !bossheal)
            util1DescriptionText.text = $"<color=#FF7F50><size=30><b>���</b></size></color>\n <color=#FFFF00>{30}</color>�� ü���� ȸ���մϴ�.";
        else
            util1DescriptionText.text = "";
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
        StartCoroutine(Turn());
    }

    public override IEnumerator Turn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        Debug.Log("----- ������ " + monsterTurn + "�� ° -----");
        yield return base.Turn();

        if (!isFrozen)
        {
            monsterNextAction.gameObject.SetActive(false);

            // �ൿ �̹����� ������ ��

            yield return new WaitForSeconds(1f); // ������ ���� ���

            if (currenthealth < monsterStats.maxhealth / 2 && !bossheal) // �� �� ���Ϸ� ������ �� 30 ȸ�� '�� ��'�� �ϱ�
            {
                currenthealth += 30;
                bossheal = true;
            }

            if (monsterTurn % 3 == 0) // 3�ϸ��� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
            }
            else if (monsterTurn == 10) // 10�� �� ���ݷ� 3�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 3);
            }
            else if (attackRandomValue < 15) // 15% Ȯ���� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
            }
            else // �⺻����
            {
                yield return PerformAttack(monsterStats.attackPower);
            }
        }

        yield return new WaitForSeconds(1f); // ������ ���� ���

        monsterTurn++;
        attackRandomValue = random.Next(0, 100);

        if (monsterTurn % 3 == 0)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 2}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        else if (monsterTurn == 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 3}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        else if (attackRandomValue < 15)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 2}</color>�� ���ط� �����ϰ�, <color=#FFFF00>{5}</color>�� ���� ���ظ� �ַ��� �մϴ�.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����ϰ�, {baseAttackPower}��ŭ ü���� �����մϴ�.";

        // ���� �Ŀ� ���� ���� ���� GameManager�� �˸�
        GameManager.instance.EndMonsterTurn();
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}
