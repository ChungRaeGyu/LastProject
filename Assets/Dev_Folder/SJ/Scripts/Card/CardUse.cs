using System;
using UnityEngine;

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
                        AttackMethod(targetMonster);
                        //���ϰ��ݿ� ���� �޼ҵ�,
                        break;
                    case Kind.RangeAttack:
                        RangeAttackMethod();
                        //�������ݿ� ���� �޼ҵ�
                        break;
                    case Kind.Heal:
                        HealMethod();
                        break;
                    case Kind.AddCard:
                        AddCardMethod();
                        break;
                    case Kind.AddCost:
                        AddCostMethod();
                        break;
                }
            //�̷��� ������ ���涧 ���� swtich�� �߰����־�� �Ѵ�., ���ø޼ҵ嵵 �����ؾ��Ѵ�.
            //���� �޼ҵ常 �����ؼ� �ϰ� ������


                
                Destroy(gameObject);

                if (GameManager.instance.AllMonstersDead())
                {
                    GameManager.instance.TurnEndButton.gameObject.SetActive(false);
                    GameManager.instance.lobbyButton.gameObject.SetActive(true);
                }

                PlayPlayerAttackAnimation();
            }
        }
        else
        {
            cardDrag.ResetPosition();
        }
    }

    private void AddCostMethod()
    {
        player.AddCost(cardSO.ability);
    }

    private void AddCardMethod()
    {
        
        for (int i = 0; i < cardSO.ability; i++)
        {
            //GameManager.instance.DrawCardFromDeck();
        }
    }

    private void EffectMethod()
    {
        
    }
    private void AttackMethod(Monster targetMonster)
    {
        EffectMethod();
        targetMonster.TakeDamage(cardSO.ability);
    }

    private void HealMethod()
    {
        EffectMethod();
        player.currenthealth += cardSO.ability;
    }

    private void RangeAttackMethod()
    {
        foreach (Monster monster in GameManager.instance.monsters)
        {
            EffectMethod();
            monster.TakeDamage(cardSO.ability);
        }
    }

    private void PlayPlayerAttackAnimation()
    {
        if (player != null && player.animator != null)
        {
            player.animator.SetTrigger("Attack");
        }
    }
}
