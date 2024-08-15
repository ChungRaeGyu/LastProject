using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameGolem : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
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

        util1DescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n <color=#FFFF00>2</color>�ϸ��� ���ݷ��� <color=#FFFF00>1</color>�� �����մϴ�.";

        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 15)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 3}</color>�� ���ط� �����ϰ�, <color=#FFFF00>{5}</color>�� ���� ���ظ� �ַ��� �մϴ�.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����ϰ�, {baseAttackPower}��ŭ ü���� �����մϴ�.";
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

            if (monsterTurn % 2 == 0) // 2�ϸ��� ���ݷ� 1 ���
            {
                monsterStats.attackPower += 1;
            }

            if (attackRandomValue < 15) // 15% Ȯ���� ���ݷ� 3�� ����
            {
                yield return PerformAttack(monsterStats.attackPower * 3);
                Debug.Log(this.name + "�� ���Ѱ���!");
            }
            else
            {
                yield return PerformAttack(monsterStats.attackPower);
                currenthealth += baseAttackPower;
            }
        }

        yield return new WaitForSeconds(0.5f); // ������ ���� ���

        monsterTurn++;
        attackRandomValue = random.Next(0, 100);

        if (attackRandomValue < 15)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower * 3}</color>�� ���ط� �����Ϸ��� �մϴ�."; // <color=#FFFF00>{5}</color>�� ���� ���ظ� �ַ��� �մϴ�.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����Ϸ��� �մϴ�."; // {baseAttackPower}��ŭ ü���� �����մϴ�.";
    }

}