using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicAttack : CardBasic
{
    //이름
    //데이터 넣을꺼임
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

    public override bool TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            //코스트가 충분할 때 
            if (GameManager.instance.player.currentCost >= cost)
            {
                GameManager.instance.player.UseCost(cost);

                CardUse(targetMonster);

                DataManager.Instance.AddUsedCard(cardBasic.cardBasic);

                GameManager.instance.handManager.RemoveCard(transform);
                Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

                GameManager.instance.CheckAllMonstersDead();
            }
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }

    public void CardUse(Monster targetMonster)
    {
        targetMonster.TakeDamage(ability);
        PlayPlayerAttackAnimation();
    }

    #region 특수카드 사용

    #endregion
    private void PlayPlayerAttackAnimation()
    {
        if (GameManager.instance.player != null && GameManager.instance.player.animator != null)
        {
            GameManager.instance.player.animator.SetTrigger("Attack");
        }
    }


}
