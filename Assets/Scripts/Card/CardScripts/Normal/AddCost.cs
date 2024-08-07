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

    public override IEnumerator TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (GameManager.instance.player != null)
        {
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

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        //SettingManager.Instance.PlaySound(CardClip1); // 소리 없는게 나음

        GameManager.instance.effectManager.Buff(cardBasic);
        GameManager.instance.player.AddCost(utilAbility);
    }

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                utilAbility += 1; // 코스트 증가량 증가
                break;
            case 2:
                utilAbility += 1; // 코스트 증가량 증가
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
