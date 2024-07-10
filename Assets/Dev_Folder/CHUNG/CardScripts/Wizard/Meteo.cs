using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meteo : CardBasic
{
    //�̸�
    //������ ��������
    

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

    public override bool TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            //�ڽ�Ʈ�� ����� �� 
            if (GameManager.instance.player.currentCost >= cost)
            {
                GameManager.instance.player.UseCost(cost);

                CardUse(targetMonster);

                DataManager.Instance.AddUsedCard(cardBasic.CardObj);

                GameManager.instance.handManager.RemoveCard(transform);
                Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

                GameManager.instance.CheckAllMonstersDead();
            }
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    public void CardUse(Monster targetMonster)
    {
        GameManager.instance.effectManager.MagicRangeAttackMethod(GameManager.instance.player, CardObj);
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
