using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    //AnimationEffectManager
    //마법과 물리의 차이
    //마법 : 캐스팅 후 스킬 실행
    //물리 : 플레이어와 몬스터 이펙트 동시 실행
    public RangeAttackSystem RangeAttack;
    public CardBasic tempCardInfo;
    public Vector2 playerEffectPos;

    public void PhysicalAttack(CardBasic cardBasic, MonsterCharacter targetMonster=null)
    {
        //호출될 메소드 //범위공격, 단일공격 포함
        tempCardInfo = cardBasic;
        PlayerEffectMethod(GetPos()); //플레이어의 공격 이펙트
        if(targetMonster == null)
        {
            //범위공격
            List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters);
            foreach (MonsterCharacter monster in monsters)
            {
                if(monster.currenthealth>0)
                    monster.TakeDamage(tempCardInfo.damageAbility);
            }
        }
        else
        {
            //단일 공격
            AttackEffectMethod(targetMonster.transform.position);
            targetMonster.TakeDamage(tempCardInfo.damageAbility);
        }
    }


    public void MagicAttack(CardBasic cardBasic, MonsterCharacter targetMonster=null)
    {
        //호출될 메소드

        tempCardInfo = cardBasic;
        StartCoroutine(MagicAttack(targetMonster));
    }
    IEnumerator MagicAttack(MonsterCharacter targetMonster = null)
    {
        //캐스팅 후 공격을 위한 코루틴
        PlayerEffectMethod(GetPos());
        yield return new WaitForSecondsRealtime(0.5f);
        if (targetMonster == null)
        {

            //범위공격
            List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // 복제
            RangeAttack.AttackAnim(tempCardInfo);
            foreach (MonsterCharacter monster in monsters)
            {
                monster.TakeDamage(tempCardInfo.damageAbility);
            }
        }
        else
        {
            //단일공격
            AttackEffectMethod(targetMonster.transform.position);
            targetMonster.TakeDamage(tempCardInfo.damageAbility);
        }
    }
    public void Buff(CardBasic cardBasic)
    {
        //호출될 메소드

        tempCardInfo = cardBasic;
        PlayerEffectMethod(GameManager.instance.player.transform.position);
    }
    public void Debuff(MonsterCharacter targetMonster, CardBasic cardBasic)
    {
        //호출될 메소드

        tempCardInfo = cardBasic;
        StartCoroutine(DeBuffCoroutine(false, targetMonster));
    }

    IEnumerator DeBuffCoroutine(bool isRange, MonsterCharacter targetMonster = null)
    {
        PlayerEffectMethod(GetPos());
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
            DebuffEffectMethod(targetMonster);
        }
    }
    private void DebuffEffectMethod(MonsterCharacter monster)
    {
        GameObject prefab = tempCardInfo.debuffEffectPrefab;
        if (monster.deBuff == null)
            monster.deBuff = Instantiate(prefab, monster.transform.position, prefab.transform.rotation);
    }

    private void PlayerEffectMethod(Vector2 position)
    {
        //기술사용 이펙트 소환 , Heal(버프), 마법진
        GameObject prefab = tempCardInfo.playerEffect;
        GameObject tempPrefab = Instantiate(prefab, position, prefab.transform.rotation);
    }

    public void AttackEffectMethod(Vector2 position)
    {
        //공격 이펙트 소환
        GameObject prefab = tempCardInfo.attackEffect;
        GameObject tempPrefab = Instantiate(prefab, position, prefab.transform.rotation);
    }

    private Vector2 GetPos()
    {
        return new Vector2(GameManager.instance.player.transform.position.x, -2.4f);
    }


    //코루틴 받아와서 사용하기
    public void RandomAttackCoroutine(CardBasic card)
    {
        StartCoroutine(Coroutine(card));
    }
    IEnumerator Coroutine(CardBasic cardBasic)
    {
        for (int i = 0; i < cardBasic.utilAbility; i++)
        {
            List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // 복제
            if (monsters.Count == 0) yield break;
            int num = CatchNum(monsters);
            
            GameObject tempPrefab = Instantiate(cardBasic.attackEffect, monsters[num].transform.position, cardBasic.attackEffect.transform.rotation);
            monsters[num].TakeDamage(cardBasic.damageAbility);
            yield return new WaitForSecondsRealtime(0.2f);
        }

    }
    private int CatchNum(List<MonsterCharacter> monsters)
    {
        int returnNum = 0;
        while (true)
        {
            int num = Random.Range(0, monsters.Count);
            if (monsters[num].currenthealth > 0)
            {
                returnNum = num;
                break;
            }
        }

        return returnNum;
    }
}
