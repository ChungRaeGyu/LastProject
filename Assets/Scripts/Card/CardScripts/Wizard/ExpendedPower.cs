using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpendedPower : CardBasic
{
    private BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        bezierDragLine = GetComponent<BezierDragLine>();

        SetDescription();
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
                cardCountText = "X"; // 카드 수를 대신하는 X
            }

            descriptionText.text = $"버려진 카드 수 <color={color}><b>{cardCountText}</b></color> 만큼 피해를 줍니다.";
        }
    }

    public override bool TryUseCard()
    {
        MonsterCharacter targetMonster = bezierDragLine.detectedMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            bezierDragLine.DestroyAimingImage();

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
        targetMonster.TakeDamage(DataManager.Instance.usedCards.Count);
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
