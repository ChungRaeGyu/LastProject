using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    //AnimationEffectManager
    //������ ������ ����
    //���� : ĳ���� �� ��ų ����
    //���� : �÷��̾�� ���� ����Ʈ ���� ����
    public RangeAttackSystem RangeAttack;
    public CardBasic tempCardInfo;
    public Vector2 playerEffectPos;

    public void PhysicalAttack(CardBasic cardBasic, MonsterCharacter targetMonster=null)
    {
        //ȣ��� �޼ҵ� //��������, ���ϰ��� ����
        tempCardInfo = cardBasic;
        PlayerEffectMethod(GetPos()); //�÷��̾��� ���� ����Ʈ
        if(targetMonster == null)
        {
            //��������
            List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters);
            foreach (MonsterCharacter monster in monsters)
            {
                if(monster.currenthealth>0)
                    monster.TakeDamage(tempCardInfo.damageAbility);
            }
        }
        else
        {
            //���� ����
            AttackEffectMethod(targetMonster.transform.position);
            targetMonster.TakeDamage(tempCardInfo.damageAbility);
        }
    }


    public void MagicAttack(CardBasic cardBasic, MonsterCharacter targetMonster=null)
    {
        //ȣ��� �޼ҵ�

        tempCardInfo = cardBasic;
        StartCoroutine(MagicAttack(targetMonster));
    }
    IEnumerator MagicAttack(MonsterCharacter targetMonster = null)
    {
        //ĳ���� �� ������ ���� �ڷ�ƾ
        PlayerEffectMethod(GetPos());
        yield return new WaitForSecondsRealtime(0.5f);
        if (targetMonster == null)
        {

            //��������
            List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // ����
            RangeAttack.AttackAnim(tempCardInfo);
            foreach (MonsterCharacter monster in monsters)
            {
                monster.TakeDamage(tempCardInfo.damageAbility);
            }
        }
        else
        {
            //���ϰ���
            AttackEffectMethod(targetMonster.transform.position);
            targetMonster.TakeDamage(tempCardInfo.damageAbility);
        }
    }
    public void Buff(CardBasic cardBasic)
    {
        //ȣ��� �޼ҵ�

        tempCardInfo = cardBasic;
        PlayerEffectMethod(GameManager.instance.player.transform.position);
    }
    public void Debuff(MonsterCharacter targetMonster, CardBasic cardBasic)
    {
        //ȣ��� �޼ҵ�

        tempCardInfo = cardBasic;
        StartCoroutine(DeBuffCoroutine(false, targetMonster));
    }

    IEnumerator DeBuffCoroutine(bool isRange, MonsterCharacter targetMonster = null)
    {
        PlayerEffectMethod(GetPos());
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
        //������ ����Ʈ ��ȯ , Heal(����), ������
        GameObject prefab = tempCardInfo.playerEffect;
        GameObject tempPrefab = Instantiate(prefab, position, prefab.transform.rotation);
    }

    public void AttackEffectMethod(Vector2 position)
    {
        //���� ����Ʈ ��ȯ
        GameObject prefab = tempCardInfo.attackEffect;
        GameObject tempPrefab = Instantiate(prefab, position, prefab.transform.rotation);
    }

    private Vector2 GetPos()
    {
        return new Vector2(GameManager.instance.player.transform.position.x, -2.4f);
    }


    //�ڷ�ƾ �޾ƿͼ� ����ϱ�
    public void RandomAttackCoroutine(CardBasic card)
    {
        StartCoroutine(Coroutine(card));
    }
    IEnumerator Coroutine(CardBasic cardBasic)
    {
        for (int i = 0; i < cardBasic.utilAbility; i++)
        {
            List<MonsterCharacter> monsters = new List<MonsterCharacter>(GameManager.instance.monsters); // ����
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
