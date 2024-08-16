using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FullPower: CardBasic
{
    private BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        bezierDragLine = GetComponent<BezierDragLine>();

        SetDescription();

        costText.text = "X";
    }

    public override void SetDescription()
    {
        base.SetDescription();

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
                ? $"<b>{damageAbility}</b> X �����ڽ�Ʈ��ŭ ���ظ� �ݴϴ�."
                : $"<color={color}><b>{damageAbility}</b></color> X �����ڽ�Ʈ��ŭ ���ظ� �ݴϴ�.";
        }
    }

    public override IEnumerator TryUseCard()
    {
        MonsterCharacter targetMonster = bezierDragLine.detectedMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            bezierDragLine.DestroyAimingImage();

            if (GameManager.instance.volumeUp > 0)
            {
                GameManager.instance.volumeUp -= 1;
                CardUse(targetMonster);

                yield return new WaitForSeconds(1f);
            }

            CardUse(targetMonster);

            GameManager.instance.player.UseCost(GameManager.instance.player.currentCost);
            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        //�̰� �Ƹ��� �ٺ��� �ٲٴ°� �ƴ϶� ������ ���̴�. ��ȸ������ �þ�� ����?
        damageAbility *= GameManager.instance.player.currentCost;
        GameManager.instance.effectManager.MagicAttack(this, targetMonster);
        SettingManager.Instance.PlaySound(CardClip1);

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
                damageAbility += 3; // ������ ����
                break;
            case 2:
                damageAbility += 6; // ������ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}
