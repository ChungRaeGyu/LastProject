using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
    private int attackRandomValue;

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

        if (monsterTurn % 4 == 0)
            util1DescriptionText.text = $"<color=#FF7F50><size=30><b>껍질</b></size></color>\n <color=#FFFF00>5</color>턴마다 방어력이 <color=#FFFF00>{monsterStats.defense += 1}</color>씩 증가합니다.";

        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 15)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower * 2}</color>의 피해로 공격하려고 합니다.";
        else if (currenthealth < 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 현재 방어력을 모두 소모해 <color=#FFFF00>{monsterStats.defense *= monsterStats.attackPower}</color>의 피해로 공격하려고 합니다.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower}</color>의 피해로 공격하려고 합니다.";
    }

    protected override void Update()
    {
        base.Update();
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

        // 부모 클래스의 MonsterTurn을 호출하여 얼리는 효과 적용
        yield return base.Turn();

        if (!isFrozen)
        {
            if (isDead) yield break;
            monsterStats.defense = 0;
            monsterNextAction.gameObject.SetActive(false);

            // 행동 이미지에 연출을 줌

            yield return new WaitForSeconds(monsterTurnDelay); // 연출을 위한 대기

            if (monsterTurn % 5 == 0) // 5턴마다 방어력 1 상승
            {
                monsterStats.defense += 1;
            }

            if (attackRandomValue < 15) // 15% 확률로 공격력 2배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 2);

                Debug.Log(this.name + "이 강한공격!");
            }
            else if (currenthealth > 10) // 쌓인 방어도 만큼 기본 공격력을 곱해 공격한다 (최후의 발악 느낌)
            {
                monsterStats.defense *= monsterStats.attackPower;
                monsterStats.defense = 0;
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);
            }
        }

        yield return new WaitForSeconds(monsterTurnDelay); // 연출을 위한 대기

        monsterTurn++;
        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 15)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower * 2}</color>의 피해로 공격하려고 합니다.";
        else if (currenthealth < 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 현재 방어력을 모두 소모해 <color=#FFFF00>{monsterStats.defense *= monsterStats.attackPower}</color>의 피해로 공격하려고 합니다.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower}</color>의 피해로 공격하려고 합니다.";
    }

}