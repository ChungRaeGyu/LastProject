using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private int monsterTurn = 0;
    // ���� ������ ������ �ٲ�� ���� ���� ������ �ʵ�
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
        monsterTurn++;
        attackRandomValue = Random.Range(0, 100);

        if (monsterTurn % 4 == 0)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{Mathf.FloorToInt(monsterStats.attackPower * 1.2f)}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n�� ���� </color> �⸦ ������ �ֽ��ϴ�.";
        //else
        //    attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����Ϸ��� �մϴ�.";
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

            if (monsterTurn % 4 == 0)
            {
                yield return PerformAttack(Mathf.FloorToInt(monsterStats.attackPower * 1.2f));
            }
            else
            {
                yield return PerformAttack(0);
            }
        }

        yield return new WaitForSeconds(monsterTurnDelay); // ������ ���� ���

        monsterTurn++;
        attackRandomValue = Random.Range(0, 100);

        if (monsterTurn % 4 == 0)
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{Mathf.FloorToInt(monsterStats.attackPower * 1.2f)}</color>�� ���ط� �����Ϸ��� �մϴ�.";
        else
            attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n�� ���� </color> �⸦ ������ �ֽ��ϴ�.";
        //else
        //    attackDescriptionText.text = $"<color=#FF7F50><size=30><b>����</b></size></color>\n �� ���� <color=#FFFF00>{monsterStats.attackPower}</color>�� ���ط� �����Ϸ��� �մϴ�.";
    }

 
}