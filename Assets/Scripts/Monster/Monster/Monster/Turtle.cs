using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
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
            monsterNextAction.gameObject.SetActive(false);

            // �ൿ �̹����� ������ ��

            yield return new WaitForSeconds(1f); // ������ ���� ���

            if (random.Next(0, 100) < 15) // 15% Ȯ���� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);

                Debug.Log(this.name + "�� ���Ѱ���!");
            }
            else if (monsterTurn / 3 == 0) // 3�ϸ��� ���� 1 ���
            {
                monsterStats.defense += 1;
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