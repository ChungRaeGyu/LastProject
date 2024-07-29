using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefensiveStrike : CardBasic
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
            string color = "#FFFFFF";
            string currentDefenseText;

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                color = "#00FF00"; // 초록색
                currentDefenseText = $"{GameManager.instance.player.currentDefense}"; // 실제 카드 사용 수
            }
            else
            {
                currentDefenseText = "X"; // 카드 수를 대신하는 X
            }

            descriptionText.text = $"현재 방어력 만큼 <color={color}><b>{currentDefenseText}</b></color> 피해를 줍니다.";
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
            if (GameManager.instance.volumeUp)
            {
                CardUse(targetMonster);
                GameManager.instance.volumeUp = false;
            }

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }

    public void CardUse(MonsterCharacter targetMonster)
    {
        targetMonster.TakeDamage(GameManager.instance.player.currentDefense);
        PlayPlayerAttackAnimation();
    }

    private void PlayPlayerAttackAnimation()
    {
        if (GameManager.instance.player != null && GameManager.instance.player.animator != null)
        {
            GameManager.instance.player.animator.SetTrigger("Attack");
        }
    }
}
