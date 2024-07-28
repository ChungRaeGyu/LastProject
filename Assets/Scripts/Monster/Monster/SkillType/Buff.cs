using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonsterCharacter
{
    private PlayerStats playerStats;
    //private System.Random random = new System.Random(); // 디버프가 걸릴 확률

    private int buffCounter = 5;  // 디버프 카운트
    private bool buffCounterOnOff = false; // 디버프 활성화

    // GameManager.instance.player.TakeDamage(monsterStats.attackPower); ☆공격을 다 넣고 맨 뒤에 else에 넣기★

    public void FireDotDeal()
    {
        buffCounter--;
        if (buffCounter <= 3) // 2턴에 디버프 시작
        {
            GameManager.instance.player.TakeDamage(5);
            Debug.Log(this.name + " 디버프를 걸었다! " + 5 + " 의 화염 데미지를 입었다!");
            buffCounterOnOff = true;
        }
        
        if (buffCounter <= 1) // 5턴째에 디버프 끝
        {
            buffCounterOnOff = false;
            buffCounter = 5;
        }
    }

    public void SelfDestruct() // 시한폭탄 같은느낌
    {
        buffCounter--;
        if (buffCounter <= 5) // 5턴 뒤에 시작
        {
            if (buffCounter <= 0) // 5턴 안에 잡지못하면 피0 딜30을 넣고 자폭
            {
                monsterStats.maxhealth = 0;
                GameManager.instance.player.TakeDamage(30);
                buffCounter = 5;
            }
        }
    }

    public void StrongAttack() // 3턴동안 공격력의 2배 공격하기
    {
        buffCounter--;
        if (buffCounter <= 5 && !buffCounterOnOff)
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
        }

        if (buffCounter >= 3 && !buffCounterOnOff)
        {
            buffCounterOnOff = true; // 몬스터가 죽으면 false로 바꿔주기
        }
    }

    public void AddAttackPower() // 턴마다 공격력 1씩 상승시키기 
    {
        buffCounter--;
        if(buffCounter <= 5)
        {
            monsterStats.attackPower++; // 몬스터가 죽으면 공격력을 원래대로 돌리기를 어떻게 할지 생각해야할듯
        }
        else if (buffCounter == 0)
        {
            buffCounter = 5;
        }
    }

    public void HealingHp() // 5턴에 한 번씩 체력을 15씩 회복하게됨
    {
        buffCounter--;
        if(buffCounter == 0)
        {
            monsterStats.maxhealth += 15;
        }
    }
}
