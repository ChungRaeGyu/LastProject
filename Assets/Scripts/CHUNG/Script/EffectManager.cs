using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    //������ ������ ����
    //���� : ĳ���� �� ��ų ����
    //���� : �÷��̾�� ���� ����Ʈ ���� ����
    public RangeAttackSystem RangeAttack;
    public CardBasic tempCardInfo;
    public Vector2 playerEffectPos;
    #region ��������
    public void AttackMethod(MonsterCharacter targetMonster,CardBasic cardSO)
    {
        //���� �Ⱦ��� ��
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


    #region ����
    public void FrozemMagic(MonsterCharacter targetMonster, CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        //���ϰ���
        StartCoroutine(DeBuffCoroutine(false, targetMonster));
    }

    IEnumerator DeBuffCoroutine(bool isRange, MonsterCharacter targetMonster = null)
    {
        PlayerEffectMethod_Image(GetPos());
        yield return new WaitForSeconds(1f);

        List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // ����

        if (isRange)
        {
            //RangeAttack.AttackAnim(tempCardInfo);�ҵ��� �Ѱ��� �������� �ҷ��� �������.
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



    #region ��������
    public void MagicAttackMethod(MonsterCharacter targetMonster, CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        //���ϰ���
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

        List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // ����

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
    #region ����Ʈ ȣ��
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
    #region ����Ʈ����(��ƼŬ)
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
    #region ����Ʈ����(�̹��� �ִϸ��̼�)
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
