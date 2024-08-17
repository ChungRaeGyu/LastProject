using System.Collections;
using UnityEngine;

public class Meteo : CardBasic
{
    protected override void Start()
    {
        base.Start();

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
                ? $"운석을 떨어트려 적 전체에게 <b>{damageAbility}</b> 만큼 피해를 줍니다."
                : $"운석을 떨어트려 적 전체에게 <color={color}><b>{damageAbility}</b></color> 만큼 피해를 줍니다.";
        }
    }

    public override IEnumerator TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            if (GameManager.instance.volumeUp > 0)
            {
                GameManager.instance.volumeUp -= 1;
                CardUse();

                yield return new WaitForSeconds(1f);
            }

            CardUse();

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster = null)
    {
        SettingManager.Instance.PlaySound(CardClip1);

        GameManager.instance.effectManager.MagicAttack(cardBasic);
        //TODO : 애니메이션 넣어주기
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
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
