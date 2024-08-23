using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MonsterCharacter : Character
{
    public MonsterStats monsterStats;
    public int baseAttackPower;
    public int currenthealth;

    private static readonly int takeDamage = Animator.StringToHash("TakeDamage");
    public static readonly int Attack = Animator.StringToHash("Attack");

    public Transform conditionPos;
    public Transform hpBarPos; // HP 바 위치
    public Transform monsterNextActionPos; // 다음 행동을 나타낼 위치
    public Transform monsterNamePos; // 이름 위치

    private Transform MonsterCondition; //이건 또 뭘까
    public Transform monsterNextAction { get; set; }
    private Transform monsterName;
    private Transform monsterNextActionList;

    private AdjustHeightBasedOnText attackDescriptionAdjustHeight;
    protected TMP_Text attackDescriptionText;

    private AdjustHeightBasedOnText util1DescriptionAdjustHeight;
    protected TMP_Text util1DescriptionText;

    private AdjustHeightBasedOnText util2DescriptionAdjustHeight;
    protected TMP_Text util2DescriptionText;
    private float defDownValue;

    public float monsterTurnDelay = 0.5f;

    public bool boss;

    public System.Random random = new System.Random();

    public Action deBuffAnim;

    [Header("Description_NotCashing")]
    public Transform descriptionTransform;
    public GameObject attackDescriptionObject;
    public GameObject defenseDescriptionObject;
    public GameObject healDescriptionObject;
    public bool isDead;
    public bool showActionFrozenAction;

    public void Start()
    {
        currenthealth = monsterStats.maxhealth;
        // 몬스터에 랜덤한
        if (!boss)
        {
            int hpUp = random.Next(-3, 3);
            currenthealth += hpUp;
        }
        baseAttackPower = monsterStats.attackPower;
        // ConditionBox 프리팹을 conditionCanvas의 자식으로 생성하고 playerCondition에 할당
        MonsterCondition = Instantiate(GameManager.instance.conditionBoxPrefab, UIManager.instance.conditionCanvas.transform).transform;

        AddCondition(MonsterCondition, monsterStats.defense, GameManager.instance.defenseconditionPrefab);

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
        monsterNextAction.position = monsterNextActionPos.position;
        monsterName.position = monsterNamePos.position;
        monsterNextActionList.position = transform.position;
        MonsterCondition.position = conditionPos.position;

        //// 얼면 아무것도 띄우지 않는다.
        //if (isFrozen && showActionFrozenAction)
        //{
        //    showActionFrozenAction = false;
        //    attackDescriptionText.text = "";
        //}
    }

    public virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - monsterStats.defense, 0);
        actualDamage = (int)(defDownValue > 0 ? actualDamage * (1 + defDownValue) : actualDamage);
        currenthealth -= actualDamage;
        if (animator != null)
        {
            animator.SetTrigger(takeDamage);
        }

        SpawnDamageText(actualDamage, transform.position);

        DieAction();
    }


    public void DieAction()
    {
        if (IsDead())
        {
            monsterStats.attackPower = baseAttackPower;
            Die();
            DataManager.Instance.ClearMonstersKilledCount++; // DataManager에서 몬스터 카운트 증가
            DataManager.Instance.DefeatMonstersKilledCount++; // DataManager에서 몬스터 카운트 증가

            // 몬스터가 가진 코인에 -2에서 2 사이의 랜덤 값을 추가
            int randomCoinAdjustment = UnityEngine.Random.Range(-2, 3); // -2에서 2까지의 값 (3은 포함되지 않음)
            int rewardCoin = monsterStats.Coin + randomCoinAdjustment;

            GameManager.instance.monsterTotalRewardCoin += rewardCoin; // 랜덤하게 수정된 보상 코인을 추가
        }
    }
    public bool IsDead()
    {
        return currenthealth <= 0;
    }

    protected virtual void Die()
    {
        if (isFrozen)
            GameManager.instance.DestroyDeBuffAnim(deBuff);

        //이름, 다음행동 액션 사라짐
        if (monsterNextAction != null)
        {
            monsterNextAction.gameObject.SetActive(false);
        }

        if (monsterName != null)
        {
            monsterName.gameObject.SetActive(false);
        }

        if (monsterNextActionList != null)
        {
            monsterNextActionList.gameObject.SetActive(false);
        }

        isDead = true;
        gameObject.SetActive(false);
        GameManager.instance.CheckAllMonstersDead();
    }
    public override void TakedamageCharacter(int damage)
    {
        TakeDamage(damage);
    }

    public override void BaseWeakerMethod()
    {
        monsterStats.attackPower = baseAttackPower;
    }
    public override void WeakingMethod(float ability)
    {
        Debug.Log("약화");
        monsterStats.attackPower = (int)(monsterStats.attackPower * (1 - ability));
    }
    public override void BasedefMethod()
    {
        defDownValue = 0;
    }
    public override void DefDownValue(float ability)
    {
        Debug.Log("방깍");
        defDownValue = ability;
    }


    protected IEnumerator PerformAttack(int damage, float attackDelay = 1.2f)
    {
        // 애니메이션 트리거
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(0.3f);

        // 플레이어 맞는 소리
        SettingManager.Instance.PlaySound(GameManager.instance.BaseAttackClip);
        Instantiate(GameManager.instance.hitEffect, GameManager.instance.player.transform.position, Quaternion.identity);

        GameManager.instance.player.TakeDamage(damage);

        //대기 시간
        yield return new WaitForSeconds(attackDelay);
    }
    protected override Transform GetConditionPos()
    {
        return MonsterCondition;
    }
    protected override Transform GetConditionTransfrom()
    {
        return conditionPos;
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
}
