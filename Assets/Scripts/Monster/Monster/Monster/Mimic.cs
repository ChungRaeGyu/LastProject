using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
    private int attackRandomValue;
    private int selfdestructionCount = 5;

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

        attackRandomValue = Random.Range(0, 100);

        if (monsterTurn <= 5)
        {
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n {selfdestructionCount}�� �Ŀ� �� ���� <color=#FFFF00>{30}</color>�� ���ط� �����ϰ� ������ϴ�.";
        }

        if (attackRandomValue < 15)
        {
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 2}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        }
        else
        {
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        }
    }

    protected override void Update()
    {
        base.Update();
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

        // �θ� Ŭ������ MonsterTurn�� ȣ���Ͽ� �󸮴� ȿ�� ����
        yield return base.Turn();

        if (!isFrozen)
        {
            if (isDead) yield break;
            monsterNextAction.gameObject.SetActive(false);

            // �ൿ �̹����� ������ ��

            yield return new WaitForSeconds(monsterTurnDelay); // ������ ���� ���

            if (monsterTurn == 5) // 5�� �ȿ� �������ϸ� ��0 ��30�� �ְ� ����
            {
                monsterStats.maxhealth = 0;
                yield return PerformAttack(30);
            }

            if (attackRandomValue < 15) // 15% Ȯ���� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
                Debug.Log(this.name + "�� ���Ѱ���!");
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);
            }
        }

        yield return new WaitForSeconds(monsterTurnDelay); // ������ ���� ���

        monsterTurn++;
        selfdestructionCount -= 1;
        attackRandomValue = Random.Range(0, 100);

        if (monsterTurn <= 5)
        {
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n {selfdestructionCount}�� �Ŀ� �� ���� <color=#FFFF00>{30}</color>�� ���ط� �����ϰ� ������ϴ�.";
        }

        if (attackRandomValue < 15)
        {
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 2}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        }
        else
        {
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        }
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}