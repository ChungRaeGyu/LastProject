using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllForOne : CardBasic
{
    public BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        bezierDragLine = GetComponent<BezierDragLine>();

        SetDescription();
    }

    protected override void SetDescription()
    {
        if (descriptionText != null)
        {
            string color;

            // �ʱ� ability�� ���� ability ��
            if (damageAbility > initialDamageAbility)
            {
                color = "#00FF00"; // �ʷϻ�
            }
            else if (damageAbility < initialDamageAbility)
            {
                color = "#FF0000"; // ������
            }
            else
            {
                color = ""; // �⺻ ��
            }

            descriptionText.text = color == ""
                ? $"<b>{damageAbility}</b> ��ŭ ���ظ� �ݴϴ�. ī����̿� �ڽ�Ʈ�� 0��ī�带 ��� ������ �����ɴϴ�."
                : $"<color={color}><b>{damageAbility}</b></color> ��ŭ ���� ���ظ� �ݴϴ�.��ŭ ���ظ� �ݴϴ�. ī����̿� �ڽ�Ʈ�� 0��ī�带 ��� ������ �����ɴϴ�.";
        }
    }

    public override bool TryUseCard()
    {
        MonsterCharacter targetMonster = bezierDragLine.detectedMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            bezierDragLine.DestroyAimingImage();

            GameManager.instance.player.UseCost(cost);

            CardUse(targetMonster);

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        List<CardBasic> tempCards = new List<CardBasic>();
        targetMonster.TakeDamage(damageAbility);
        foreach(CardBasic temp in DataManager.Instance.usedCards)
        {
            if(temp.cost==0)
                tempCards.Add(temp);
        }
        foreach (CardBasic temp in tempCards)
        {
            DataManager.Instance.usedCards.Remove(temp);
            GameManager.instance.AddCard(temp);
        }

        PlayPlayerAttackAnimation();
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
