using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlime : Monster
{
    private System.Random random = new System.Random();
    private bool harder;

    public override IEnumerator MonsterTurn()
    {
        if (random.Next(0, 100) < 15 && !harder)
        {
            monsterStats.defense += 2;
            harder = true;
        }
        else
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower);
        }
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(1f); // ������ ���� ���

        // ���� �Ŀ� �ʿ��� �ٸ� ����

        // ���� �Ŀ� ���� ���� ���� GameManager�� �˸�
        GameManager.instance.EndMonsterTurn();
    }
}