using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int attackRandomValue;

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

        attackRandomValue = random.Next(6, 10);

        attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{attackRandomValue}</color>�� ���ط� �����Ϸ��� �մϴ�.";
    }

    protected override void Update()
    {
        base.Update();
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
        StartCoroutine(Turn());
    }

    public override IEnumerator Turn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        // �θ� Ŭ������ MonsterTurn�� ȣ���Ͽ� �󸮴� ȿ�� ����
        yield return base.Turn();


        if (!isFrozen)
        {
            if (isDead) yield break;
            monsterNextAction.gameObject.SetActive(false);

            // �ൿ �̹����� ������ ��

            yield return new WaitForSeconds(monsterTurnDelay); // ������ ���� ���

            yield return PerformAttack(attackRandomValue);
        }

        yield return new WaitForSeconds(monsterTurnDelay); // ������ ���� ���

        attackRandomValue = random.Next(6, 10);

        attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{attackRandomValue}</color>�� ���ط� �����Ϸ��� �մϴ�.";
    }
}