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

            // 초기 ability와 현재 ability 비교
            if (utilAbility > initialUtilAbility)
            {
                color = "#00FF00"; // 초록색
            }
            else if (utilAbility < initialUtilAbility)
            {
                color = "#FF0000"; // 빨간색
            }
            else
            {
                color = ""; // 기본 색
            }

            descriptionText.text = color == ""
                ? $"적을 <b>{damageAbility}</b>만큼 데미지를 추고 20퍼센트 확률로 <b>{utilAbility}</b> 턴 동안 얼립니다."
                : $"적을 <color={color}><b>{damageAbility}</b>만큼 데미지를 추고 20퍼센트 확률로<b>{utilAbility}</b></color> 턴 동안 얼립니다.";
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
            Destroy(gameObject);// 카드를 사용했으므로 카드를 제거

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
            targetMonster.animator.StartPlayback(); //몬스터의 애니메이션이 멈춘다.
        }
        targetMonster.TakeDamage(damageAbility);
        //targetMonster.monsterNextAction.gameObject.SetActive(false);
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
                utilAbility += 1; // 빙결 턴 증가
                break; // 아무것도 없음
            case 2:
                utilAbility += 1; // 빙결 턴 증가
                cost -= 1; // 코스트 감소
                break;
            default:
                break;
        }

        SetDescription();
    }
}
