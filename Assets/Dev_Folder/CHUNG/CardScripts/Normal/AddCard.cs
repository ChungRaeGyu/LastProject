using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCard : CardBasic
{
    //이름
    //데이터 넣을꺼임
    [Header("CardData")]

    private CardDrag cardDrag;
    private void Start()
    {
        cardDrag = GetComponent<CardDrag>();
    }

    public override bool TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            CardUse();

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);

            // 덱에서 카드 뽑기 시작
            GameManager.instance.StartCoroutine(DrawCard());

            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }

    private IEnumerator DrawCard()
    {
        // 덱에서 카드 뽑기
        yield return GameManager.instance.StartCoroutine(GameManager.instance.DrawInitialHand(ability));
    }

    public void CardUse(Monster targetMonster = null)
    {
        GameManager.instance.effectManager.AddCardMethod(cardBasic);
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
