using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireBall : CardBasic
{
    public BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        bezierDragLine = GetComponent<BezierDragLine>();

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
                ? $"<b>{damageAbility}</b> 만큼 피해를 주고 {utilAbility}만큼 화상을 입힙니다."
                : $"<color={color}><b>{damageAbility}</b></color> 만큼 피해를 주고 {utilAbility}만큼 화상을 입힙니다.";
        }
    }

    public override IEnumerator TryUseCard()
    {
        MonsterCharacter targetMonster = bezierDragLine.detectedMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            bezierDragLine.DestroyAimingImage();

            GameManager.instance.player.UseCost(cost);

            if (GameManager.instance.volumeUp > 0)
            {
                GameManager.instance.volumeUp -= 1;
                CardUse(targetMonster);

                yield return new WaitForSeconds(1f);
            }

            CardUse(targetMonster);

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);

            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        SettingManager.Instance.PlaySound(CardClip1);

        targetMonster.burnForTurns(utilAbility);
        GameManager.instance.effectManager.MagicAttack(cardBasic,targetMonster);
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
                damageAbility += 4; // 데미지 증가
                utilAbility += 1;
                break;
            default:
                break;
        }

        SetDescription();
    }
}
