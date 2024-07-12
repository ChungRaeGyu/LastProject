using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff : MonsterCharacter
{
    private PlayerStats playerStats;
    private System.Random random = new System.Random(); // 디버프가 걸릴 확률

    private int deBuffCounter = 5;  // 디버프 카운트
    private bool deBuffCounterOnOff = false; // 디버프 활성화

    // GameManager.instance.player.TakeDamage(monsterStats.attackPower); ☆공격을 다 넣고 맨 뒤에 else에 넣기★

    public void PlayerAttackWeak()
    {

    }
}
