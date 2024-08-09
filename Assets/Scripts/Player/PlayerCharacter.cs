using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PlayerCharacter : Character
{
    public PlayerStats playerStats;
    public int currenthealth;
    public int currentDefense;
    public int currentAttack;
    public float defdown;
    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int attack = Animator.StringToHash("Attack");
    private static readonly int die = Animator.StringToHash("Die");
    protected bool isDying = false; // 죽는 중인지 여부를 저장하는 변수

    [SerializeField] GameObject damagedEffect;

    protected virtual void Start()
    {
        currentAttack = playerStats.attack;
        currentDefense = playerStats.defense;
        currenthealth = playerStats.maxhealth;
    }

    public virtual void InitializeStats(int currenthealthData)
    {
        currenthealth = currenthealthData;
    }

    public virtual int SavePlayerStats()
    {
        return currenthealth;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - currentDefense, 0);
        actualDamage = (int)(defDownTurnsRemaining > 0 ? actualDamage * (1 + defdown) : actualDamage);
        currenthealth -= actualDamage;

        GameManager.instance.ShakeCamera();
        Instantiate(damagedEffect, transform.position, Quaternion.identity);
        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        if (IsDead())
        {
            StartCoroutine(Die());
        }
    }

    public virtual void Heal(int amount)
    {
        currenthealth += amount;

        SpawnDamageText(amount, transform.position);
    }

    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual IEnumerator Die()
    {
        if (isDying) yield break; // 이미 죽는 중이면 중복 실행을 막음
        isDying = true;

        if (animator != null)
        {
            animator.SetTrigger(die);
        }

        // 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 결과패널(패배)를 보여줌
        UIManager.instance.ShowDefeatPanel();
        //UIManager.instance.ApplyDeathPenalty(); //카드 랜덤 삭제 일단 보류
        //덱 초기화는 LobbyManager에 있습니다.
    }

    protected override void TakedamageCharacter(int damage)
    {
        //지속 딜을 위한
        TakeDamage(damage);
    }

    protected override void BaseWeakerMethod()
    {
        currentAttack = playerStats.attack;
    }
    protected override void WeakingMethod(float ability)
    {
        currentAttack = (int)(playerStats.attack * (1 - ability));
    }
    protected override void BasedefMethod()
    {
        defdown = 0;
    }
    protected override void DefDownValue(float ability)
    {
        defdown = ability;
    }
}
