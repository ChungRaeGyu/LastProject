using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff : MonsterCharacter
{
    private PlayerStats playerStats;
    private System.Random random = new System.Random(); // ������� �ɸ� Ȯ��

    private int deBuffCounter = 5;  // ����� ī��Ʈ
    private bool deBuffCounterOnOff = false; // ����� Ȱ��ȭ

    // GameManager.instance.player.TakeDamage(monsterStats.attackPower); �ٰ����� �� �ְ� �� �ڿ� else�� �ֱ��

    public void PlayerAttackWeak()
    {

    }
}
