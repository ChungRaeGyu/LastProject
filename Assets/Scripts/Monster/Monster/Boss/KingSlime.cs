using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
    private int attackRandomValue;
    private bool bossheal = false;

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

        attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower * 2}</color>의 피해로 공격하려고 합니다.";
    }

    protected override void Update()
    {
        base.Update();

        // 얼면 아무것도 띄우지 않는다.
        if (isFrozen)
        {
            attackDescriptionText.text = "";
        }

        if (currenthealth < monsterStats.maxhealth / 2 && !bossheal)
            util1DescriptionText.text = $"<color=#FF7F50><size=30><b>재생</b></size></color>\n <color=#FFFF00>{30}</color>의 체력을 회복합니다.";
        else
            util1DescriptionText.text = "";
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
        StartCoroutine(Turn());
    }

    public override IEnumerator Turn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        Debug.Log("----- 보스의 " + monsterTurn + "턴 째 -----");
        yield return base.Turn();

        if (!isFrozen)
        {
            monsterNextAction.gameObject.SetActive(false);

            // 행동 이미지에 연출을 줌

            yield return new WaitForSeconds(1f); // 연출을 위한 대기

            if (currenthealth < monsterStats.maxhealth / 2 && !bossheal) // 피 반 이하로 떨어질 때 30 회복 '한 번'만 하기
            {
                currenthealth += 30;
                bossheal = true;
            }

            if (monsterTurn % 3 == 0) // 3턴마다 공격력 2배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
            }
            else if (monsterTurn == 10) // 10턴 뒤 공격력 3배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 3);
            }
            else if (attackRandomValue < 15) // 15% 확률로 공격력 2배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
            }
            else // 기본공격
            {
                yield return PerformAttack(monsterStats.attackPower);
            }
        }

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        monsterTurn++;
        attackRandomValue = random.Next(0, 100);

        if (monsterTurn % 3 == 0)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower * 2}</color>의 피해로 공격하려고 합니다.";
        else if (monsterTurn == 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower * 3}</color>의 피해로 공격하려고 합니다.";
        else if (attackRandomValue < 15)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower * 2}</color>의 피해로 공격하고, <color=#FFFF00>{5}</color>의 출혈 피해를 주려고 합니다.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower}</color>의 피해로 공격하고, {baseAttackPower}만큼 체력이 증가합니다.";

        // 공격 후에 다음 턴을 위해 GameManager에 알림
        GameManager.instance.EndMonsterTurn();
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}
