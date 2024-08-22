using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

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

        attackRandomValue = random.Next(6, 10);

        attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{attackRandomValue}</color>의 피해로 공격하려고 합니다.";
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

            yield return PerformAttack(attackRandomValue);
        }

        yield return new WaitForSeconds(monsterTurnDelay); // 연출을 위한 대기

        attackRandomValue = random.Next(6, 10);

        attackDescriptionText.text = $"<color=#FF7F50><size=30><b>공격</b></size></color>\n 이 적은 <color=#FFFF00>{attackRandomValue}</color>의 피해로 공격하려고 합니다.";
    }
}