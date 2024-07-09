using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;
    private System.Random random = new System.Random();
    private float dotDealCount = 0;
    private bool dotDealOnOff = false;

    private void Start()
    {
        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab을 canvas의 자식으로 생성
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth, transform.GetChild(1));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (healthBarInstance != null)
        {
            healthBarInstance.ResetHealthSlider(currenthealth);
            healthBarInstance.UpdatehealthText();
        }
    }

    public void StartMonsterTurn()
    {
        StartCoroutine(MonsterTurn());
    }

    public IEnumerator MonsterTurn()
    {
        if (dotDealCount <= 2 && !dotDealOnOff) // 시작하자마자 도트딜을 걸어버림
        {
            dotDealCount++;
            GameManager.instance.player.TakeDamage(monsterStats.attackPower / 3); //.공격력의 3/1 도트데미지
            if (dotDealCount >= 3) // 3턴동안만 건다
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

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        // 공격 후에 필요한 다른 동작

        // 공격 후에 다음 턴을 위해 GameManager에 알림
        GameManager.instance.EndMonsterTurn();
    }
}