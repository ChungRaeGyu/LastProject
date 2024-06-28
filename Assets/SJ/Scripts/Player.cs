using UnityEngine;

public class Player : Character
{
    public GameObject arrowPrefab;

    public Player(int health, int defense, int attackPower, float actionGauge) : base(health, defense, attackPower, actionGauge)
    {
        
    }

    public override void Attack(Character target)
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.Shoot(target, stats.attackPower);

        target.TakeDamage(stats.attackPower);
    }
}