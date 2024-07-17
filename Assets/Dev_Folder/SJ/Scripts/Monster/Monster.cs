using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonsterCharacter
{
    public HpBar healthBarPrefab;
    private HpBar healthBarInstance;

    private System.Random random = new System.Random();

    [SerializeField] private Condition defenseconditionPrefab; // 프리팹을 설정할 수 있도록 SerializeField 추가

    private List<Condition> conditionInstances = new List<Condition>();
    public float conditionSpacing = 1f; // 각 컨디션 간의 간격

    private void Start()
    {
        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            int hpUp = random.Next(0, 6); // 0~5 까지 랜덤 hp상승 효과

            // healthBarPrefab을 canvas의 자식으로 생성
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth + hpUp, monsterStats.maxhealth + hpUp, transform.GetChild(1));
        }

        AddCondition(UIManager.instance.conditionCanvas.transform, monsterStats.defense);
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

        StartCoroutine(MonsterTurn());
    }

    public override IEnumerator MonsterTurn()
    {
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
        GameManager.instance.player.TakeDamage(monsterStats.attackPower);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(2f); // 연출을 위한 대기

        // 공격 후에 필요한 다른 동작

        // 공격 후에 다음 턴을 위해 GameManager에 알림
        GameManager.instance.EndMonsterTurn();
    }

    // 새로운 Condition 인스턴스를 생성하고 리스트에 추가한 후, 위치를 업데이트
    public void AddCondition(Transform parent, int initialStackCount)
    {
        if (defenseconditionPrefab != null)
        {
            Condition newCondition = Instantiate(defenseconditionPrefab, parent);
            conditionInstances.Add(newCondition);
            UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, newCondition.transform); // 위치 초기화 후에 스택 값 설정
        }
    }

    // 리스트에서 Condition 인스턴스를 제거하고 위치를 업데이트
    public void RemoveCondition(Condition condition)
    {
        if (conditionInstances.Contains(condition))
        {
            conditionInstances.Remove(condition);
            Destroy(condition.gameObject);
            UpdateConditionPositions();
        }
    }

    // 모든 Condition 인스턴스를 제거 (모든 해로운 효과 한번에 제거용도, 안써도 됨)
    public void ClearConditions()
    {
        foreach (var condition in conditionInstances)
        {
            Destroy(condition.gameObject);
        }
        conditionInstances.Clear();
    }

    // 각 Condition의 위치를 transform.GetChild(2)를 기준으로 우측으로 하나씩 나열 (위치 업데이트 용도)
    public void UpdateConditionPositions()
    {
        for (int i = 0; i < conditionInstances.Count; i++)
        {
            Vector3 newPosition = transform.GetChild(2).position + new Vector3(conditionSpacing * i, 0, 0);
            conditionInstances[i].transform.position = newPosition;
        }
    }

    public void UpdateConditions()
    {
        foreach (var condition in conditionInstances)
        {
            // Condition 업데이트 로직 구현
        }
    }

    protected override void Die()
    {
        GameManager.instance.RemoveMonsterDead(this);

        base.Die();
    }

}
