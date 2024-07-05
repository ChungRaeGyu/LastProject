using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CardUse : MonoBehaviour
{
    private Player player; // Player Ŭ���� ���� �߰�
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    public CardSO cardSO { get; private set; }

    private void Start()
    {
        cardCollision = GetComponent<CardCollision>();
        cardDrag = GetComponent<CardDrag>();
        cardSO = GetComponent<CardData>().cardSO;
        player = GameManager.instance.player; // Player Ŭ���� ã�Ƽ� �Ҵ�

        if (player == null)
        {
            Debug.Log("Player�� ����.");
        }
    }

    public void TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && player != null)
        {
            //�ڽ�Ʈ�� ����� �� 
            if (player.currentCost >= cardSO.cost)
            {
                player.UseCost(cardSO.cost);

                //TODO : ������ ���� �Ӽ��� ������ ���� �ϸ� �ǰڴ�.
                //��� ������ CardSO�� �ִ�.
                //CardSO�� ������ ���� ���� ������.
                // 
                switch(cardSO.kind){
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
                //�̷��� ������ ���涧 ���� swtich�� �߰����־�� �Ѵ�., ���ø޼ҵ嵵 �����ؾ��Ѵ�.
                //���� �޼ҵ常 �����ؼ� �ϰ� ������

                // HandManager���� ī�� ����
                HandManager handManager = GameManager.instance.handManager;
                if (handManager != null)
                {
                    handManager.RemoveCard(transform);
                }

                DataManager.Instance.AddUsedCard(cardSO);
                Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

                if (GameManager.instance.AllMonstersDead())
                {
                    GameManager.instance.turnEndButton.gameObject.SetActive(false);
                    GameManager.instance.lobbyButton.gameObject.SetActive(true);
                    GameManager.instance.rewardPanel.gameObject.SetActive(true);
                }
                
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
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
