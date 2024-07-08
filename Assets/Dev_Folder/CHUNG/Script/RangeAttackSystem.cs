using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackSystem : MonoBehaviour
{
    float random;

    public void AttackAnim(CardBasic cardBasic)
    {
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(AttackMeteo(cardBasic));
            
            //TODO : ¶¥¿¡ ºÎµúÇûÀ» ¶§ ÅÍÁö´Â ÀÌÆåÆ®µµ ³Ö¾î¾ß°Ú´Ù.
        }
    }

    IEnumerator AttackMeteo(CardBasic cardBasic)
    {
        float delay = Random.Range(0f, 1f);
        yield return new WaitForSecondsRealtime(delay);
        random = Random.Range(-8, 0);
        Instantiate(cardBasic.attackEffect, new Vector2(random, transform.position.y), cardBasic.attackEffect.transform.rotation);

    }
}
