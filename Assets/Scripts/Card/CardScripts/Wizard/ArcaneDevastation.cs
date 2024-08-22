using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcaneDevastation : CardBasic
{
    private BezierDragLine bezierDragLine;

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
                ? $"<b>{damageAbility}</b> 만큼 피해를 주고, 이 공격으로 적이 죽었다면 코스트를 <b>{utilAbility}</b> 만큼 회복합니다."
                : $"<color={color}><b>{damageAbility}</b></color> 만큼 피해를 주고, 이 공격으로 적이 죽었다면 코스트를 <b>{utilAbility}</b> 만큼 회복합니다.";
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
                // 몬스터가 죽었는지 확인
                if (targetMonster.IsDead())
                {
                    RecoverCost();
                }

                yield return new WaitForSeconds(.5f);
            }

            CardUse(targetMonster);
            yield return new WaitForSeconds(1f);
            // 몬스터가 죽었는지 확인
            if (targetMonster.IsDead())
            {
                RecoverCost();
            }

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        GameManager.instance.effectManager.MagicAttack(this, targetMonster);

        SettingManager.Instance.PlaySound(CardClip1);
        PlayPlayerAttackAnimation();
    }

    private void RecoverCost()
    {
        GameManager.instance.player.AddCost(utilAbility);
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
                damageAbility += 6; // 데미지 증가
                break;
            case 2:
                damageAbility += 12; // 데미지 증가
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
