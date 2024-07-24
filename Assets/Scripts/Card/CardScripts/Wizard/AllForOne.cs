using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllForOne : CardBasic
{
    public BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        bezierDragLine = GetComponent<BezierDragLine>();

        SetDescription();
    }

    protected override void SetDescription()
    {
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
                ? $"<b>{damageAbility}</b> 만큼 피해를 줍니다. 카드더미에 코스트가 0인카드를 모두 손으로 가져옵니다."
                : $"<color={color}><b>{damageAbility}</b></color> 만큼 번개 피해를 줍니다.만큼 피해를 줍니다. 카드더미에 코스트가 0인카드를 모두 손으로 가져옵니다.";
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

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        List<CardBasic> tempCards = new List<CardBasic>();
        targetMonster.TakeDamage(damageAbility);
        foreach(CardBasic temp in DataManager.Instance.usedCards)
        {
            if(temp.cost==0)
                tempCards.Add(temp);
        }
        foreach (CardBasic temp in tempCards)
        {
            DataManager.Instance.usedCards.Remove(temp);
            GameManager.instance.AddCard(temp);
        }

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
}
