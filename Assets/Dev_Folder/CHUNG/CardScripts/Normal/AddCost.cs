using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCost : CardBasic
{
    //�̸�
    //������ ��������
    [Header("CardData")]


    private CardBasic cardBasic;
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    private void Start()
    {
        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        cardBasic = GetComponent<CardBasic>();
        cardCollision = GetComponent<CardCollision>();
        cardDrag = GetComponent<CardDrag>();
    }

    public override void TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            //�ڽ�Ʈ�� ����� �� 
            if (GameManager.instance.player.currentCost >= cost)
            {
                GameManager.instance.player.UseCost(cost);

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
        GameManager.instance.effectManager.AddCostMethod(CardObj);
    }

    #region Ư��ī�� ���

    #endregion
    private void PlayPlayerAttackAnimation()
    {
        if (GameManager.instance.player != null && GameManager.instance.player.animator != null)
        {
            GameManager.instance.player.animator.SetTrigger("Attack");
        }
    }


}
