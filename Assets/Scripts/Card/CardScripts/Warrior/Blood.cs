using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blood : CardBasic
{
    //이름
    //데이터 넣을꺼임
    [Header("CardData")]


    private CardDrag cardDrag;
    private BezierDragLine bezierDragLine;
    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        bezierDragLine = GetComponent<BezierDragLine>();
        cardDrag = GetComponent<CardDrag>();

        SetDescription();
    }

    public override void SetDescription()
    {
        base.SetDescription();

        if (descriptionText != null)
        {
            string color;

            // 초기 ability와 현재 ability 비교
            if (damageAbility > initialDamageAbility)
            {
                color = "#00FF00"; // 초록색
            }
            else if (damageAbility < initialDamageAbility)
            {
                color = "#FF0000"; // 빨간색
            }
            else
            {
                color = ""; // 기본 색
            }

            descriptionText.text = color == ""
                ? $"<b>{damageAbility}</b> 만큼 피해를 줍니다.{utilAbility}의 피를 회복합니다."
                : $"<color={color}><b>{damageAbility}</b></color> 만큼 피해를 줍니다.{utilAbility}의 피를 회복합니다.";
        }
    }

    public override bool TryUseCard()
    {
        MonsterCharacter targetMonster = bezierDragLine.detectedMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            bezierDragLine.DestroyAimingImage();

            GameManager.instance.player.UseCost(cost);

            CardUse(targetMonster);
            if (GameManager.instance.volumeUp)
            {
                CardUse(targetMonster);
                GameManager.instance.volumeUp = false;
            }

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        GameManager.instance.effectManager.PhysicalAttack(this, targetMonster);
        SettingManager.Instance.PlaySound(CardClip1);

        GameManager.instance.player.Heal(utilAbility);
        PlayPlayerAttackAnimation();
    }

    #region 특수카드 사용

    #endregion
    private void PlayPlayerAttackAnimation()
    {
        if (GameManager.instance.player != null && GameManager.instance.player.animator != null)
        {
            GameManager.instance.player.animator.SetTrigger("Attack");
        }
    }

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                damageAbility += 3; // 데미지 증가
                break;
            case 2:
                utilAbility += 3; // 데미지 증가
                break;
            default:
                break;
        }

        SetDescription();
    }
}
