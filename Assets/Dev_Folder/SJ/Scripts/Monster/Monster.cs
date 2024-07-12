using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Monster : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private PlayerStats playerStats;

    private System.Random random = new System.Random();

    private int counter = 0;  // 카운트 변수
    private bool counterOnOff = false;

    private void Start()
    {
        Canvas canvas = GameManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(0, 6); // 0~5 까지 랜덤 hp상승 효과

            // healthBarPrefab을 canvas의 자식으로 생성
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
        if(counter >= 2 && !counterOnOff) // 2턴에 시작
        {
            Debug.Log(this.name + "디버프 를 걸었다! " + 3 + " 의 데미지를 입었다!");
            counterOnOff = true; // 디버프 활성화
            GameManager.instance.player.TakeDamage(playerStats.maxhealth - 3); // player쪽에 이미지 띄우기
            if(counter >= 5 && !counterOnOff) // 2,3,4 턴 까지 도트 데미지
            {
                counterOnOff = false; // 디버프 비활성화
                counter = 0; // 카운터 초기화 다시 0턴부터
                Debug.Log(this.name + "디버프 끝! ");
            }
        }
        GameManager.instance.player.TakeDamage(monsterStats.attackPower);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        // 공격 후에 필요한 다른 동작

        // 공격 후에 다음 턴을 위해 GameManager에 알림
        GameManager.instance.EndMonsterTurn();
    }
}
