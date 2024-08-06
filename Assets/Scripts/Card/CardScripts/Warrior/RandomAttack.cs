using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RandomAttack : CardBasic
{
    //이름
    //데이터 넣을꺼임
    [Header("CardData")]


    private CardDrag cardDrag;
    private BezierDragLine bezierDragLine;
    protected override void Start()
    {
        base.Start();

        this.enabled = SceneManager.GetActiveScene().buildIndex == 3 ? true : false;

        bezierDragLine = GetComponent<BezierDragLine>();
        cardDrag = GetComponent<CardDrag>();

        SetDescription();
    }

    public override void SetDescription()
    {
        base.SetDescription();

        if (descriptionText != null)
        {
            string color;

            // 초기 ability와 현재 ability 비교
            if (damageAbility > initialDamageAbility)
            {
                color = "#00FF00"; // 초록색
            }
            else if (damageAbility < initialDamageAbility)
            {
                color = "#FF0000"; // 빨간색
            }
            else
            {
                color = ""; // 기본 색
            }

            descriptionText.text = color == ""
                ? $"무작위 적들에게 <b>{damageAbility}</b> 만큼 {utilAbility}번 피해를 줍니다."
                : $"무작위 적들에게 <color={color}><b>{damageAbility}</b></color> 만큼 {utilAbility}번 피해를 줍니다.";
        }
    }

    public override bool TryUseCard()
    {
        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.UseCost(cost);

            CardUse();
            if (GameManager.instance.volumeUp)
            {
                CardUse();
                GameManager.instance.volumeUp = false;
            }

            DataManager.Instance.AddUsedCard(cardBasic);

            GameManager.instance.handManager.RemoveCard(transform);
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

            GameManager.instance.CheckAllMonstersDead();
        }

        return true; // 카드 사용이 실패한 경우 시도했음을 반환
    }

    public void CardUse(MonsterCharacter targetMonster=null)
    {
        GameManager.instance.effectManager.RandomAttackCoroutine(this);
        SettingManager.Instance.PlaySound(CardClip1);

        List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // 복제
        for(int i=0; i< initialUtilAbility; i++)
        {
            int num = Random.Range(0, monsters.Count);
            monsters[num].TakeDamage(damageAbility);
        }
        PlayPlayerAttackAnimation();
    }
    #region 특수카드 사용

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
                damageAbility += 2; // 데미지 증가
                break;
            case 2:
                damageAbility += 3; // 데미지 증가
                break;
            default:
                break;
        }

        SetDescription();
    }
}