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

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        // 공격 후에 필요한 다른 동작

        // 공격 후에 다음 턴을 위해 GameManager에 알림
        GameManager.instance.EndMonsterTurn();
    }
}