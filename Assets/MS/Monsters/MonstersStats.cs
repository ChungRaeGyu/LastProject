using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStats : MonoBehaviour // 해야할 것 1. 플레이어 공격 2. 몬스터 공격
{
    public AttackAnim attackAnim;

    [Header("Stats")]  // 3. 공격 시 hp바 까이고 게이지 0 시키고 턴마다 게이지 차는 것 4. 임시 공격카드 만들기 8짜리
    public string name;
    public int curHealth;
    public int maxHealth;
    public int defense;
    public int attackPower;
    public int curActionGauge;
    public int maxActionGauge;

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
    private void Start()
    {
        attackAnim = GetComponent<AttackAnim>();
    }
    private void Attack(MonsterStats target, bool isEnemy = false) // monster special damage
    {
        int damage = this.attackPower;

        attackAnim.MonsterAttackAnim();

        if (isEnemy) // percent change 0.3% twice damage
        {
            float randomValue = Random.value;

            if(randomValue < 0.35f)
            {
                target.curHealth -= damage; // defalut attack
                damage *= 2; // double attack
                Debug.Log($"{this.name} 가 크리티컬 데미지를 입혔다!");
            }
            target.curHealth -= damage; // defalut attack
            Debug.Log($"{this.name} 가 데미지를 입혔다!");
        }
        //target.curHealth -= damage; // defalut attack

        //if (target.curHealth < 0) // - block 
        //{
        //    target.curHealth = 0;
        //}
        // Debug.Log($"{this.name}가 공격함 {target.name} 가 {this.attackPower} 데미지 입음.");
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
