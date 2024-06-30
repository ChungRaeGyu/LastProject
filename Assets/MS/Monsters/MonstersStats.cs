using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStats : MonoBehaviour // �ؾ��� �� 1. �÷��̾� ���� 2. ���� ����
{
    [Header("Stats")]  // 3. ���� �� hp�� ���̰� ������ 0 ��Ű�� �ϸ��� ������ ���� �� 4. �ӽ� ����ī�� ����� 8¥��
    public string name;
    public float curHealth;
    public float maxHealth;
    public float defense;
    public float attackPower;
    public float curActionGauge;
    public float maxActionGauge;

    public MonsterStats(string name, int curHealth, int maxHealth, int defense, int attackPower, int curActionGauge, int maxActionGauge)
    {
        this.name = name;
        this.curHealth = curHealth;
        this.maxHealth = maxHealth;
        this.defense = defense;
        this.attackPower = attackPower;
        this.curActionGauge = curActionGauge;
        this.maxActionGauge = maxActionGauge;
    }

    public void Attack(MonsterStats target)
    {
        target.curHealth -= this.attackPower;
        if(target.curHealth < 0) target.curHealth = 0;

        Debug.Log($"{this.name}�� ������ {target.name} �� {this.attackPower} ������ ����.");
    }
    public bool IsAlive()
    {
        return this.curHealth > 0; // heal == 0 is dead
    }

    //public MonsterStats(int health, int defense, int attackPower, float actionGauge) : base(health, defense, attackPower, actionGauge)
    //{

    //}

    //public override void Attack(Character target)
    //{
    //    target.TakeDamage(stats.attackPower);
    //}
}
