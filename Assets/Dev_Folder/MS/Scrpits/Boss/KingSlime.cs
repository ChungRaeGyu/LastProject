using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;
    private int bossTurnCount = 0;
    private bool bossHeal = false;
    private bool strongAttack = false;
    private System.Random random = new System.Random();

    private void Start()
    {
        Canvas canvas = UIManager.Instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab을 canvas의 자식으로 생성
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth, transform.GetChild(1));
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
        bossTurnCount++;
        Debug.Log("----- 보스의 " + bossTurnCount + "턴 째 -----");
        if (monsterStats.maxhealth < 50 && !bossHeal) // 피 50 이하로 떨어질 때 30 회복 '한 번'만 하기
        {
            monsterStats.maxhealth += 30;
            bossHeal = true;
            Debug.Log(this.name + "이" + 30 + "만큼 회복했다!");
        }

        if (bossTurnCount <= 4) // 3턴동안 공격력 2배 공격
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
            strongAttack = true;
            Debug.Log(this.name + "초반 공격" + monsterStats.attackPower * 2 + "데미지");
        }

        else if (bossTurnCount % 10 == 0) // 10턴 뒤 확률로 공격력 2배 공격
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 3);
            Debug.Log(this.name + "이 강한 공격을 했다!" + monsterStats.attackPower * 3 + "데미지");
        }

        else if (random.Next(0, 100) < 15) // 15% 확률로 공격력 2배 공격
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
            Debug.Log(this.name + "이 일정 확률로 강한공격!");
        }

        else // 기본공격
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower);
        }
        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        if (animator != null) // 애니메이션
        {
            animator.SetTrigger("Attack");
        }
        // 공격 후에 필요한 다른 동작

        // 공격 후에 다음 턴을 위해 GameManager에 알림
        GameManager.instance.EndMonsterTurn();
    }
}
