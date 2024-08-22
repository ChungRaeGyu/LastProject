using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcaneDevastation : CardBasic
{
    private BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        bezierDragLine = GetComponent<BezierDragLine>();

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
                ? $"<b>{damageAbility}</b> ��ŭ ���ظ� �ְ�, �� �������� ���� �׾��ٸ� �ڽ�Ʈ�� <b>{utilAbility}</b> ��ŭ ȸ���մϴ�."
                : $"<color={color}><b>{damageAbility}</b></color> ��ŭ ���ظ� �ְ�, �� �������� ���� �׾��ٸ� �ڽ�Ʈ�� <b>{utilAbility}</b> ��ŭ ȸ���մϴ�.";
        }
    }

    public override IEnumerator TryUseCard()
    {
        MonsterCharacter targetMonster = bezierDragLine.detectedMonster;
        if (targetMonster != null && GameManager.instance.player != null)
        {
            bezierDragLine.DestroyAimingImage();

            GameManager.instance.player.UseCost(cost);


            if (GameManager.instance.volumeUp > 0)
            {
                GameManager.instance.volumeUp -= 1;
                CardUse(targetMonster);
                yield return new WaitForSeconds(1f);
                // ���Ͱ� �׾����� Ȯ��
                if (targetMonster.IsDead())
                {
                    RecoverCost();
                }

                yield return new WaitForSeconds(.5f);
            }

            CardUse(targetMonster);
            yield return new WaitForSeconds(1f);
            // ���Ͱ� �׾����� Ȯ��
            if (targetMonster.IsDead())
            {
                RecoverCost();
            }

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        GameManager.instance.effectManager.MagicAttack(this, targetMonster);

        SettingManager.Instance.PlaySound(CardClip1);
        PlayPlayerAttackAnimation();
    }

    private void RecoverCost()
    {
        GameManager.instance.player.AddCost(utilAbility);
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
                damageAbility += 6; // ������ ����
                break;
            case 2:
                damageAbility += 12; // ������ ����
                cost -= 1; // �ڽ�Ʈ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}
