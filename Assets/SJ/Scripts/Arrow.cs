using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed = 10f; // ȭ���� �̵� �ӵ�
    private Character target; // ���� ���
    private int damage; // ���ݷ�

    public void Shoot(Character target, int damage)
    {
        this.target = target;
        this.damage = damage;

        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = targetDirection * speed;

        Destroy(gameObject, 2f); // 2�� �Ŀ� ȭ�� ����
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
