using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonsterCharacter
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
        //������ �Ǵ� �ǰ�?
        Debug.Log("StartMonsterTurn���� : " + IsDead());

        StartCoroutine(Turn());
    }

    public override IEnumerator Turn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        //counter++;
        //if(counter >= 2 && !counterOnOff) // 2�Ͽ� ����
        //{
        //    Debug.Log(this.name + "����� �� �ɾ���! " + 3 + " �� �������� �Ծ���!");
        //    counterOnOff = true; // ����� Ȱ��ȭ
        //    GameManager.instance.player.TakeDamage(playerStats.maxhealth - 3); // player�ʿ� �̹��� ����
        //    if(counter >= 5 && !counterOnOff) // 2,3,4 �� ���� ��Ʈ ������
        //    {
        //        counterOnOff = false; // ����� ��Ȱ��ȭ
        //        counter = 0; // ī���� �ʱ�ȭ �ٽ� 0�Ϻ���
        //        Debug.Log(this.name + "����� ��! ");
        //    }
        //}
        monsterNextAction.gameObject.SetActive(false);

        // �ൿ �̹����� ������ ��

        yield return new WaitForSeconds(1f); // ������ ���� ���

        GameManager.instance.player.TakeDamage(monsterStats.attackPower);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(1f); // ������ ���� ���

    }


}
