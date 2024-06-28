using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed = 10f; // 화살의 이동 속도
    private Character target; // 공격 대상
    private int damage; // 공격력

    public void Shoot(Character target, int damage)
    {
        this.target = target;
        this.damage = damage;

        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = targetDirection * speed;

        Destroy(gameObject, 2f); // 2초 후에 화살 삭제
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target.gameObject)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
