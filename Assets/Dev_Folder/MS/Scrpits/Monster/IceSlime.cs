using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlime : Monster
{
    private System.Random random = new System.Random();
    private float dotDealCount = 0;
    private bool dotDealOnOff = false;
    public override IEnumerator MonsterTurn()
    {
        if (dotDealCount <= 2 && !dotDealOnOff) // �������ڸ��� ��Ʈ���� �ɾ����
        {
            dotDealCount++;
            GameManager.instance.player.TakeDamage(monsterStats.attackPower / 3); //.���ݷ��� 3/1 ��Ʈ������
            if (dotDealCount >= 3) // 3�ϵ��ȸ� �Ǵ�
            {
                dotDealOnOff = true;
            }
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