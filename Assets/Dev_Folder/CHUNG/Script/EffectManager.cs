using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{

    Player tempPlayer;
    CardBasic tempCardInfo;
    public void AttackMethod(Monster targetMonster, Player player,CardSO cardSO)
    {
        PlayerEffectMethod(tempPlayer.transform.position);
        AttackEffectMethod(targetMonster.transform.position);
    }

    public void MagicAttackMethod(Monster targetMonster, Player player,CardBasic cardBasic)
    {
        tempPlayer = player;
        tempCardInfo = cardBasic;
        //단일공격
        Debug.Log("코루틴 실행");
        StartCoroutine(MagicAttack(targetMonster));
        Debug.Log("코루틴 종료");
    }
    IEnumerator MagicAttack(Monster targetMonster)
    {
        Debug.Log("코루틴 실행중 0.5초전");

        PlayerEffectMethod(tempPlayer.transform.position);
        yield return new WaitForSeconds(1f);
        Debug.Log("코루틴 실행중 0.5초후");

        AttackEffectMethod(targetMonster.transform.position);
        targetMonster.TakeDamage(tempCardInfo.ability);

    }

    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = tempCardInfo.attackEffect;
        Instantiate(prefab, position, prefab.transform.rotation);
        Debug.Log("이펙트 실행");
    }

    private void PlayerEffectMethod(Vector2 position)
    {

        GameObject prefab = tempCardInfo.effect;
        Instantiate(prefab, position, prefab.transform.rotation);
    }
    public void RangeAttackMethod(CardBasic cardBasic)
    {
        tempCardInfo = cardBasic;
        foreach (Monster monster in GameManager.instance.monsters)
        {
            monster.TakeDamage(tempCardInfo.ability);
        }
    }
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
        //여긴 이펙트 말고 카드가 여러장 날라오는 느낌으로 애니메이션 만들기
        Vector2 pos = tempPlayer.transform.position;
        PlayerEffectMethod(pos);
        tempPlayer.currenthealth += tempCardInfo.ability;
    }


 
}
