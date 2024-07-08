using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlime : Monster
{
    private System.Random random = new System.Random();
    private bool stronger;
    public override IEnumerator MonsterTurn()
    {

        if (random.Next(0, 100) < 15 && !stronger)
        {
            monsterStats.attackPower += 2;
            stronger = true;
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