using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    //������ ������ ����
    //���� : ĳ���� �� ��ų ����
    //���� : �÷��̾�� ���� ����Ʈ ���� ����

    public RangeAttackSystem RangeAttack;
    Player tempPlayer;
    CardBasic tempCardInfo;

    #region ��������
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
    #region ��������
    public void MagicAttackMethod(Monster targetMonster, Player player,CardBasic cardBasic)
    {
        tempPlayer = player;
        tempCardInfo = cardBasic;
        //���ϰ���
        Debug.Log("�ڷ�ƾ ����");
        StartCoroutine(MagicAttack(false,targetMonster));
        Debug.Log("�ڷ�ƾ ����");
    }
    public void MagicRangeAttackMethod(Player player,CardBasic cardBasic)
    {
        tempPlayer = player;
        tempCardInfo = cardBasic;
        StartCoroutine(MagicAttack(true));
    }
    IEnumerator MagicAttack(bool isRange,Monster targetMonster=null)
    {
        Debug.Log("�ڷ�ƾ ������ 0.5����");

        PlayerEffectMethod(tempPlayer.transform.position);
        yield return new WaitForSeconds(1f);
        Debug.Log("�ڷ�ƾ ������ 0.5����");
        if (isRange)
        {
            RangeAttack.AttackAnim(tempCardInfo);
            foreach (Monster monster in GameManager.instance.monsters)
            {
                targetMonster.TakeDamage(tempCardInfo.ability);
            }
        }
        else
        {
            AttackEffectMethod(targetMonster.transform.position);
            targetMonster.TakeDamage(tempCardInfo.ability);
        }
    }
    #endregion
    #region ���� �� ��Ÿ �ɷ�(Player���Ը� ����Ʈ ����)
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
            //�ڷ�ƾ�� �Ϲ� �޼ҵ�ó�� ������ ��Ȳ
        }
    }
    public void HealMethod(Player player, CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        tempPlayer = player;
        Vector2 pos = tempPlayer.transform.position;
        PlayerEffectMethod(pos);
        tempPlayer.currenthealth += tempCardInfo.ability;
    }
    #endregion

    #region ����Ʈ����
    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = tempCardInfo.attackEffect;
        Instantiate(prefab, position, prefab.transform.rotation);
        Debug.Log("����Ʈ ����");
    }

    private void PlayerEffectMethod(Vector2 position)
    {

        GameObject prefab = tempCardInfo.effect;
        Instantiate(prefab, position, prefab.transform.rotation);
    }
    #endregion

}
