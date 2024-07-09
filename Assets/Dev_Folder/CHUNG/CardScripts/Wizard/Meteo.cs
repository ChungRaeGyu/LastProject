using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : CardBasic
{
    //�̸�
    //������ ��������
    

    private CardBasic cardBasic;
    private Player player; // Player Ŭ���� ���� �߰�
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    private void Start()
    {
        cardBasic = GetComponent<CardBasic>();
        cardCollision = GetComponent<CardCollision>();
        cardDrag = GetComponent<CardDrag>();
        player = GameManager.instance.player; // Player Ŭ���� ã�Ƽ� �Ҵ�

        if (player == null)
        {
            Debug.Log("Player�� ����.");
        }
    }

    public override void TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && player != null)
        {
            //�ڽ�Ʈ�� ����� �� 
            if (player.currentCost >= cost)
            {
                player.UseCost(cost);

                CardUse(targetMonster);
                //currentCard.GetComponent<CardBasic>().CardUse(targetMonster, player);


                //TODO : ������ ���� �Ӽ��� ������ ���� �ϸ� �ǰڴ�.
                //��� ������ CardSO�� �ִ�.
                //CardSO�� ������ ���� ���� ������.
                // 
                /*
                                switch (cardSO.kind){
                                    case Kind.Attack:
                                        GameManager.instance.effectManager.AttackMethod(targetMonster,player,cardSO);
                                        PlayPlayerAttackAnimation();
                                        //���ϰ��ݿ� ���� �޼ҵ�,
                                        break;
                                    case Kind.MagicAttack:
                                        GameManager.instance.effectManager.MagicAttackMethod(targetMonster, player, cardSO);
                                        PlayPlayerAttackAnimation();
                                        break;
                                    case Kind.RangeAttack:
                                        GameManager.instance.effectManager.RangeAttackMethod(cardSO);
                                        PlayPlayerAttackAnimation();
                                        //�������ݿ� ���� �޼ҵ�
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
                //�̷��� ������ ���涧 ���� swtich�� �߰����־�� �Ѵ�., ���ø޼ҵ嵵 �����ؾ��Ѵ�.
                //���� �޼ҵ常 �����ؼ� �ϰ� ������

                // HandManager���� ī�� ����

                DataManager.Instance.AddUsedCard(cardBasic.CardObj);

                GameManager.instance.handManager.RemoveCard(transform);
                Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

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
        //TODO : �ִϸ��̼� �־��ֱ�
    }

    #region Ư��ī�� ���

    #endregion
    private void PlayPlayerAttackAnimation()
    {
        if (player != null && player.animator != null)
        {
            player.animator.SetTrigger("Attack");
        }
    }


}
