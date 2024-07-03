using System;
using UnityEngine;

public class CardUse : MonoBehaviour
{
    private Player player; // Player 클래스 참조 추가
    private CardDrag cardDrag;
    private CardCollision cardCollision;
    public CardSO cardSO { get; private set; }

    private void Start()
    {
        cardCollision = GetComponent<CardCollision>();
        cardDrag = GetComponent<CardDrag>();
        cardSO = GetComponent<CardData>().cardSO;
        player = GameManager.instance.player; // Player 클래스 찾아서 할당

        if (player == null)
        {
            Debug.Log("Player가 없음.");
        }
    }

    public void TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && player != null)
        {
            //코스트가 충분할 때 
            if (player.currentCost >= cardSO.cost)
            {
                player.UseCost(cardSO.cost);

                //TODO : 공격의 종류 속성을 놔눠서 공격 하면 되겠다.
                //모든 정보는 CardSO에 있다.
                //CardSO를 공격의 종류 별로 나눈다.
                // 
                switch(cardSO.kind){
                    case Kind.Attack:
                        AttackMethod(targetMonster);
                        //단일공격에 관한 메소드,
                        break;
                    case Kind.RangeAttack:
                        RangeAttackMethod();
                        //범위공격에 관한 메소드
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
            //이러면 종류가 생길때 마다 swtich를 추가해주어야 한다., 관련메소드도 생성해야한다.
            //관련 메소드만 생성해서 하고 싶은데


                
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
