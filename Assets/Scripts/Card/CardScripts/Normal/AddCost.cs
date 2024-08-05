using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCost : CardBasic
{
    private CardCollision cardCollision;

    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        cardCollision = GetComponent<CardCollision>();

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
                ? $"<b>{utilAbility}</b> 만큼 코스트를 회복합니다."
                : $"<color={color}><b>{utilAbility}</b></color> 만큼 코스트를 회복합니다.";
        }
    }

    public override bool TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (GameManager.instance.player != null)
        {
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
        GameManager.instance.effectManager.Buff(cardBasic);
        GameManager.instance.player.AddCost(utilAbility);
    }

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                utilAbility += 1; // 데미지 증가
                break;
            case 2:
                utilAbility += 1; // 데미지 증가
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
