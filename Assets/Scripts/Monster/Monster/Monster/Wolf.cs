using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private System.Random random = new System.Random();

    private int monsterTurn = 0;
    private bool buffCounterOnOff = false;

    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(10, 20);

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

        monsterTurn++;

        // 부모 클래스의 MonsterTurn을 호출하여 얼리는 효과 적용
        yield return base.MonsterTurn();

        if (!isFrozen)
        {
            yield return new WaitForSeconds(1f); // 연출을 위한 대기

            if (monsterTurn / 2 == 0) // 2턴마다 공격력 1 상승
            {
                monsterStats.attackPower += 1;
            }

            if (monsterTurn / 3 == 0) // 3턴 마다 공격력 2배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
            }

            if (random.Next(0, 100) < 15) // 15% 확률로 공격력 3배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 3);

                Debug.Log(this.name + "이 강한공격!");
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);

                monsterStats.maxhealth += monsterStats.attackPower;
            }

            if (monsterTurn / 3 == 0 && !buffCounterOnOff) // 2턴 뒤 공격력 2배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
                Debug.Log(this.name + "이 강한공격!");

                yield return PerformAttack(5);
                Debug.Log(this.name + " 디버프를 걸었다! " + 5 + " 의 출혈 데미지를 입었다!");
                buffCounterOnOff = true;

                if (monsterTurn <= 4) // 4턴째에 디버프 끝
                {
                    buffCounterOnOff = false;
                }
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