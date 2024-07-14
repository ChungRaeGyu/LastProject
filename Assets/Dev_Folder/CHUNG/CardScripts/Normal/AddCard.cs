using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCard : CardBasic
{
    //�̸�
    //������ ��������
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

            // ������ ī�� �̱� ����
            GameManager.instance.StartCoroutine(DrawCard());

            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    private IEnumerator DrawCard()
    {
        // ������ ī�� �̱�
        yield return GameManager.instance.StartCoroutine(GameManager.instance.DrawInitialHand(ability));
    }

    public void CardUse(Monster targetMonster = null)
    {
        GameManager.instance.effectManager.AddCardMethod(cardBasic);
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
