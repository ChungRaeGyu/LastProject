using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeUp : CardBasic
{
    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

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
                ? $"�̹� �Ͽ��� ������ �� ī�带 <b>{utilAbility}</b> ��ŭ �� ����մϴ�."
                : $"�̹� �Ͽ��� ������ �� ī�带 <color={color}><b>{utilAbility}</b></color> ��ŭ �� ����մϴ�.";
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
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    public void CardUse(MonsterCharacter targetMonster = null)
    {
        SettingManager.Instance.PlaySound(CardClip1);

        GameManager.instance.volumeUp = true;
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

    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                break;
            case 2:
                cost -= 1; // �ڽ�Ʈ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}
