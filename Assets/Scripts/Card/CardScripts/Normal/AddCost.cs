using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCost : CardBasic
{
    private CardCollision cardCollision;

    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        cardCollision = GetComponent<CardCollision>();

        SetDescription();
    }

    public override void SetDescription()
    {
        base.SetDescription();

        if (descriptionText != null)
        {
            string color;

            // �ʱ� ability�� ���� ability ��
            if (utilAbility > initialUtilAbility)
            {
                color = "#00FF00"; // �ʷϻ�
            }
            else if (utilAbility < initialUtilAbility)
            {
                color = "#FF0000"; // ������
            }
            else
            {
                color = ""; // �⺻ ��
            }

            descriptionText.text = color == ""
                ? $"<b>{utilAbility}</b> ��ŭ �ڽ�Ʈ�� ȸ���մϴ�."
                : $"<color={color}><b>{utilAbility}</b></color> ��ŭ �ڽ�Ʈ�� ȸ���մϴ�.";
        }
    }

    public override bool TryUseCard()
    {
        Monster targetMonster = cardCollision.currentMonster;
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            CardUse(targetMonster);
            if (GameManager.instance.volumeUp)
            {
                CardUse(targetMonster);
                GameManager.instance.volumeUp = false;
            }

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        GameManager.instance.effectManager.Buff(cardBasic);
        GameManager.instance.player.AddCost(utilAbility);
    }

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                utilAbility += 1; // ������ ����
                break;
            case 2:
                utilAbility += 1; // ������ ����
                cost -= 1; // �ڽ�Ʈ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}
