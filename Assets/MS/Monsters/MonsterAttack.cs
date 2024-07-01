using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    // �����ư��鼭 �����ؾ���
    // �÷��̾ ī�带 �Ἥ �����ϰ� �� ���Ḧ ������ ���Ͱ� �ڵ����� �÷��̾ �����ϰ� �ؾ���
    // �̰Ŵ� ����

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
        // ���� ������ �ִϸ��̼� �ֱ�

        state = State.playerTurn;
    }

    public void PlayerAttackButton() // ���� ��ư(ī��)
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
        Debug.Log("�÷��̾� ����");


        // Ư�� ��ų, ������ �� �ֱ�

        if(!isLive && monsterStats.curHealth <= 0) // ���Ͱ� �׾����� ���� ��
        {
            state = State.win;
            IsDead();
            EndBattle(); 
        }
        else if(monsterStats.curHealth >= 0)// ���Ͱ� ���� �ʾ��� �� �� �ѱ��
        {
            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    void EndBattle()
    {
        Debug.Log("���� ��");
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        // ������ ����, ���� ������ �÷��̾� ��
        state = State.playerTurn;
    }

    public bool IsDead()
    {
        Debug.Log("���");
        return monsterStats.curHealth <= 0;
    }

}
