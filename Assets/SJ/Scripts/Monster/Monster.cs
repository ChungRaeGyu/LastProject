using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Monster : MonsterCharacter
{
    public Slider healthSlider;
    public int Currenthealth => currenthealth;
    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = monsterStats.maxhealth;
            healthSlider.value = Currenthealth;
        }
    }

    public void StartMonsterTurn()
    {
        StartCoroutine(MonsterTurn());
    }

    public IEnumerator MonsterTurn()
    {
        GameManager.instance.player.TakeDamage(monsterStats.attackPower);
        yield return new WaitForSeconds(1f); // ������ ���� ���

        // ���� �Ŀ� �ʿ��� �ٸ� ����

        // ���� �Ŀ� ���� ���� ���� GameManager�� �˸�
        GameManager.instance.EndMonsterTurn();
    }

    public void ResetHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = Currenthealth;
        }
    }

    private void Update()
    {
        ResetHealthSlider();
    }
}
