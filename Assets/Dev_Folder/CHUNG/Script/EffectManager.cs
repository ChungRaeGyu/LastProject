using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{

    Player tempPlayer;
    CardSO tempCardSO;
    public void AttackMethod(Monster targetMonster, Player player,CardSO cardSO)
    {
        PlayerEffectMethod(tempPlayer.transform.position);
        AttackEffectMethod(targetMonster.transform.position);
    }

    public void MagicAttackMethod(Monster targetMonster, Player player, CardSO cardSO)
    {
        tempPlayer = player;
        tempCardSO = cardSO;
        //���ϰ���
        Debug.Log("�ڷ�ƾ ����");
        StartCoroutine(MagicAttack(targetMonster));
        Debug.Log("�ڷ�ƾ ����");
    }
    IEnumerator MagicAttack(Monster targetMonster)
    {
        Debug.Log("�ڷ�ƾ ������ 0.5����");

        PlayerEffectMethod(tempPlayer.transform.position);
        yield return new WaitForSeconds(1f);
        Debug.Log("�ڷ�ƾ ������ 0.5����");

        AttackEffectMethod(targetMonster.transform.position);
        targetMonster.TakeDamage(tempCardSO.ability);

    }

    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = tempCardSO.attackEffect;
        Instantiate(prefab, position, prefab.transform.rotation);
        Debug.Log("����Ʈ ����");
    }

    private void PlayerEffectMethod(Vector2 position)
    {

        GameObject prefab = tempCardSO.effect;
        Instantiate(prefab, position, prefab.transform.rotation);
    }
    public void RangeAttackMethod(CardSO cardSO)
    {
        tempCardSO = cardSO;
        foreach (Monster monster in GameManager.instance.monsters)
        {
            monster.TakeDamage(tempCardSO.ability);
        }
    }
    public void AddCostMethod(CardSO cardSO)
    {
        tempCardSO = cardSO;
        PlayerEffectMethod(tempPlayer.transform.position);
        tempPlayer.AddCost(tempCardSO.ability);
    }

    public void AddCardMethod(CardSO cardSO)
    {
        tempCardSO = cardSO;
        for (int i = 0; i < tempCardSO.ability; i++)
        {
            GameManager.instance.DrawCardFromDeck();
        }
    }
    public void HealMethod(Player player,CardSO cardSO)
    {
        tempCardSO = cardSO;
        tempPlayer = player;
        //���� ����Ʈ ���� ī�尡 ������ ������� �������� �ִϸ��̼� �����
        Vector2 pos = tempPlayer.transform.position;
        PlayerEffectMethod(pos);
        tempPlayer.currenthealth += tempCardSO.ability;
    }


 
}
