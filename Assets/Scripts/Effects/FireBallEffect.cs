using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
           
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Destroy(gameObject);
            //터지는 이펙트 생성하기.. 
        }
    }
}
