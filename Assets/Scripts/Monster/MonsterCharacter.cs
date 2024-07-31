using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterCharacter : MonoBehaviour
{
    public MonsterStats monsterStats;
    public int baseAttackPower;
    public int currenthealth;

    public Animator animator;

    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int Attack = Animator.StringToHash("Attack");

    public List<Condition> conditionInstances = new List<Condition>();

    public Transform hpBarPos; // HP 바 위치
    public Transform conditionPos; // 컨디션 위치
    public Transform monsterNextActionPos; // 다음 행동을 나타낼 위치
    public Transform monsterNamePos; // 이름 위치

    private Transform MonsterCondition;
    public Transform monsterNextAction { get; set; }
    private Transform monsterName;
    private Transform monsterNextActionList;

    private AdjustHeightBasedOnText attackDescriptionAdjustHeight;
    protected TMP_Text attackDescriptionText;

    private AdjustHeightBasedOnText util1DescriptionAdjustHeight;
    protected TMP_Text util1DescriptionText;

    private AdjustHeightBasedOnText util2DescriptionAdjustHeight;
    protected TMP_Text util2DescriptionText;

    //디버프관련변수
    public int frozenTurnsRemaining = 0; // 얼린 상태가 유지될 턴 수
    public int weakerTurnsRemaining = 0; // 약화 상태가 유지될 턴 수
    public int defDownTurnsRemaining = 0; //방깍 상태가 유지될 턴 수 
    public int burnTurnsRemaining = 0; //화상
    public int poisonTurnsRemaining = 0; //중독 
    public int bleedingTurnsRemaining = 0; //출혈

    private float defDownValue;
    public bool isFrozen; // 얼었는지 확인하는 용도

    public bool boss;

    public System.Random random = new System.Random();

    [Header("DeBuff_InputScript")]
    public GameObject deBuff;

    public Action deBuffAnim;

    [Header("Description_NotCashing")]
    public Transform descriptionTransform;
    public GameObject attackDescriptionObject;
    public GameObject defenseDescriptionObject;
    public GameObject healDescriptionObject;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        currenthealth = monsterStats.maxhealth;
        // 몬스터에 랜덤한
        if (!boss)
        {
            int hpUp = random.Next(0, 10);
            currenthealth += hpUp;
        }
        baseAttackPower = monsterStats.attackPower;
        // ConditionBox 프리팹을 conditionCanvas의 자식으로 생성하고 playerCondition에 할당
        MonsterCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        AddCondition(MonsterCondition, monsterStats.defense, GameManager.instance.defenseconditionPrefab, ConditionType.Defense);

        monsterNextAction = Instantiate(GameManager.instance.attackActionPrefab, UIManager.instance.nextActionIconCanvas.transform).transform;
        monsterNextAction.gameObject.SetActive(false);

        monsterName = Instantiate(GameManager.instance.monsterNamePrefab, UIManager.instance.monsterNameCanvas.transform).transform;

        // TMP_Text 컴포넌트를 찾아서 몬스터 이름 설정
        TMP_Text nameText = monsterName.GetComponentInChildren<TMP_Text>();
        if (nameText != null)
        {
            nameText.text = monsterStats.monsterName;
        }
        
        monsterNextActionList = Instantiate(GameManager.instance.monsterNextActionListPrefab, UIManager.instance.nextActionDescriptionCanvas.transform).transform;

        // ActionDescriptionPrefab을 Description 오브젝트 안에 생성
        descriptionTransform = monsterNextActionList.GetChild(0); // GetChild(0)
        if (descriptionTransform != null)
        {
            // 공격 설명 프리팹
            attackDescriptionObject = Instantiate(GameManager.instance.actionDescriptionPrefab, descriptionTransform);
            attackDescriptionAdjustHeight = attackDescriptionObject.GetComponent<AdjustHeightBasedOnText>();
            attackDescriptionText = attackDescriptionAdjustHeight.childText;
            attackDescriptionText.text = $"";

            // 방어 설명 프리팹
            defenseDescriptionObject = Instantiate(GameManager.instance.actionDescriptionPrefab, descriptionTransform);
            util1DescriptionAdjustHeight = defenseDescriptionObject.GetComponent<AdjustHeightBasedOnText>();
            util1DescriptionText = util1DescriptionAdjustHeight.childText;
            util1DescriptionText.text = $"";

            // 회복 설명 프리팹
            GameObject healDescriptionObject = Instantiate(GameManager.instance.actionDescriptionPrefab, descriptionTransform);
            util2DescriptionAdjustHeight = healDescriptionObject.GetComponent<AdjustHeightBasedOnText>();
            util2DescriptionText = util2DescriptionAdjustHeight.childText;
            util2DescriptionText.text = $"";
        }
    }

    protected virtual void Update()
    {
        MonsterCondition.position = conditionPos.position;
        monsterNextAction.position = monsterNextActionPos.position;
        monsterName.position = monsterNamePos.position;
        monsterNextActionList.position = transform.position;
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
        actualDamage = (int)(defDownTurnsRemaining > 0 ? actualDamage * (1 + defDownValue) : actualDamage);
        currenthealth -= actualDamage;

        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        DieAction();
    }

    private void SpawnText(string text, Vector3 position, Color? color = null)
    {
        if (GameManager.instance.damageTextPrefab != null)
        {
            GameObject textInstance = Instantiate(GameManager.instance.damageTextPrefab, position, Quaternion.identity);
            DamageText damageText = textInstance.GetComponent<DamageText>();
            damageText.SetText(text);

            // 색상 설정
            if (color.HasValue)
            {
                damageText.currentColor = color.Value;
            }

            // 화면 좌표에서 위로 이동
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
            float yOffset = 200f; // 얼마나 위로 위치할지 설정
            Vector3 newScreenPosition = new Vector3(screenPosition.x, screenPosition.y + yOffset, 10f);
            textInstance.transform.position = Camera.main.ScreenToWorldPoint(newScreenPosition);
        }
    }

    private void SpawnDamageText(int damageAmount, Vector3 position)
    {
        SpawnText(damageAmount.ToString(), position);
    }

    private void SpawnConditionText(string conditionText, Vector3 position)
    {
        Color? textColor = conditionText == "빙결" ? new Color(0.53f, 0.81f, 0.92f) : (Color?)null;
        SpawnText(conditionText, position, textColor);
    }


    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual void Die()
    {
        if (isFrozen)
            GameManager.instance.DestroyDeBuffAnim(deBuff);

        if (monsterNextAction != null)
        {
            Destroy(monsterNextAction.gameObject);
        }

        if (monsterName != null)
        {
            Destroy(monsterName.gameObject);
        }

        if (monsterNextActionList = null)
        {
            Destroy(monsterNextActionList.gameObject);
        }

        Destroy(gameObject);
    }

    public virtual IEnumerator MonsterTurn()
    {
        if (GameManager.instance.player?.IsDead() == true) yield break;

        if (frozenTurnsRemaining > 0)
        {
            frozenTurnsRemaining--;
            Debug.Log($"{gameObject.name}는 얼어있습니다. 남은 얼린 턴 수: {frozenTurnsRemaining}");

            // SpawnDamageText로 "빙결" 텍스트 띄우도록 개조
            SpawnConditionText("빙결", transform.position);

            yield return new WaitForSeconds(2f); // 연출을 위한 대기
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            if (frozenTurnsRemaining == 0)
            {
                animator.StopPlayback();
                GameManager.instance.DestroyDeBuffAnim(deBuff); //얼음오브젝트 삭제 하는 곳
            }
            yield break;
        }
        else
        {
            isFrozen = false;
        }
        if (weakerTurnsRemaining > 0)
        {
            weakerTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Weaker);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            yield break;
        }
        else
        {
            monsterStats.attackPower = baseAttackPower;
        }
        if (defDownTurnsRemaining > 0)
        {
            weakerTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.DefDown);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            yield break;
        }
        if (burnTurnsRemaining > 0)
        {
            burnTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Burn);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(3);
        }
        if (poisonTurnsRemaining > 0)
        {
            poisonTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Poison);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(5);
        }
        if (bleedingTurnsRemaining > 0)
        {
            bleedingTurnsRemaining--;
            Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Bleeding);
            if (existingFrozenCondition != null)
            {
                existingFrozenCondition.DecrementStackCount(this);
            }
            TakeDamage(5);
        }
        yield return null;
    }

    protected IEnumerator PerformAttack(int damage, float attackDelay = 1.2f)
    {
        // 애니메이션 트리거
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(0.3f);

        GameManager.instance.player.TakeDamage(damage);

        //대기 시간
        yield return new WaitForSeconds(attackDelay);
    }

    #region 디버프
    public virtual void FreezeForTurns(int turns)
    {
        isFrozen = true;
        frozenTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Frozen);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.frozenConditionPrefab, ConditionType.Frozen);
        }
        Debug.Log($"{gameObject.name}가 {turns}턴 동안 얼렸습니다. 남은 얼린 턴 수: {frozenTurnsRemaining}");
    }
    public void WeakForTurns(int turns, float ability)
    {
        //약화 : 몬스터의 공격력이 약해진다.
        weakerTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Weaker);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.weakerConditionPrefab, ConditionType.Weaker);
            //약화 
            monsterStats.attackPower = (int)(monsterStats.attackPower * (1 - ability));
        }
    }
    public void DefDownForTurns(int turns, float ability)
    {
        //취약 : 몬스터의 방어력이 약해진다.
        defDownTurnsRemaining += turns;

        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.DefDown);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.defDownConditionPrefab, ConditionType.DefDown);
            defDownValue = ability;
        }
    }

    public void burnForTunrs(int turns)
    {
        //도트 딜
        burnTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Burn);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.burnConditionPrefab, ConditionType.Burn);
        }

    }
    public void PoisonForTunrs(int turns)
    {
        //도트 딜
        burnTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Poison);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.bleedingConditioinPrefab, ConditionType.Poison);
        }

    }
    public void BleedingForTunrs(int turns)
    {
        //도트 딜
        burnTurnsRemaining += turns;
        Condition existingFrozenCondition = conditionInstances.Find(condition => condition.conditionType == ConditionType.Bleeding);
        if (existingFrozenCondition != null)
        {
            existingFrozenCondition.IncrementStackCount(turns);
        }
        else
        {
            AddCondition(MonsterCondition, turns, GameManager.instance.poisonConditionPrefab, ConditionType.Bleeding);
        }

    }

    #endregion


    // 새로운 Condition 인스턴스를 생성하고 리스트에 추가한 후, 위치를 업데이트
    public void AddCondition(Transform parent, int initialStackCount, Condition conditionPrefab, ConditionType type)
    {
        if (conditionPrefab != null)
        {
            Condition newCondition = Instantiate(conditionPrefab, parent);
            conditionInstances.Add(newCondition);
            //UpdateConditionPositions();
            newCondition.Initialized(initialStackCount, conditionPos, type); // 위치 초기화 후에 스택 값 설정
        }
    }

    #region 안쓰는 것
    // 리스트에서 Condition 인스턴스를 제거하고 위치를 업데이트
    public void RemoveCondition(Condition condition)
    {
        //비사용 중
        if (conditionInstances.Contains(condition))
        {
            conditionInstances.Remove(condition);
            Destroy(condition.gameObject);
            //UpdateConditionPositions();
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

    public void UpdateConditions()
    {
        foreach (var condition in conditionInstances)
        {
            //Condition 업데이트 로직 구현
        }
    }
    #endregion
    public void DieAction()
    {
        if (IsDead())
        {
            monsterStats.attackPower = baseAttackPower;
            Die();
            DataManager.Instance.monstersKilledCount++; // DataManager에서 몬스터 카운트 증가

            // 몬스터가 가진 코인에 -2에서 2 사이의 랜덤 값을 추가
            int randomCoinAdjustment = UnityEngine.Random.Range(-2, 3); // -2에서 2까지의 값 (3은 포함되지 않음)
            int rewardCoin = monsterStats.Coin + randomCoinAdjustment;

            GameManager.instance.monsterTotalRewardCoin += rewardCoin; // 랜덤하게 수정된 보상 코인을 추가
        }
    }
}
