using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const string PLAYER_TURN_TEXT = "턴 종료";
    private const string ENEMY_TURN_TEXT = "적 턴";

    public bool playerTurn { get; private set; } = true;

    [Header("CharacterPrefab")]
    public GameObject playerPrefab;
    public List<GameObject> monsterPrefab = new List<GameObject>();

    [Header("CharacterSpawnPoint")]
    public Transform playerSpawnPoint;
    public List<Transform> monsterSpawnPoint1;
    public List<Transform> monsterSpawnPoint2;
    public List<Transform> monsterSpawnPoint3;
    public List<Transform> monsterSpawnPoint4;

    public Transform cardSpawnPoint; // 카드 소환 위치

    public Player player { get; private set; }
    public List<MonsterCharacter> monsters = new List<MonsterCharacter>();

    [Header("Manager")]
    public HandManager handManager; // 손 패 매니저
    public EffectManager effectManager;

    [Header("MonsterInfoPrefabs")]
    public GameObject attackActionPrefab;
    public GameObject monsterNextActionListPrefab;
    public GameObject actionDescriptionPrefab;
    public GameObject monsterNamePrefab;

    [Header("Condition")]
    public GameObject conditionBoxPrefab;
    public Condition defenseconditionPrefab;
    public Condition frozenConditionPrefab;
    public Condition weakerConditionPrefab;
    public Condition defDownConditionPrefab;
    public Condition burnConditionPrefab; //화상 이런 류로 한다. 일단 화상을 입힌다.
    public Condition poisonConditionPrefab;
    public Condition bleedingConditioinPrefab;

    [Header("DamageText")]
    public GameObject damageTextPrefab;

    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip CardDrawClip;

    [Header("ScrollView")]
    [SerializeField] private GameObject unUsedScrollView;

    [Header("CardUtil")]
    public int volumeUp = 0;

    // 몬스터에 대한 보상 코인 합산
    public int monsterTotalRewardCoin;

    public CardListManager cardListManager;

    // 카드 드래그나 버튼작동을 멈추는 딜레이를 스킵하게 하는 값
    public bool skip;

    [Header("Effect")]
    public GameObject hitEffect;

    [Header("AudioClip")]
    public AudioClip turnClip;
    public AudioClip showRewardClip;
    public AudioClip rewardCardClip;
    public AudioClip BaseAttackClip;

    public CardBasic cardBasic;

    public Queue<CardBasic> cardQueue = new Queue<CardBasic>();
    public bool isPlayingCard = false;

    [Header("ShakeObject")]
    float ShakeAmount = 0.2f;
    float ShakeTime;
    List<Vector3> initialPosition = new List<Vector3>();
    public List<Transform> ShakeObject = new List<Transform>();

    [Header("Backgrounds")]
    [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();

    [Header("Dungeon Backgrounds")]
    [SerializeField] private SpriteRenderer battleBG;  // BattleBG에 해당하는 배경

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetBackgroundBasedOnDungeonNum();

        // 플레이어 생성
        if (playerPrefab != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            player = playerObject.GetComponent<Player>();
        }

        cardListManager = GetComponent<CardListManager>();

        handManager.dungeonDeckCardCountText.text = DataManager.Instance.deckList.Count.ToString();

        // 몬스터 생성
        SpawnMonsters();
    }

    private void SetBackgroundBasedOnDungeonNum()
    {
        int dungeonNum = DataManager.Instance.accessDungeonNum;

        // dungeonNum이 리스트 범위 내에 있는 경우 해당 배경 선택
        if (dungeonNum >= 0 && dungeonNum < backgrounds.Count)
        {
            battleBG.sprite = backgrounds[dungeonNum];
        }
    }

    private void Start()
    {
        StartCoroutine(Battle());
    }

    private void SpawnMonsters()
    {
        int i = 0;
        monsterPrefab = DataManager.Instance.Monsters;
        switch (monsterPrefab.Count)
        {
            case 1:
                foreach (Transform spawnPoint in monsterSpawnPoint1)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;
            case 2:
                foreach (Transform spawnPoint in monsterSpawnPoint2)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;
            case 3:
                foreach (Transform spawnPoint in monsterSpawnPoint3)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;
            case 4:
                foreach (Transform spawnPoint in monsterSpawnPoint4)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;

        }
    }

    public IEnumerator DrawInitialHand(int count, bool skip = true)
    {
        for (int i = 0; i < count; i++)
        {
            audioSource.PlayOneShot(CardDrawClip);

            yield return StartCoroutine(DrawCardFromDeck());
            yield return new WaitForSeconds(0.2f); // 드로우 간 딜레이
        }
    }

    private IEnumerator PlayCardCoroutine()
    {
        while (cardQueue.Count > 0)
        {
            CardBasic card = cardQueue.Dequeue();

            StartCoroutine(card.PlayCard());

            yield return new WaitUntil(() => card.playCardCompleted); // 카드 플레이 완료 대기
        }
    }

    private IEnumerator Battle()
    {
        int turnCount = 1;
        while (true)
        {
            //Debug.Log("----- 플레이어 턴 시작 -----");
            playerTurn = true; // 플레이어 턴 시작
            if (player.currentDefense > 0) player.currentDefense--;
            foreach (var condition in player.conditionInstances)
            {
                if (condition.conditionType == ConditionType.Defense)
                {
                    condition.DecrementStackCount(player);
                    break; // Defense Condition이 하나만 있어야 하기 때문에 루프를 종료
                }
            }
            StartCoroutine(player.Turn());
            UIManager.instance.UpdatePlayerTurnCount(turnCount);
            UIManager.instance.TurnText.text = PLAYER_TURN_TEXT; // 플레이어 턴 텍스트 설정

            player.InitializeCost();

            skip = false;
            if (volumeUp > 0) volumeUp = 0;
            if (DataManager.Instance.deck.Count + DataManager.Instance.usedCards.Count >= 5)
                // 덱에서 카드 드로우
                yield return StartCoroutine(DrawInitialHand(5));
            else
                yield return StartCoroutine(DrawInitialHand(DataManager.Instance.deck.Count + DataManager.Instance.usedCards.Count));
            skip = true;

            foreach (MonsterCharacter monster in monsters)
            {
                if (monster.monsterNextAction != null)
                {

                    if (monster.frozenTurnsRemaining < 1&&monster.currenthealth>0)
                        monster.monsterNextAction.gameObject.SetActive(true); // 모든 몬스터의 다음 액션 오브젝트 활성화
                }
            }


            while (playerTurn)
            {
                if (!isPlayingCard)
                {
                    isPlayingCard = true;

                    StartCoroutine(PlayCardCoroutine());

                    isPlayingCard = false;
                }
                yield return null; // 매 프레임 대기
            }

            // Debug.Log("----- 몬스터들의 턴 시작 -----");
            UIManager.instance.UpdateMonsterTurnCount(turnCount);
            UIManager.instance.TurnText.text = ENEMY_TURN_TEXT; // 적 턴 텍스트 설정

            // 모든 몬스터의 턴 순차적으로 진행
            //이게 지금 몬스터
            for (int i = 0; i < monsters.Count; i++)
            {
                MonsterCharacter monster = monsters[i];
                if (monster.currenthealth > 0)
                {
                    // Debug.Log($"----- 몬스터의 턴 시작 -----");
                    yield return StartCoroutine(monster.Turn());
                }

            }

            turnCount++;
        }
    }

    public IEnumerator DrawCardFromDeck()
    {
        if (DataManager.Instance.deck.Count + DataManager.Instance.usedCards.Count == 0) yield break;

        CardBasic cardBasic = DataManager.Instance.PopCard();
        AddCard(cardBasic);
        yield return null;
    }

    public void AddCard(CardBasic cardBasic)
    {
        GameObject newCard = Instantiate(cardBasic.gameObject, cardSpawnPoint.position, Quaternion.identity); // 카드 소환 위치 사용
        newCard.GetComponent<CardBasic>().cardBasic = cardBasic;
        handManager.AddCard(newCard.transform);
    }

    public void CheckAllMonstersDead()
    {
        if (AllMonstersDead())
        {
            StartCoroutine(WaitAndClearUI());

        }
    }

    private IEnumerator WaitAndClearUI()
    {
        UIManager.instance.ResetUIPositions();
        handManager.HideAllCards();
        DataManager.Instance.currenthealth = player.currenthealth;
        // 잠깐 기다림
        yield return new WaitForSeconds(1.0f);

        // 보상 패널 켜질때
        SettingManager.Instance.PlaySound(showRewardClip);
        UIManager.instance.UIClear(true, false, true, true, true);
    }

    private bool AllMonstersDead()
    {
        int count = monsters.Count;
        foreach(MonsterCharacter monster in monsters)
        {
            if (!monster.gameObject.activeInHierarchy) count--;
            else
            {
                Debug.Log("Monster이름 : " + monster.name);
            }
        }
        if (count == 0)
        {
            monsters.Clear();
            return true;
        }

        return false;
    }

    public void OnLobbyButtonClick()
    {
        DataManager.Instance.ClearStageClearCount++;
        DataManager.Instance.DefeatStageClearCount++;

        // 보스 클리어 확인
        if (SaveManager.Instance.isBossStage)
        {
            SaveManager.Instance.isBossStage = false;
            SaveManager.Instance.isEliteStage = false;
            DataManager.Instance.ClearBossesDefeatedCount++;
            SaveManager.Instance.StopTrackingTime();
            DataManager.Instance.ClearTotalClearTime = (int)Math.Floor(SaveManager.Instance.stopwatch.Elapsed.TotalSeconds);

            // 클리어 패널을 띄워 줌
            UIManager.instance.victoryPanel.gameObject.SetActive(true);
            UIManager.instance.ClearPanelFade.SetActive(true);

            // 텍스트 업데이트
            UpdateVictoryTexts();

            // 획득한 크리스탈 계산 및 표시
            DataManager.Instance.ClearCalculateTotalCrystal();
            if (UIManager.instance.victoryTotalCrystal != null)
            {
                UIManager.instance.victoryTotalCrystal.text = $"{DataManager.Instance.ClearTotalCrystal}";
            }
        }
        else
        {
            SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);
            SaveManager.Instance.isBossStage = false;
            SaveManager.Instance.isEliteStage = false;
            SceneFader.instance.LoadSceneWithFade(2);
        }
    }

    private void UpdateVictoryTexts()
    {
        DataManager.Instance.adjustedCurrentCoin = Mathf.FloorToInt(DataManager.Instance.currentCoin / 100f);
        DataManager.Instance.adjustedClearTime = Mathf.Max(300 - DataManager.Instance.ClearTotalClearTime, 0);

        SetText(UIManager.instance.victoryMonstersKilledText, $"처치한 몬스터 ({DataManager.Instance.ClearMonstersKilledCount})");
        SetText(UIManager.instance.victoryStageClearCountText, $"클리어한 스테이지 ({DataManager.Instance.ClearStageClearCount})");
        SetText(UIManager.instance.victoryTotalClearTimeText, $"던전 클리어 시간 ({SaveManager.Instance.FormatTime(SaveManager.Instance.stopwatch.Elapsed.TotalSeconds)})");
        SetText(UIManager.instance.victoryBossesDefeatedCountText, $"보스 처치 ({DataManager.Instance.ClearBossesDefeatedCount})");
        SetText(UIManager.instance.victoryRemainingCoinCountText, $"잔여 코인 ({DataManager.Instance.currentCoin})");

        SetText(UIManager.instance.victoryMonstersKilledPointText, $"{DataManager.Instance.ClearMonstersKilledCount}");
        SetText(UIManager.instance.victoryStageClearCountPointText, $"{DataManager.Instance.ClearStageClearCount}");
        SetText(UIManager.instance.victoryTotalClearTimePointText, $"{DataManager.Instance.adjustedClearTime}");
        SetText(UIManager.instance.victoryBossesDefeatedCountPointText, $"{DataManager.Instance.adjustedBossesDefeatedCount}");
        SetText(UIManager.instance.victoryRemainingCoinCountPointText, $"{DataManager.Instance.adjustedCurrentCoin}");
    }

    private void SetText(TMP_Text textComponent, string text)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
        }
    }

    // 다음 스테이지 해금과 씬로드, 클리어 했을 때 하단의 진행 버튼
    public void ClearGoToLobbyScene()
    {
        DataManager.Instance.currentCrystal += DataManager.Instance.ClearTotalCrystal;
        StageCheck();
        SceneFader.instance.LoadSceneWithFade(1);
    }

    private void StageCheck()
    {
        SaveManager.Instance.isBossStage = false;
        SaveManager.Instance.isEliteStage = false;
        SaveManager.Instance.accessDungeon = false;
        DataManager.Instance.openDungeonNum++;
        if (DataManager.Instance.openDungeonNum < DataManager.Instance.accessibleDungeon.Length - 1)
        {
            if (DataManager.Instance.openDungeonNum >= 4) return;

        }
    }

    public void EndPlayerTurn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);

        if (!playerTurn) return;
        if (!handManager.setCardEnd) return;

        handManager.MoveUnusedCardsToUsed();
        playerTurn = false;
    }

    public void DestroyDeBuffAnim(GameObject deBuff)
    {
        StartCoroutine(DelayDestroy(deBuff));
    }
    IEnumerator DelayDestroy(GameObject deBuff)
    {
        if (deBuff != null)
        {
            deBuff.GetComponentInChildren<Animator>().SetTrigger("IceOff");
            yield return new WaitForSecondsRealtime(1f);
            Destroy(deBuff);
        }
    }

    // ScrollView의 활성화/비활성화 공통 메서드
    private void ToggleScrollView(GameObject scrollView, Action showCardsAction, Action hideCardsAction, Action resetUIAction, bool fadeRewardPanelActive, Action updateList)
    {
        if (scrollView != null)
        {
            if (scrollView.activeSelf)
            {
                // 비활성화
                SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);
                UIManager.instance.MoveUIElementsToStartPositions();
                UIManager.instance.fadeRewardPanel.gameObject.SetActive(false);
                showCardsAction?.Invoke();
            }
            else
            {
                // 활성화
                updateList?.Invoke();
                SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardPassClip);
                resetUIAction?.Invoke();
                UIManager.instance.fadeRewardPanel.gameObject.SetActive(fadeRewardPanelActive);
                hideCardsAction?.Invoke();
            }

            scrollView.SetActive(!scrollView.activeSelf);
        }
    }

    // unUsedScrollView 활성화/비활성화 메서드
    public void ToggleUnUsedScrollView()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);
        ToggleScrollView(
            unUsedScrollView,
            handManager.ShowAllCardsActive,  // 카드 표시
            handManager.HideAllCardsActive,  // 카드 숨기기
            UIManager.instance.UnUsedCardsResetUIPositions, // UI 위치 재설정
            true, // fadeRewardPanel 활성화
            cardListManager.UpdateDeckList
        );
    }

    // usedScrollView 활성화/비활성화 메서드
    public void ToggleUsedScrollView()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);
        ToggleScrollView(
            unUsedScrollView,
            handManager.ShowAllCardsActive,  // 카드 표시
            handManager.HideAllCardsActive,  // 카드 숨기기
            UIManager.instance.UsedCardsResetUIPositions, // UI 위치 재설정
            true, // fadeRewardPanel 활성화
            cardListManager.UpdateUsedCardsList
        );
    }

    // 덱 스크롤 뷰 활성화/비활성화 메서드
    public void ToggleDungeonDeckScrollView()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);
        ToggleScrollView(
            unUsedScrollView,
            handManager.ShowAllCardsActive,  // 카드 표시
            handManager.HideAllCardsActive,  // 카드 숨기기
            UIManager.instance.dungeonDeckCardsResetUIPositions, // UI 위치 재설정
            true, // fadeRewardPanel 활성화
            cardListManager.UpdateDungeonDeckList
        );
    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraCoroutine());
    }
    IEnumerator ShakeCameraCoroutine()
    {
        float shakeValue = 0.2f;
        float shaketime = 0.2f;
        foreach (Transform pos in ShakeObject)
        {
            initialPosition.Add(pos.position);
        }
        ShakeAmount = shakeValue;
        ShakeTime = shaketime;
        while (true)
        {
            if (ShakeTime > 0)
            {
                for(int i=0; i < ShakeObject.Count; i++)
                {
                    ShakeObject[i].position = Random.insideUnitSphere * ShakeAmount + initialPosition[i];

                }
                ShakeTime -= Time.deltaTime;
                yield return null;
            }
            else
            {
                for (int i = 0; i < ShakeObject.Count; i++)
                {
                    ShakeObject[i].position = initialPosition[i];
                }

                break;
            }
        }
        initialPosition.Clear();
    }
}
