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

                targetMonster.TakeDamage(cardSO.ability);

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
                    GameManager.instance.ButtonClear(true, false, true);
                }
                StartCoroutine(DestroyGameObject());
                
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }

    IEnumerator DestroyGameObject()
    {
        //���� ���� ����
        yield return new WaitForSecondsRealtime(4f);
        Destroy(this.gameObject);
    }
    #region Ư��ī�� ���
    private void AddCostMethod()
    {
        PlayerEffectMethod(player.transform.position);
        player.AddCost(cardSO.ability);
    }

    private void AddCardMethod()
    {
        
        for (int i = 0; i < cardSO.ability; i++)
        {
            //GameManager.instance.DrawCardFromDeck();
        }
    }
    private void HealMethod()
    {
        //���� ����Ʈ ���� ī�尡 ������ ������� �������� �ִϸ��̼� �����
        Vector2 pos = player.transform.position;
        PlayerEffectMethod(pos);
        player.currenthealth += cardSO.ability;
    }
    #endregion
    #region ���ݸ޼���
    private void AttackMethod(Monster targetMonster)
    {
        //���ϰ���
        Debug.Log("�ڷ�ƾ ����");
        StartCoroutine(MagicAttack(targetMonster));
        Debug.Log("�ڷ�ƾ ����");
    }
    IEnumerator MagicAttack(Monster targetMonster)
    {
        Debug.Log("�ڷ�ƾ ������ 0.5����");

        PlayerEffectMethod(player.transform.position);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("�ڷ�ƾ ������ 0.5����");

        AttackEffectMethod(GetAttackEffectPos(targetMonster));
        targetMonster.TakeDamage(cardSO.ability);

    }
    private void RangeAttackMethod()
    {
        foreach (Monster monster in GameManager.instance.monsters)
        {
            AttackEffectMethod(GetAttackEffectPos(monster));
            monster.TakeDamage(cardSO.ability);
        }
    }
    #endregion

    #region Effect
    private static Vector2 GetAttackEffectPos(Monster targetMonster)
    {
        //�� 
        Vector2 pos = targetMonster.transform.position;
        Vector2 newPosition = new Vector2(pos.x, pos.y);
        return newPosition;
    }

    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = cardSO.attackEffect;
        Instantiate(prefab, position, prefab.transform.rotation);
        Debug.Log("����Ʈ ����");
    }

    private void PlayerEffectMethod(Vector2 position)
    {

        GameObject prefab = cardSO.effect;
        Instantiate(prefab, position, prefab.transform.rotation);
    }
    #endregion
    private void PlayPlayerAttackAnimation()
    {
        if (player != null && player.animator != null)
        {
            player.animator.SetTrigger("Attack");
        }
    }
}
