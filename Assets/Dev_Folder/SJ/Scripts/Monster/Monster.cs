using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Monster : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private PlayerStats playerStats;

    private System.Random random = new System.Random();

    private int counter = 0;  // ī��Ʈ ����
    private bool counterOnOff = false;

    private void Start()
    {
        Canvas canvas = GameManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(0, 6); // 0~5 ���� ���� hp��� ȿ��

            // healthBarPrefab�� canvas�� �ڽ����� ����
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth + hpUp, transform.GetChild(1));
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

    public IEnumerator MonsterTurn()
    {
        counter++;
        if(counter >= 2 && !counterOnOff) // 2�Ͽ� ����
        {
            Debug.Log(this.name + "����� �� �ɾ���! " + 3 + " �� �������� �Ծ���!");
            counterOnOff = true; // ����� Ȱ��ȭ
            GameManager.instance.player.TakeDamage(playerStats.maxhealth - 3); // player�ʿ� �̹��� ����
            if(counter >= 5 && !counterOnOff) // 2,3,4 �� ���� ��Ʈ ������
            {
                counterOnOff = false; // ����� ��Ȱ��ȭ
                counter = 0; // ī���� �ʱ�ȭ �ٽ� 0�Ϻ���
                Debug.Log(this.name + "����� ��! ");
            }
        }
        GameManager.instance.player.TakeDamage(monsterStats.attackPower);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(1f); // ������ ���� ���

        // ���� �Ŀ� �ʿ��� �ٸ� ����

        // ���� �Ŀ� ���� ���� ���� GameManager�� �˸�
        GameManager.instance.EndMonsterTurn();
    }
}
