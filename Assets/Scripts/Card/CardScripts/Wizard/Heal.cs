using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardBasic
{
    protected override void Start()
    {
        base.Start();

        SetDescription();
    }

    protected override void SetDescription()
    {
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
                ? $"<b>{utilAbility}</b> 만큼 회복합니다."
                : $"<color={color}><b>{utilAbility}</b></color> 만큼 회복합니다.";
        }
    }

    public override bool TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            CardUse();
            if (GameManager.instance.volumeUp)
            {
                CardUse();
                GameManager.instance.volumeUp = false;
            }

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject); // 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }

    public void CardUse(MonsterCharacter targetMonster = null)
    {
        GameManager.instance.effectManager.PlayerEffect(cardBasic);
        GameManager.instance.player.Heal(utilAbility);
    }
}
