using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
                        PlayPlayerAttackAnimation();
                        //단일공격에 관한 메소드,
                        break;
                    case Kind.RangeAttack:
                        RangeAttackMethod();
                        PlayPlayerAttackAnimation();
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

                DataManager.Instance.AddUsedCard(cardSO);
                Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

                if (GameManager.instance.AllMonstersDead())
                {
                    GameManager.instance.TurnEndButton.gameObject.SetActive(false);
                    GameManager.instance.lobbyButton.gameObject.SetActive(true);
                    GameManager.instance.rewardPanel.gameObject.SetActive(true);
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
        //추후 삭제 예정
        yield return new WaitForSecondsRealtime(4f);
        Destroy(this.gameObject);
    }
    #region 특수카드 사용
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
        //여긴 이펙트 말고 카드가 여러장 날라오는 느낌으로 애니메이션 만들기
        Vector2 pos = player.transform.position;
        PlayerEffectMethod(pos);
        player.currenthealth += cardSO.ability;
    }
    #endregion
    #region 공격메서드
    private void AttackMethod(Monster targetMonster)
    {
        //단일공격
        Debug.Log("코루틴 실행");
        StartCoroutine(MagicAttack(targetMonster));
        Debug.Log("코루틴 종료");
    }
    IEnumerator MagicAttack(Monster targetMonster)
    {
        Debug.Log("코루틴 실행중 0.5초전");

        PlayerEffectMethod(player.transform.position);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("코루틴 실행중 0.5초후");

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
        //할 
        Vector2 pos = targetMonster.transform.position;
        Vector2 newPosition = new Vector2(pos.x, pos.y);
        return newPosition;
    }

    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = cardSO.attackEffect;
        Instantiate(prefab, position, prefab.transform.rotation);
        Debug.Log("이펙트 실행");
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
