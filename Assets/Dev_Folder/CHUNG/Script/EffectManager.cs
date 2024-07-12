using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    //마법과 물리의 차이
    //마법 : 캐스팅 후 스킬 실행
    //물리 : 플레이어와 몬스터 이펙트 동시 실행

    public RangeAttackSystem RangeAttack;
    public Player tempPlayer;
    public CardBasic tempCardInfo;

    #region 물리공격
    public void AttackMethod(Monster targetMonster, Player player,CardSO cardSO)
    {
        PlayerEffectMethod(tempPlayer.transform.position);
        AttackEffectMethod(targetMonster.transform.position);
    }
    public void RangeAttackMethod(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        foreach (Monster monster in GameManager.instance.monsters)
        {
            monster.TakeDamage(tempCardInfo.ability);
        }
    }
    #endregion
    #region 마법공격
    public void MagicAttackMethod(MonsterCharacter targetMonster, Player player,CardBasic cardBasic)
    {
        tempPlayer = player;
        tempCardInfo = cardBasic;
        //단일공격
        Debug.Log("코루틴 실행");
        StartCoroutine(MagicAttack(false,targetMonster));
        Debug.Log("코루틴 종료");
    }
    public void MagicRangeAttackMethod(Player player,CardBasic cardBasic)
    {
        tempPlayer = player;
        tempCardInfo = cardBasic;
        StartCoroutine(MagicAttack(true));
    }
    IEnumerator MagicAttack(bool isRange,MonsterCharacter targetMonster=null)
    {
        Debug.Log("코루틴 실행중 0.5초전");

        PlayerEffectMethod(tempPlayer.transform.position);
        yield return new WaitForSeconds(1f);
        Debug.Log("코루틴 실행중 0.5초후");
        if (isRange)
        {
            RangeAttack.AttackAnim(tempCardInfo);
            foreach (MonsterCharacter monster in GameManager.instance.monsters)
            {
                monster.TakeDamage(tempCardInfo.ability);
            }
        }
        else
        {
            AttackEffectMethod(targetMonster.transform.position);
            targetMonster.TakeDamage(tempCardInfo.ability);
        }
    }
    #endregion
    #region 버프 및 기타 능력(Player에게만 이팩트 존재)
    public void AddCostMethod(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        PlayerEffectMethod(tempPlayer.transform.position);
        tempPlayer.AddCost(tempCardInfo.ability);
    }

    public void AddCardMethod(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        for (int i = 0; i < tempCardInfo.ability; i++)
        {
            //GameManager.instance.DrawCardFromDeck();
            //코루틴을 일반 메소드처럼 썻을때 상황
        }
    }
    public void HealMethod(Player player, CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        tempPlayer = player;
        Vector2 pos = tempPlayer.transform.position;
        PlayerEffectMethod(pos);
        
    }
    #endregion

    #region 이펙트실행
    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = tempCardInfo.attackEffect;
        Instantiate(prefab, position, prefab.transform.rotation);
        StartCoroutine(EndOfParticle(prefab));
        Debug.Log("이펙트 실행");
    }

    private void PlayerEffectMethod(Vector2 position)
    {

        GameObject prefab = tempCardInfo.effect;
        Instantiate(prefab, position, prefab.transform.rotation);
        StartCoroutine(EndOfParticle(prefab));

    }
    #endregion


    public IEnumerator EndOfParticle(GameObject particle)
    {
        
        yield return new WaitForSecondsRealtime(particle.GetComponent<ParticleSystem>().main.duration);

        Destroy(particle);
    }
}
