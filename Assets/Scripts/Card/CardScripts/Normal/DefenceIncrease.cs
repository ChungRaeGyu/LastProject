using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefenceIncrease : CardBasic
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
                ? $"<b>{utilAbility}</b>만큼 방어력이 오릅니다."
                : $"<color={color}><b>{utilAbility}</b></color>만큼 방어력이 오릅니다.";
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

        //이펙트
        GameManager.instance.effectManager.Buff(this);
        // 방어력이 일시적(?)으로 증가
        GameManager.instance.player.currentDefense += utilAbility;

        // 방어력 Condition의 스택을 증가시킵니다.
        GameManager.instance.player.AddConditions(GameManager.instance.defenseconditionPrefab,utilAbility);
    }

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                utilAbility += 1; // 증가 방어력 증가
                break;
            case 2:
                utilAbility += 1; // 증가 방어력 증가
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
