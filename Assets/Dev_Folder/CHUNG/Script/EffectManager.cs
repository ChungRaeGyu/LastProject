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
        targetMonster.TakeDamage(tempCardSO.ability);

    }

    private void AttackEffectMethod(Vector2 position)
    {
        GameObject prefab = tempCardSO.attackEffect;
        Instantiate(prefab, position, prefab.transform.rotation);
        Debug.Log("이펙트 실행");
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
        //여긴 이펙트 말고 카드가 여러장 날라오는 느낌으로 애니메이션 만들기
        Vector2 pos = tempPlayer.transform.position;
        PlayerEffectMethod(pos);
        tempPlayer.currenthealth += tempCardSO.ability;
    }


 
}
