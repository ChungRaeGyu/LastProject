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
            // healthBarPrefab을 canvas의 자식으로 생성
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
        //지워도 되는 건가?
        Debug.Log("StartMonsterTurn실행 : " + IsDead());

        StartCoroutine(Turn());
    }

    public override IEnumerator Turn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        //counter++;
        //if(counter >= 2 && !counterOnOff) // 2턴에 시작
        //{
        //    Debug.Log(this.name + "디버프 를 걸었다! " + 3 + " 의 데미지를 입었다!");
        //    counterOnOff = true; // 디버프 활성화
        //    GameManager.instance.player.TakeDamage(playerStats.maxhealth - 3); // player쪽에 이미지 띄우기
        //    if(counter >= 5 && !counterOnOff) // 2,3,4 턴 까지 도트 데미지
        //    {
        //        counterOnOff = false; // 디버프 비활성화
        //        counter = 0; // 카운터 초기화 다시 0턴부터
        //        Debug.Log(this.name + "디버프 끝! ");
        //    }
        //}
        monsterNextAction.gameObject.SetActive(false);

        // 행동 이미지에 연출을 줌

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

        GameManager.instance.player.TakeDamage(monsterStats.attackPower);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(1f); // 연출을 위한 대기

    }


}
