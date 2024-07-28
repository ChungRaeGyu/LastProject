using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab을 canvas의 자식으로 생성
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(currenthealth, currenthealth, hpBarPos);
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

        monsterTurn++;

        // 부모 클래스의 MonsterTurn을 호출하여 얼리는 효과 적용
        yield return base.MonsterTurn();

        if (!isFrozen)
        {
            monsterNextAction.gameObject.SetActive(false);

            // 행동 이미지에 연출을 줌

            yield return new WaitForSeconds(1f); // 연출을 위한 대기

            if (monsterTurn >= 0) // 바로 시작
            {
                if (monsterTurn == 5) // 5턴 안에 잡지못하면 피0 딜30을 넣고 자폭
                {
                    monsterStats.maxhealth = 0;
                    yield return PerformAttack(30);
                }
            }
            if (random.Next(0, 100) < 15) // 15% 확률로 공격력 2배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
                Debug.Log(this.name + "이 강한공격!");
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);
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