using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrozenMagic : CardBasic
{
    private BezierDragLine bezierDragLine;

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
                ? $"���� <b>{utilAbility}</b> �� ���� �󸳴ϴ�."
                : $"���� <color={color}><b>{utilAbility}</b></color> �� ���� �󸳴ϴ�.";
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

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // ī�� ����� ������ ��� �õ������� ��ȯ
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        targetMonster.FreezeForTurns(utilAbility);
        GameManager.instance.effectManager.FrozemMagic(targetMonster,cardBasic);
        targetMonster.animator.StartPlayback(); //������ �ִϸ��̼��� �����.
        targetMonster.deBuffAnim += GameManager.instance.DeBuffAnim;
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
