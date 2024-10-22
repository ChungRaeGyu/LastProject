using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpendedPower : CardBasic
{
    private BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        bezierDragLine = GetComponent<BezierDragLine>();

        SetDescription();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
            descriptionText.text = $"버린 카드 수 <color=#00FF00><b>{DataManager.Instance.usedCards.Count}</b></color> X 5 만큼 피해를 줍니다.";
    }

    public override void SetDescription()
    {
        base.SetDescription();

        if (descriptionText != null)
        {
            string color = "#FFFFFF";
            string cardCountText;

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                color = "#00FF00"; // 초록색
                cardCountText = $"{DataManager.Instance.usedCards.Count}"; // 실제 카드 사용 수
            }
            else
            {
                color = "#00FF00"; // 초록색
                cardCountText = "X"; // 카드 수를 대신하는 X
            }

            descriptionText.text = $"버린 카드 수 <color={color}><b>{cardCountText}</b></color> X 5 만큼 피해를 줍니다.";
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

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        SettingManager.Instance.PlaySound(CardClip1);

        damageAbility = DataManager.Instance.usedCards.Count * 5;
        GameManager.instance.effectManager.MagicAttack(this, targetMonster);

        PlayPlayerAttackAnimation();
    }

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
                cost -= 1; // 코스트 감소
                break;
            case 2:
                cost -= 2; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
