using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : CardBasic
{
    //�̸�
    //������ ��������
    [Header("CardData")]
    

    private CardBasic cardBasic;
    private CardDrag cardDrag;
    private float useLimit = -2f;
    private void Start()
    {
        cardBasic = GetComponent<CardBasic>();
        cardDrag = GetComponent<CardDrag>();
    }

    public override bool TryUseCard()
    {
        if (GameManager.instance.player != null && transform.position.y > useLimit)
        {
            // �ڽ�Ʈ�� ����� �� 
            GameManager.instance.player.UseCost(cost);

            CardUse();

            DataManager.Instance.AddUsedCard(cardBasic.cardObj);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject); // ī�带 ��������Ƿ� ī�带 ����

            GameManager.instance.CheckAllMonstersDead();
        }

        return false; // ī�� ����� ������ ��� ��ȯ
    }


    public void CardUse(Monster targetMonster=null)
    {
        GameManager.instance.effectManager.HealMethod(GameManager.instance.player, cardObj);
        GameManager.instance.player.Heal(ability);
        //TODO : �ִϸ��̼� �־��ֱ�
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
