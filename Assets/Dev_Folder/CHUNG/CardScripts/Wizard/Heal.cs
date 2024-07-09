using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardBasic
{
    //�̸�
    //������ ��������
    [Header("CardData")]
    

    private CardBasic cardBasic;
    private Player player; // Player Ŭ���� ���� �߰�
    private CardDrag cardDrag;
    private float useLimit = -2f;
    private void Start()
    {
        cardBasic = GetComponent<CardBasic>();
        cardDrag = GetComponent<CardDrag>();
        player = GameManager.instance.player; // Player Ŭ���� ã�Ƽ� �Ҵ�

        if (player == null)
        {
            Debug.Log("Player�� ����.");
        }
    }

    public override void TryUseCard()
    {
        if (player != null && transform.position.y > useLimit) { 
            //�ڽ�Ʈ�� ����� �� 
            player.UseCost(cost);

            CardUse();
                
            DataManager.Instance.AddUsedCard(cardBasic.CardObj);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

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
