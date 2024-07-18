using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    //마법과 물리의 차이
    //마법 : 캐스팅 후 스킬 실행
    //물리 : 플레이어와 몬스터 이펙트 동시 실행
    public RangeAttackSystem RangeAttack;
    public CardBasic tempCardInfo;
    public Vector2 playerEffectPos;
    #region 물리공격
    public void AttackMethod(MonsterCharacter targetMonster,CardBasic cardSO)
    {
        //현재 안쓰는 중
        PlayerEffectMethod(GetPos());
        AttackEffectMethod(targetMonster.transform.position);

    }
    public void RangeAttackMethod(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        foreach (MonsterCharacter monster in GameManager.instance.monsters)
        {
            monster.TakeDamage(tempCardInfo.ability);
        }
    }
    #endregion
    #region 마법공격
    public void MagicAttackMethod(MonsterCharacter targetMonster, CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        //단일공격
        StartCoroutine(MagicAttack(false,targetMonster));
    }
    public void MagicRangeAttackMethod(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        StartCoroutine(MagicAttack(true));
    }
    IEnumerator MagicAttack(bool isRange,MonsterCharacter targetMonster=null)
    {
        PlayerEffectMethod(GetPos());
        yield return new WaitForSeconds(0f);

        List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // 복제

        if (isRange)
        {
            RangeAttack.AttackAnim(tempCardInfo);
            foreach (MonsterCharacter monster in monsters)
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
    #region 이펙트 호출
    public void PlayerEffect(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        PlayerEffectMethod(GetPos());

    }
    #endregion
    #region 이펙트실행
    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = tempCardInfo.attackEffect;
        GameObject tempPrefab = Instantiate(prefab, position, prefab.transform.rotation);
        if (prefab.name == "lightingAttack") return;
        StartCoroutine(EndOfParticle(tempPrefab));
    }

    private void PlayerEffectMethod(Vector2 position)
    {
        GameObject prefab = tempCardInfo.effect;
        GameObject tempPrefab = Instantiate(prefab, position, prefab.transform.rotation);
        StartCoroutine(EndOfParticle(tempPrefab));

    }
    #endregion


    public IEnumerator EndOfParticle(GameObject particle)
    {
        if (particle.TryGetComponent<ParticleSystem>(out var particleSystem)){
            yield return new WaitForSecondsRealtime(particleSystem.main.duration);
        }else
        {
            particleSystem = particle.GetComponentInChildren<ParticleSystem>();
            yield return new WaitForSecondsRealtime(particleSystem.main.duration);
        }
        //yield return new WaitForSecondsRealtime(particleSystem.main.duration);
        DestroyImmediate(particle);
    }

    private Vector2 GetPos()
    {
        return new Vector2(GameManager.instance.player.transform.position.x, -2.4f);
    }
}
