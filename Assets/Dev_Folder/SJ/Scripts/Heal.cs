using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardBasic
{
    //이름
    //데이터 넣을꺼임
    [Header("CardData")]
    public GameObject attackEffect;

    private CardData cardData;
    private Player player; // Player 클래스 참조 추가
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    private void Start()
    {
        cardData = GetComponent<CardData>();
        cardCollision = GetComponent<CardCollision>();
        cardDrag = GetComponent<CardDrag>();
        player = GameManager.instance.player; // Player 클래스 찾아서 할당

        if (player == null)
        {
            Debug.Log("Player가 없음.");
        }
    }

    public override void TryUseCard()
    {
        if (player != null)
        {
            //코스트가 충분할 때 
            if (player.currentCost >= cost)
            {
                player.UseCost(cost);

                //CardUse(targetMonster);
                player.Heal(cardData.CardObj.ability);

                //this와 cardData.CardObj의 차이
                Debug.Log(cardData.CardObj);
                DataManager.Instance.AddUsedCard(cardData.CardObj);

                GameManager.instance.handManager.RemoveCard(transform);
                Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

                if (GameManager.instance.AllMonstersDead())
                {
                    GameManager.instance.UIClear(true, false, true, true, true);
                }

            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }

    public void CardUse(Monster targetMonster)
    {
        
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
