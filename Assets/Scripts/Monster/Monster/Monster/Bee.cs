using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private new void Start()
    {
        base.Start();

        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(currenthealth, currenthealth, hpBarPos);
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

        if (!isFrozen)
        {
            monsterNextAction.gameObject.SetActive(false);

            // �ൿ �̹����� ������ ��

            yield return new WaitForSeconds(1f); // ������ ���� ���

            if (random.Next(0, 100) < 15) // 15% Ȯ���� ���ݷ� 2�� ����
            {
                GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
                Debug.Log(this.name + "�� ���Ѱ���!");
            }
            else
            {
                GameManager.instance.player.TakeDamage(monsterStats.attackPower);
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
            }
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