using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;

public class DefenceIncrease : CardBasic
{
    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        SetDescription();
    }

    protected override void SetDescription()
    {
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
                ? $"<b>{utilAbility}</b>��ŭ ������ �����ϴ�."
                : $"<color={color}><b>{utilAbility}</b></color>��ŭ ������ �����ϴ�.";
        }
    }

    public override bool TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            CardUse();

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    public void CardUse(MonsterCharacter targetMonster = null)
    {
        // ������ �Ͻ���(?)���� ����
        GameManager.instance.player.currentDefense += utilAbility;

        // ���� Condition�� ������ ������ŵ�ϴ�.
        GameManager.instance.player.IncrementDefenseConditionStack(utilAbility);
    }
}
