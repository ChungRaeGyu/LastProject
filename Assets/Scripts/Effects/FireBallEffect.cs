using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallEffect : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y+4.5f);
            Destroy(gameObject);
            Instantiate(explosion, position, explosion.transform.rotation);
            //터지는 이펙트 생성하기.. 
        }
    }
}
