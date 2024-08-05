using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCard : CardBasic
{
    protected override void Start()
    {
        base.Start();

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
                ? $"<b>{utilAbility}</b> ��ŭ ī�带 �̽��ϴ�."
                : $"<color={color}><b>{utilAbility}</b></color> ��ŭ ī�带 �̽��ϴ�.";
        }
    }

    public override bool TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            CardUse();
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    private IEnumerator DrawCard()
    {
        // ������ ī�� �̱�
        yield return GameManager.instance.StartCoroutine(GameManager.instance.DrawInitialHand(utilAbility));
    }

    public void CardUse(Monster targetMonster = null)
    {
        GameManager.instance.effectManager.Buff(cardBasic);
        GameManager.instance.player.UseCost(cost);
        if (GameManager.instance.volumeUp)
        {
            GameManager.instance.player.UseCost(cost);
            GameManager.instance.volumeUp = false;
        }
        DataManager.Instance.AddUsedCard(cardBasic);

        GameManager.instance.handManager.RemoveCard(transform);

        // ������ ī�� �̱� ����
        GameManager.instance.StartCoroutine(DrawCard());

        Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

        GameManager.instance.CheckAllMonstersDead();
    }

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                utilAbility += 1; // ī�� 1���
                break;
            case 2:
                utilAbility += 1;
                cost -= 1; // �ڽ�Ʈ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}
