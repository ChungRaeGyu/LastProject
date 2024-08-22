using System.Collections;
using UnityEngine;
public class FrozenMagic : CardBasic
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
                ? $"���� <b>{damageAbility}</b>��ŭ �������� �߰� 20�ۼ�Ʈ Ȯ���� <b>{utilAbility}</b> �� ���� �󸳴ϴ�."
                : $"���� <color={color}><b>{damageAbility}</b>��ŭ �������� �߰� 20�ۼ�Ʈ Ȯ����<b>{utilAbility}</b></color> �� ���� �󸳴ϴ�.";
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
        SettingManager.Instance.PlaySound(CardClip1);
        int rand = Random.Range(1, 11);
        if (rand <= 3)
        {
            targetMonster.FreezeForTurns(utilAbility);
            GameManager.instance.effectManager.Debuff(targetMonster,cardBasic);
            targetMonster.animator.StartPlayback(); //������ �ִϸ��̼��� �����.
        }
        targetMonster.TakeDamage(damageAbility);
        //targetMonster.monsterNextAction.gameObject.SetActive(false);
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
                utilAbility += 1; // ���� �� ����
                break; // �ƹ��͵� ����
            case 2:
                utilAbility += 1; // ���� �� ����
                cost -= 1; // �ڽ�Ʈ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}
