using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardBasic
{
    //�̸�
    //������ ��������
    [Header("CardData")]
    

    private CardData cardData;
    private Player player; // Player Ŭ���� ���� �߰�
    private CardDrag cardDrag;
    private float useLimit = -2f;
    private void Start()
    {
        cardData = GetComponent<CardData>();
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
                
            DataManager.Instance.AddUsedCard(cardData.CardObj);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

            if (GameManager.instance.AllMonstersDead())
            {
                GameManager.instance.UIClear(true, false, true, true, true);
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }

    public void CardUse(Monster targetMonster=null)
    {
        GameManager.instance.effectManager.HealMethod(player,this);
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
