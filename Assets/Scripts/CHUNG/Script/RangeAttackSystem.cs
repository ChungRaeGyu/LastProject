using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackSystem : MonoBehaviour
{
    

    public void AttackAnim(CardBasic cardBasic)
    {
        for (int i = 0; i < 10; i++)
        {
            float delay = Random.Range(0f, 1f);
            float random = Random.Range(-8, 0);
            StartCoroutine(AttackMeteo(cardBasic, delay, random));
            
            //TODO : ¶¥¿¡ ºÎµúÇûÀ» ¶§ ÅÍÁö´Â ÀÌÆåÆ®µµ ³Ö¾î¾ß°Ú´Ù.
        }
    }

    IEnumerator AttackMeteo(CardBasic cardBasic,float delay, float random)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameObject meteo = Instantiate(cardBasic.attackEffect, new Vector2(random, 0), cardBasic.attackEffect.transform.rotation);
        StartCoroutine(GameManager.instance.effectManager.EndOfParticle(meteo));
        meteo.SetActive(true);

    }
}
