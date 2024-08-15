using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerGoblin : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    //private int monsterTurn = 0;
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

        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{5}</color>�� ���� ���ظ� �ַ��� �մϴ�.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����Ϸ��� �մϴ�.";
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

            yield return new WaitForSeconds(0.5f); // ������ ���� ���

            if (attackRandomValue < 10) // 10% Ȯ���� ����
            {
                yield return PerformAttack(5);
                GameManager.instance.player.BleedingForTurns(2);

                Debug.Log(this.name + " ������� �ɾ���! " + 5 + " �� ���� �������� �Ծ���!");
                //attackDescriptionObject.SetActive(false);
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);
            }
        }

        yield return new WaitForSeconds(0.5f); // ������ ���� ���

        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 10)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{5}</color>�� ���� ���ظ� �ַ��� �մϴ�.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����Ϸ��� �մϴ�.";
    }

}