using System.Collections;
using UnityEngine;


public class HammerAttack : CardBasic
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
                ? $"<b>{damageAbility}</b> ��ŭ ���ظ� �ݴϴ�. ī�带 1�� �̽��ϴ�."
                : $"<color={color}><b>{damageAbility}</b></color> ��ŭ ���ظ� �ݴϴ�. ī�带 1�� �̽��ϴ�.";
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
            }

            CardUse(targetMonster);

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// ī�带 ��������Ƿ� ī�带 ����

            GameManager.instance.CheckAllMonstersDead();
        }
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        GameManager.instance.effectManager.PhysicalAttack(this, targetMonster);
        GameManager.instance.StartCoroutine(DrawCard());
        SettingManager.Instance.PlaySound(CardClip1);
        PlayPlayerAttackAnimation();
    }
    private IEnumerator DrawCard()
    {
        if (DataManager.Instance.deck.Count + DataManager.Instance.usedCards.Count == 0) utilAbility = 0;
        // ������ ī�� �̱�
        yield return GameManager.instance.StartCoroutine(GameManager.instance.DrawInitialHand(utilAbility));
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