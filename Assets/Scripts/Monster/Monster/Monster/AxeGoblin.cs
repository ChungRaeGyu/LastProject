using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeGoblin : MonsterCharacter
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

    protected override void Update()
    {
        base.Update();

        // 얼면 아무것도 띄우지 않는다.
        if (isFrozen)
        {
            attackDescriptionText.text = "";
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
            monsterNextAction.gameObject.SetActive(false);

            // 행동 이미지에 연출을 줌

            yield return new WaitForSeconds(1f); // 연출을 위한 대기

            if (monsterTurn % 2 == 0) // 2턴 마다 공격력 2배 공격
            {
                yield return PerformAttack(monsterStats.attackPower * 2);
                Debug.Log(this.name + "이 강한공격!");
                yield return PerformAttack(5);
                Debug.Log(this.name + " 디버프를 걸었다! " + 5 + " 의 출혈 데미지를 입었다!");
                //attackDescriptionObject.SetActive(false);
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);
            }
        }

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        monsterTurn++;

        if (monsterTurn % 2 == 0)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower * 2}</color>의 피해로 공격하고, <color=#FFFF00>{5}</color>의 출혈 피해를 주려고 합니다.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{monsterStats.attackPower}</color>의 피해로 공격하려고 합니다.";

        GameManager.instance.EndMonsterTurn();
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}