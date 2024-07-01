using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    // 번갈아가면서 공격해야함
    // 플레이어가 카드를 써서 공격하고 턴 종료를 누르면 몬스터가 자동으로 플레이어를 공격하게 해야함
    // 이거는 보류

    public enum State
    {
        start,
        playerTurn,
        enemyTurn,
        win
    }

    private MonStats monsterStats;
    public State state;
    public bool isLive;

    void Awake()
    {
        state = State.start;
        BattleStart();
    }

    void BattleStart()
    {
        // 적을 때리는 애니메이션 넣기

        state = State.playerTurn;
    }

    public void PlayerAttackButton() // 공격 버튼(카드)
    {
        if(state != State.playerTurn)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("플레이어 공격");


        // 특수 스킬, 데미지 등 넣기

        if(!isLive && monsterStats.curHealth <= 0) // 몬스터가 죽었으면 전투 끝
        {
            state = State.win;
            IsDead();
            EndBattle(); 
        }
        else if(monsterStats.curHealth >= 0)// 몬스터가 죽지 않았을 때 턴 넘기기
        {
            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    void EndBattle()
    {
        Debug.Log("전투 끝");
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        // 몬스터의 공격, 공격 끝나면 플레이어 턴
        state = State.playerTurn;
    }

    public bool IsDead()
    {
        Debug.Log("사망");
        return monsterStats.curHealth <= 0;
    }

}
