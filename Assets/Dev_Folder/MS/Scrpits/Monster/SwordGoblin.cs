using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGoblin : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private System.Random random = new System.Random();

    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(0, 6);

            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth + hpUp, monsterStats.maxhealth + hpUp, transform.GetChild(1));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (healthBarInstance != null)
        {
            healthBarInstance.ResetHealthSlider(currenthealth);
            healthBarInstance.UpdatehealthText();
        }
    }

    public void StartMonsterTurn()
    {
        StartCoroutine(MonsterTurn());
    }

    public override IEnumerator MonsterTurn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;
        // �θ� Ŭ������ MonsterTurn�� ȣ���Ͽ� �󸮴� ȿ�� ����
        yield return base.MonsterTurn();

        if (isFrozen) yield break;

        yield return new WaitForSeconds(1f); // ������ ���� ���

        GameManager.instance.player.TakeDamage(monsterStats.attackPower);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(1f); // ������ ���� ���

        GameManager.instance.EndMonsterTurn();
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }
}