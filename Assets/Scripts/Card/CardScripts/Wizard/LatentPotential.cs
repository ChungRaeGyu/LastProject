using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LatentPotential : CardBasic
{
    private BezierDragLine bezierDragLine;

    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        bezierDragLine = GetComponent<BezierDragLine>();

        SetDescription();
    }

    public override void SetDescription()
    {
        base.SetDescription();

        if (descriptionText != null)
        {
            string color = "#FFFFFF";
            string cardCountText;

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                color = "#00FF00"; // �ʷϻ�
                cardCountText = $"{DataManager.Instance.deck.Count}"; // ���� ī�� ��� ��
            }
            else
            {
                cardCountText = "X"; // ī�� ���� ����ϴ� X
            }

            descriptionText.text = $"������ ���� ī�� �� <color={color}><b>{cardCountText}</b></color> ��ŭ ���ظ� �ݴϴ�.";
        }
    }

    public override bool TryUseCard()
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
            }

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
        SettingManager.Instance.PlaySound(CardClip1);

        damageAbility = DataManager.Instance.deck.Count;
        GameManager.instance.effectManager.MagicAttack(this, targetMonster);
        PlayPlayerAttackAnimation();
    }

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
                cost -= 1; // �ڽ�Ʈ ����
                break;
            case 2:
                cost -= 2; // �ڽ�Ʈ ����
                break;
            default:
                break;
        }

        SetDescription();
    }
}

