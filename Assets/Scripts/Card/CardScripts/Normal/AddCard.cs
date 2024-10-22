using System.Collections;
using UnityEngine;

public class AddCard : CardBasic
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
                ? $"<b>{utilAbility}</b> 만큼 카드를 뽑습니다."
                : $"<color={color}><b>{utilAbility}</b></color> 만큼 카드를 뽑습니다.";
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

    private IEnumerator DrawCard()
    {
        if (DataManager.Instance.deck.Count + DataManager.Instance.usedCards.Count == 0) utilAbility = 0;
        // 덱에서 카드 뽑기
        yield return GameManager.instance.StartCoroutine(GameManager.instance.DrawInitialHand(utilAbility));
    }

    public void CardUse(Monster targetMonster = null)
    {
        //SettingManager.Instance.PlaySound(CardClip1); // 소리 없는게 나음

        GameManager.instance.effectManager.Buff(cardBasic);

        // 덱에서 카드 뽑기 시작
        GameManager.instance.StartCoroutine(DrawCard());
    }

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                utilAbility += 1; // 카드 1장더
                break;
            case 2:
                utilAbility += 1;
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
