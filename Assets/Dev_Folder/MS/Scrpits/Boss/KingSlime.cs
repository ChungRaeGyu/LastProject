using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : Monster
{
    private int bossTurnCount = 0;
    private bool bossHeal = false;
    private bool strongAttack = false;
    private System.Random random = new System.Random();

    public override IEnumerator MonsterTurn()
    {
        bossTurnCount++;
        Debug.Log("----- ������ " + bossTurnCount + "�� ° -----");
        if (monsterStats.maxhealth < 50 && !bossHeal) // �� 50 ���Ϸ� ������ �� 30 ȸ�� '�� ��'�� �ϱ�
        {
            monsterStats.maxhealth += 30;
            bossHeal = true;
            Debug.Log(this.name + "��" + 30 + "��ŭ ȸ���ߴ�!");
        }

        if (bossTurnCount <= 4) // 3�ϵ��� ���ݷ� 2�� ����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
            strongAttack = true;
            Debug.Log(this.name + "�ʹ� ����" + monsterStats.attackPower * 2 + "������");
        }

        else if (bossTurnCount % 10 == 0) // 10�� �� Ȯ���� ���ݷ� 2�� ����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 3);
            Debug.Log(this.name + "�� ���� ������ �ߴ�!" + monsterStats.attackPower * 3 + "������");
        }

        else if (random.Next(0, 100) < 15) // 15% Ȯ���� ���ݷ� 2�� ����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
            Debug.Log(this.name + "�� ���� Ȯ���� ���Ѱ���!");
        }

        else // �⺻����
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower);
        }
        yield return new WaitForSeconds(1f); // ������ ���� ���

        if (animator != null) // �ִϸ��̼�
        {
            animator.SetTrigger("Attack");
        }
        // ���� �Ŀ� �ʿ��� �ٸ� ����

        // ���� �Ŀ� ���� ���� ���� GameManager�� �˸�
        GameManager.instance.EndMonsterTurn();
    }
}
