using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardBasic
{
    //이름
    //데이터 넣을꺼임
    [Header("CardData")]
    

    private CardBasic cardBasic;
    private Player player; // Player 클래스 참조 추가
    private CardDrag cardDrag;
    private float useLimit = -2f;
    private void Start()
    {
        cardBasic = GetComponent<CardBasic>();
        cardDrag = GetComponent<CardDrag>();
        player = GameManager.instance.player; // Player 클래스 찾아서 할당

        if (player == null)
        {
            Debug.Log("Player가 없음.");
        }
    }

    public override void TryUseCard()
    {
        if (player != null && transform.position.y > useLimit) { 
            //코스트가 충분할 때 
            player.UseCost(cost);

            CardUse();
                
            DataManager.Instance.AddUsedCard(cardBasic.CardObj);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }

    public void CardUse(Monster targetMonster=null)
    {
        GameManager.instance.effectManager.HealMethod(player, cardData.CardObj);
        player.Heal(ability);
        //TODO : 애니메이션 넣어주기
    }

    #region 특수카드 사용

    #endregion
    private void PlayPlayerAttackAnimation()
    {
        if (player != null && player.animator != null)
        {
            player.animator.SetTrigger("Attack");
        }
    }


}
