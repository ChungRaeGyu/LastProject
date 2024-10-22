using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeUp : CardBasic
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
            if (utilAbility > initialUtilAbility)
            {
                color = "#00FF00"; // 초록색
            }
            else if (utilAbility < initialUtilAbility)
            {
                color = "#FF0000"; // 빨간색
            }
            else
            {
                color = ""; // 기본 색
            }

            descriptionText.text = color == ""
                ? $"이번 턴에만 다음에 쓸 카드를 <b>{utilAbility}</b> 만큼 더 사용합니다."
                : $"이번 턴에만 다음에 쓸 카드를 <color={color}><b>{utilAbility}</b></color> 만큼 더 사용합니다.";
        }
    }

    public override IEnumerator TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            CardUse();

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거
        }

        yield return null;
    }

    public void CardUse(MonsterCharacter targetMonster = null)
    {
        SettingManager.Instance.PlaySound(CardClip1);

        GameManager.instance.volumeUp += utilAbility;
        GameManager.instance.effectManager.Buff(this);
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
                utilAbility += 1; // 잔여 횟수 증가
                break;
            case 2:
                utilAbility += 1; // 잔여 횟수 증가
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
