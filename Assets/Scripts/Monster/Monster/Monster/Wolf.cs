using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private System.Random random = new System.Random();

    private int monsterTurn = 0;
    private bool buffCounterOnOff = false;

    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(10, 20);

            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth + hpUp, monsterStats.maxhealth + hpUp, transform.GetChild(1));
        }
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
        if (GameManager.instance.player?.IsDead() == true) yield break;

        monsterTurn++;

        // �θ� Ŭ������ MonsterTurn�� ȣ���Ͽ� �󸮴� ȿ�� ����
        yield return base.MonsterTurn();

        if (!isFrozen)
        {
            yield return new WaitForSeconds(1f); // ������ ���� ���

            if (monsterTurn / 2 == 0) // 2�ϸ��� ���ݷ� 1 ���
            {
                monsterStats.attackPower += 1;
            }

            if (monsterTurn / 3 == 0) // 3�� ���� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
            }

            if (random.Next(0, 100) < 15) // 15% Ȯ���� ���ݷ� 3�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 3);

                Debug.Log(this.name + "�� ���Ѱ���!");
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);

                monsterStats.maxhealth += monsterStats.attackPower;
            }

            if (monsterTurn / 3 == 0 && !buffCounterOnOff) // 2�� �� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
                Debug.Log(this.name + "�� ���Ѱ���!");

                yield return PerformAttack(5);
                Debug.Log(this.name + " ������� �ɾ���! " + 5 + " �� ���� �������� �Ծ���!");
                buffCounterOnOff = true;

                if (monsterTurn <= 4) // 4��°�� ����� ��
                {
                    buffCounterOnOff = false;
                }
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);
            }
        }

        yield return new WaitForSeconds(1f); // ������ ���� ���

        GameManager.instance.EndMonsterTurn();
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}