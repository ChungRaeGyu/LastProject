using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private System.Random random = new System.Random();

    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(0, 10);

            // healthBarPrefab을 canvas의 자식으로 생성
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth + hpUp, monsterStats.maxhealth + hpUp, transform.GetChild(1));
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

    public override IEnumerator MonsterTurn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        // 부모 클래스의 MonsterTurn을 호출하여 얼리는 효과 적용
        yield return base.MonsterTurn();

        if (!isFrozen)
        {
            yield return new WaitForSeconds(1f); // 연출을 위한 대기

            if (random.Next(0, 100) < 15) // 15% 확률로 공격력 2배 공격
            {
                GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
                Debug.Log(this.name + "이 강한공격!");
            }
            else
            {
                GameManager.instance.player.TakeDamage(monsterStats.attackPower);
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
            }
        }

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        GameManager.instance.EndMonsterTurn();
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}