using System.Collections;
using UnityEngine;

public class RangeAttack : CardBasic
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
                ? $"�� ��ü���� <b>{damageAbility}</b> ��ŭ ���ظ� �ݴϴ�."
                : $"�� ��ü���� <color={color}><b>{damageAbility}</b></color> ��ŭ ���ظ� �ݴϴ�.";
        }
    }

    public override IEnumerator TryUseCard()
    {
        GameManager.instance.player.UseCost(cost);

        if (GameManager.instance.volumeUp > 0)
        {
            GameManager.instance.volumeUp -= 1;
            CardUse();

            yield return new WaitForSeconds(1f);
        }

        CardUse();

        DataManager.Instance.AddUsedCard(cardBasic);

        GameManager.instance.handManager.RemoveCard(transform);
        Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

        GameManager.instance.CheckAllMonstersDead();
    }

    public void CardUse(MonsterCharacter targetMonster=null)
    {
        GameManager.instance.effectManager.PhysicalAttack(this);
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

    // ��ȭ �ܰ迡 ���� �ɷ�ġ ����
    public override void ApplyEnhancements()
    {
        base.ApplyEnhancements();

        switch (enhancementLevel)
        {
            case 1:
                damageAbility += 2; // ������ ����
                break;
            case 2:
                damageAbility += 4; // ������ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}
