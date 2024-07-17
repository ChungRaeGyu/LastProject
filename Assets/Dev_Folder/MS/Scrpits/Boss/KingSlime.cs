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
    private CountPos countPos;

    [SerializeField] private Condition defenseconditionPrefab; // 프리팹을 설정할 수 있도록 SerializeField 추가

    private List<Condition> conditionInstances = new List<Condition>();
    public float conditionSpacing = 1f; // 각 컨디션 간의 간격

    private void Start()
    {
        Canvas canvas = UIManager.instance.healthBarCanvas;
        if (canvas != null && healthBarPrefab != null)
        {
            // healthBarPrefab을 canvas의 자식으로 생성
            healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
            healthBarInstance.Initialized(monsterStats.maxhealth, monsterStats.maxhealth, transform.GetChild(1));
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
        StartCoroutine(MonsterTurn());
    }

    public override IEnumerator MonsterTurn()
    {
        bossTurnCount++;
        Debug.Log("----- 보스의 " + bossTurnCount + "턴 째 -----");
        if (monsterStats.maxhealth < monsterStats.maxhealth / 2 && !bossHeal) // 피 반 이하로 떨어질 때 30 회복 '한 번'만 하기
        {
            monsterStats.maxhealth += 30;
            bossHeal = true;
            Debug.Log(this.name + "이" + 30 + "만큼 회복했다!");
        }

        if (bossTurnCount <= 4 && !strongAttack) // 3턴동안 공격력 2배 공격
        {
            GameManager.instance.player.TakeDamage(monsterStats.attackPower * 2);
            strongAttack = true;
            Debug.Log(this.name + "초반 공격" + monsterStats.attackPower * 2 + "데미지");
        }

        else if (bossTurnCount % 10 == 0) // 10턴 뒤 공격력 3배 공격
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
