using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackSystem : MonoBehaviour
{
    float random;
    private void OnEnable()
    {
        for(int i=0; i<10; i++)
        {
            random = Random.Range(-8, 0);
            //Instantiate() cardso에서 이펙트를 불러와서 소환한다.
        }
    }
}
