using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonsterCharacter
{
    private PlayerStats playerStats;
    //private System.Random random = new System.Random(); // ������� �ɸ� Ȯ��

    private int buffCounter = 5;  // ����� ī��Ʈ
    private bool buffCounterOnOff = false; // ����� Ȱ��ȭ

    // GameManager.instance.player.TakeDamage(monsterStats.attackPower); �ٰ����� �� �ְ� �� �ڿ� else�� �ֱ��

    public void FireDotDeal()
    {
        buffCounter--;
        if (buffCounter <= 3) // 2�Ͽ� ����� ����
        {
            GameManager.instance.player.TakeDamage(5);
            Debug.Log(this.name + " ������� �ɾ���! " + 5 + " �� ȭ�� �������� �Ծ���!");
            buffCounterOnOff = true;
        }
        
        if (buffCounter <= 1) // 5��°�� ����� ��
        {
            buffCounterOnOff = false;
            buffCounter = 5;
        }
    }

    public void SelfDestruct() // ������ź ��������
    {
        buffCounter--;
        if (buffCounter <= 5) // 5�� �ڿ� ����
        {
            if (buffCounter <= 0) // 5�� �ȿ� �������ϸ� ��0 ��30�� �ְ� ����
            {
                monsterStats.maxhealth = 0;
                GameManager.instance.player.TakeDamage(30);
                buffCounter = 5;
            }
        }
    }

    public void StrongAttack() // 3�ϵ��� ���ݷ��� 2�� �����ϱ�
    {
        buffCounter--;
        if (buffCounter <= 5 && !buffCounterOnOff)
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
        }

        if (buffCounter >= 3 && !buffCounterOnOff)
        {
            buffCounterOnOff = true; // ���Ͱ� ������ false�� �ٲ��ֱ�
        }
    }

    public void AddAttackPower() // �ϸ��� ���ݷ� 1�� ��½�Ű�� 
    {
        buffCounter--;
        if(buffCounter <= 5)
        {
            monsterStats.attackPower++; // ���Ͱ� ������ ���ݷ��� ������� �����⸦ ��� ���� �����ؾ��ҵ�
        }
        else if (buffCounter == 0)
        {
            buffCounter = 5;
        }
    }

    public void HealingHp() // 5�Ͽ� �� ���� ü���� 15�� ȸ���ϰԵ�
    {
        buffCounter--;
        if(buffCounter == 0)
        {
            monsterStats.maxhealth += 15;
        }
    }
}
