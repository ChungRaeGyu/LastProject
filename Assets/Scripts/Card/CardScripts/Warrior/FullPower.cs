using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FullPower: CardBasic
{
    private BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        bezierDragLine = GetComponent<BezierDragLine>();

        SetDescription();

        costText.text = "X";
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
                ? $"<b>{damageAbility}</b> X 현재코스트만큼 피해를 줍니다."
                : $"<color={color}><b>{damageAbility}</b></color> X 현재코스트만큼 피해를 줍니다.";
        }
    }

    public override IEnumerator TryUseCard()
    {
        MonsterCharacter targetMonster = bezierDragLine.detectedMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            bezierDragLine.DestroyAimingImage();

            if (GameManager.instance.volumeUp > 0)
            {
                GameManager.instance.volumeUp -= 1;
                CardUse(targetMonster);

                yield return new WaitForSeconds(1f);
            }

            CardUse(targetMonster);

            GameManager.instance.player.UseCost(GameManager.instance.player.currentCost);
            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        //이게 아마도 근본을 바꾸는게 아니라서 괜찮을 것이다. 일회용으로 늘어나는 느낌?
        damageAbility *= GameManager.instance.player.currentCost;
        GameManager.instance.effectManager.MagicAttack(this, targetMonster);
        SettingManager.Instance.PlaySound(CardClip1);

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
                damageAbility += 6; // 데미지 증가
                break;
            default:
                break;
        }

        SetDescription();
    }
}
