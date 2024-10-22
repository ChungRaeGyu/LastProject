using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonsterCharacter
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

        util1DescriptionText.text = $"<color=#FF7F50><size=30><b>발톱</b></size></color>\n <color=#FFFF00>2</color>턴마다 공격력이 <color=#FFFF00>1</color>씩 증가합니다.";

        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{Mathf.FloorToInt(monsterStats.attackPower * 1.5f)}</color>의 피해로 공격하고, <color=#FFFF00>{2}</color>의 출혈 피해를 주려고 합니다.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower}</color>의 피해로 공격하고 , {baseAttackPower / 4}만큼 체력이 증가합니다.";
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
            monsterNextAction.gameObject.SetActive(false);

            // 행동 이미지에 연출을 줌

            yield return new WaitForSeconds(monsterTurnDelay); // 연출을 위한 대기

            if (monsterTurn % 2 == 0) // 2턴마다 공격력 1 상승
            {
                monsterStats.attackPower += 1;
            }

            if (attackRandomValue < 10) // 10% 확률로 공격력 3배 공격
            {
                yield return PerformAttack(Mathf.FloorToInt(monsterStats.attackPower * 1.5f));
                GameManager.instance.player.BleedingForTurns(2);

                Debug.Log(this.name + "이 강한공격!");
                Debug.Log(this.name + " 디버프를 걸었다! " + 5 + " 의 출혈 데미지를 입었다!");
            }
            else // 공격력 만큼 피가 회복이 됨
            {
                yield return PerformAttack(monsterStats.attackPower);
                currenthealth += baseAttackPower / 4;
            }
        }

        yield return new WaitForSeconds(monsterTurnDelay); // 연출을 위한 대기

        monsterTurn++;
        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{Mathf.FloorToInt(monsterStats.attackPower * 1.2f)}</color>의 피해로 공격하고, <color=#FFFF00>{2}</color>의 출혈 피해를 주려고 합니다.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower}</color>의 피해로 공격하고 , {baseAttackPower / 4}만큼 체력이 증가합니다.";
    }

}