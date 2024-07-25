using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            monster.TakeDamage(tempCardInfo.damageAbility);
        }
    }
    #endregion


    #region 빙결
    public void FrozemMagic(MonsterCharacter targetMonster, CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        //단일공격
        StartCoroutine(DeBuffCoroutine(false, targetMonster));
    }

    IEnumerator DeBuffCoroutine(bool isRange, MonsterCharacter targetMonster = null)
    {
        PlayerEffectMethod_Image(GetPos());
        yield return new WaitForSeconds(1f);

        List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // 복제

        if (isRange)
        {
            //RangeAttack.AttackAnim(tempCardInfo);불덩이 한개를 여러개를 불러서 만들었다.
            foreach (MonsterCharacter monster in monsters)
            {
                //GameManager.instance.deBuff = Instantiate(tempCardInfo.debuffEffectPrefab, targetMonster.transform.position, Quaternion.identity);

            }
        }
        else
        {
            DebuffEffectMethod_Image(targetMonster);
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
                monster.TakeDamage(tempCardInfo.damageAbility);
            }
        }
        else
        {
            AttackEffectMethod(targetMonster.transform.position);
            targetMonster.TakeDamage(tempCardInfo.damageAbility);
        }
    }
    #endregion
    #region 이펙트 호출
    public void PlayerEffect(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        PlayerEffectMethod(GetPos());

    }
    public void PlayerEffect_Image(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        PlayerEffectMethod_Image(GetPos());
    }
    #endregion
    #region 이펙트실행(파티클)
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
    #region 이펙트실행(이미지 애니메이션)
    private void PlayerEffectMethod_Image(Vector2 position)
    {
        GameObject prefab = tempCardInfo.effect;
        Instantiate(prefab, position, prefab.transform.rotation);
    }
    private void DebuffEffectMethod_Image(MonsterCharacter monster)
    {
        GameObject prefab = tempCardInfo.debuffEffectPrefab;
        if(monster.deBuff==null)
            monster.deBuff = Instantiate(prefab, monster.transform.position, prefab.transform.rotation);
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
