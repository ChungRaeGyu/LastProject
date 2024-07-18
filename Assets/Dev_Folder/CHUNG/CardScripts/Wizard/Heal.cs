using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardBasic
{
    //이름
    //데이터 넣을꺼임
    [Header("CardData")]


    private CardDrag cardDrag;

    private void Start()
    {
        cardDrag = GetComponent<CardDrag>();
    }

    public override bool TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            CardUse();

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject); // 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }


    public void CardUse(Monster targetMonster = null)
    {
        GameManager.instance.effectManager.PlayerEffect(cardBasic);
        GameManager.instance.player.Heal(ability);
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


}
