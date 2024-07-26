using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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

    // 몬스터에 대한 보상 코인 합산
    public int monsterTotalRewardCoin;

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

        // 플레이어 생성
        if (playerPrefab != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            player = playerObject.GetComponent<Player>();
            Debug.Log($"{player}");
        }

        // 몬스터 생성
        SpawnMonsters();
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

    public IEnumerator DrawInitialHand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            audioSource.PlayOneShot(CardDrawClip);

            yield return StartCoroutine(DrawCardFromDeck());
            yield return new WaitForSeconds(0.2f); // 드로우 간 딜레이
        }
    }

    private IEnumerator Battle()
    {
        int turnCount = 1;

        while (true)
        {
            Debug.Log("----- 플레이어 턴 시작 -----");
            playerTurn = true; // 플레이어 턴 시작
            UIManager.instance.UpdatePlayerTurnCount(turnCount);
            UIManager.instance.TurnText.text = PLAYER_TURN_TEXT; // 플레이어 턴 텍스트 설정

            player.InitializeCost();

            // 덱에서 카드 드로우
            yield return StartCoroutine(DrawInitialHand(5));

            yield return new WaitUntil(() => !playerTurn); // 플레이어가 턴을 마칠 때까지 대기

            Debug.Log("----- 몬스터들의 턴 시작 -----");
            UIManager.instance.UpdateMonsterTurnCount(turnCount);
            UIManager.instance.TurnText.text = ENEMY_TURN_TEXT; // 적 턴 텍스트 설정

            // 모든 몬스터의 턴 순차적으로 진행
            for (int i = 0; i < monsters.Count; i++)
            {
                MonsterCharacter monster = monsters[i];
                if (monster.currenthealth > 0)
                {
                    Debug.Log($"----- 몬스터의 턴 시작 -----");
                    yield return StartCoroutine(monster.MonsterTurn());
                    yield return new WaitUntil(() => playerTurn); // 플레이어 턴이 되기 전까지 대기
                }
            }
            Debug.Log("----- 몬스터들의 턴 종료 -----");

            turnCount++;
        }
    }

    public IEnumerator DrawCardFromDeck()
    {
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

        UIManager.instance.UIClear(true, false, true, true, true);
    }

    private bool AllMonstersDead()
    {
        if (monsters.Count == 0)
            return true;

        Debug.Log($"몬스터 수: {monsters.Count}");
        return false;
    }

    public void RemoveMonsterDead(MonsterCharacter monster)
    {
        monsters.Remove(monster);
        CheckAllMonstersDead();
    }

    public void OnLobbyButtonClick()
    {
        DataManager.Instance.stageClearCount++;

        //TODO:보스클리어 확인
        if (SaveManager.Instance.isBossStage)
        {
            DataManager.Instance.bossesDefeatedCount++;

            SaveManager.Instance.StopTrackingTime();
            DataManager.Instance.totalClearTime = (int)Math.Floor(SaveManager.Instance.stopwatch.Elapsed.TotalSeconds);

            // 클리어 패널을 띄워 줌
            UIManager.instance.victoryPanel.gameObject.SetActive(true);

            // 처치한 몬스터 수를 표시하는 부분
            if (UIManager.instance.victoryMonstersKilledText != null)
            {
                UIManager.instance.victoryMonstersKilledText.text = $"처치한 몬스터 ({DataManager.Instance.monstersKilledCount})";
            }
            // 던전 클리어 시간을 표시하는 부분
            if (UIManager.instance.victoryTotalClearTime != null)
            {
                UIManager.instance.victoryTotalClearTime.text = $"던전 클리어 시간 ({DataManager.Instance.totalClearTime})";
            }
            // 클리어한 스테이지 수를 표시하는 부분
            if (UIManager.instance.victoryStageClearCount != null)
            {
                UIManager.instance.victoryStageClearCount.text = $"클리어한 스테이지 ({SaveManager.Instance.FormatTime(SaveManager.Instance.stopwatch.Elapsed.TotalSeconds)})";
            }
            // 보스 처치를 표시하는 부분
            if (UIManager.instance.victoryBossesDefeatedCount != null)
            {
                UIManager.instance.victoryBossesDefeatedCount.text = $"보스 처치 ({DataManager.Instance.bossesDefeatedCount})";
            }            
            // 잔여 코인을 표시하는 부분
            if (UIManager.instance.victoryRemainingCoinCount != null)
            {
                UIManager.instance.victoryRemainingCoinCount.text = $"잔여 코인 ({DataManager.Instance.currentCoin})";
            }            
            // 획득한 크리스탈을 표시하는 부분
            if (UIManager.instance.victoryTotalCrystal != null)
            {
                DataManager.Instance.CalculateTotalCrystal();
                UIManager.instance.victoryTotalCrystal.text = $"{DataManager.Instance.totalCrystal}";
            }
        }
        else
        {
            SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);

            SceneManager.LoadScene(2);
        }
    }

    // 다음 스테이지 해금과 씬로드, 클리어 했을 때 하단의 진행 버튼
    public void ClearGoToLobbyScene()
    {
        DataManager.Instance.currentCrystal += DataManager.Instance.totalCrystal;
        StageCheck();
        SceneManager.LoadScene(1);
    }

    private void StageCheck()
    {
        SaveManager.Instance.isBossStage = false;
        SaveManager.Instance.accessDungeon = false;
        if (SaveManager.Instance.accessDungeonNum < SaveManager.Instance.accessibleDungeon.Length - 1)
        {
            SaveManager.Instance.accessibleDungeon[SaveManager.Instance.accessDungeonNum + 1] = true;
        }
    }
    public void EndMonsterTurn()
    {
        playerTurn = true;
    }

    public void EndPlayerTurn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);

        handManager.MoveUnusedCardsToUsed();
        playerTurn = false;
    }

    public void DeBuffAnim(GameObject deBuff)
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
}
