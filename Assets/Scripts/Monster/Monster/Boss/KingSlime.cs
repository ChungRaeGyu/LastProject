using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;
    private int bossTurnCount = 0;
    private bool strongAttack = false;
    private System.Random random = new System.Random();

    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth, monsterStats.maxhealth, transform.GetChild(1));
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

        bossTurnCount++;
        Debug.Log("----- ������ " + bossTurnCount + "�� ° -----");
        yield return base.MonsterTurn();

        if (!isFrozen)
        {
            yield return new WaitForSeconds(1f); // ������ ���� ���

            if (monsterStats.maxhealth < monsterStats.maxhealth / 2) // �� �� ���Ϸ� ������ �� 30 ȸ�� '�� ��'�� �ϱ�
            {
                monsterStats.maxhealth += 30;
                Debug.Log(this.name + "��" + 30 + "��ŭ ȸ���ߴ�!");
                yield return new WaitForSeconds(1f);
            }

            if (bossTurnCount <= 4 && !strongAttack) // 3�ϵ��� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);

                strongAttack = true;
                Debug.Log(this.name + "�ʹ� ����" + monsterStats.attackPower * 2 + "������");
                yield return new WaitForSeconds(1f);
            }

            else if (bossTurnCount % 10 == 0) // 10�� �� ���ݷ� 3�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 3);

                Debug.Log(this.name + "�� ���� ������ �ߴ�!" + monsterStats.attackPower * 3 + "������");
                yield return new WaitForSeconds(1f);
            }

            else if (random.Next(0, 100) < 15) // 15% Ȯ���� ���ݷ� 2�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 2);

                Debug.Log(this.name + "�� ���� Ȯ���� ���Ѱ���!");
                yield return new WaitForSeconds(1f);
            }

            else // �⺻����
            {
                yield return PerformAttack(monsterStats.attackPower);

                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(1f); // ������ ���� ���

        // ���� �Ŀ� �ʿ��� �ٸ� ����

        // ���� �Ŀ� ���� ���� ���� GameManager�� �˸�
        GameManager.instance.EndMonsterTurn();
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}
