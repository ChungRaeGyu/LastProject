using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : CardBasic
{
    //이름
    //데이터 넣을꺼임
    

    private CardBasic cardBasic;
    private Player player; // Player 클래스 참조 추가
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    private void Start()
    {
        cardBasic = GetComponent<CardBasic>();
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
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && player != null)
        {
            //코스트가 충분할 때 
            if (player.currentCost >= cost)
            {
                player.UseCost(cost);

                CardUse(targetMonster);
                //currentCard.GetComponent<CardBasic>().CardUse(targetMonster, player);


                //TODO : 공격의 종류 속성을 놔눠서 공격 하면 되겠다.
                //모든 정보는 CardSO에 있다.
                //CardSO를 공격의 종류 별로 나눈다.
                // 
                /*
                                switch (cardSO.kind){
                                    case Kind.Attack:
                                        GameManager.instance.effectManager.AttackMethod(targetMonster,player,cardSO);
                                        PlayPlayerAttackAnimation();
                                        //단일공격에 관한 메소드,
                                        break;
                                    case Kind.MagicAttack:
                                        GameManager.instance.effectManager.MagicAttackMethod(targetMonster, player, cardSO);
                                        PlayPlayerAttackAnimation();
                                        break;
                                    case Kind.RangeAttack:
                                        GameManager.instance.effectManager.RangeAttackMethod(cardSO);
                                        PlayPlayerAttackAnimation();
                                        //범위공격에 관한 메소드
                                        break;
                                    case Kind.Heal:
                                        GameManager.instance.effectManager.HealMethod(player,cardSO);
                                        break;
                                    case Kind.AddCard:
                                        GameManager.instance.effectManager.AddCardMethod(cardSO);
                                        break;
                                    case Kind.AddCost:
                                        GameManager.instance.effectManager.AddCostMethod(cardSO);
                                        break;
                                }
                                */
                //이러면 종류가 생길때 마다 swtich를 추가해주어야 한다., 관련메소드도 생성해야한다.
                //관련 메소드만 생성해서 하고 싶은데

                // HandManager에서 카드 제거

                DataManager.Instance.AddUsedCard(cardBasic.CardObj);

                GameManager.instance.handManager.RemoveCard(transform);
                Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

                GameManager.instance.CheckAllMonstersDead();
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }

    public void CardUse(Monster targetMonster)
    {
        GameManager.instance.effectManager.MagicRangeAttackMethod(player,this);
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
