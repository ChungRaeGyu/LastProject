using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const string PLAYER_TURN_TEXT = "턴 종료";
    private const string ENEMY_TURN_TEXT = "적 턴";

    public bool playerTurn { get; private set; } = true;

    [Header("CharacterPrefab")]
    public GameObject playerPrefab;
    public GameObject monsterPrefab;

    [Header("CharacterSpawnPoint")]
    public Transform playerSpawnPoint;
    public List<Transform> monsterSpawnPoints;

    public Vector3 cardSpawnPosition = new Vector3(-7.8f, -4.1f, 0f); // 카드 소환 위치

    public Player player { get; private set; }
    public List<Monster> monsters = new List<Monster>();

    [Header("Manager")]
    public HandManager handManager; // 손 패 매니저
    public EffectManager effectManager;

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

        // HandManager 할당
        handManager = FindObjectOfType<HandManager>();
    }

    private void Start()
    {
        StartCoroutine(Battle());
    }

    private void SpawnMonsters()
    {
        if (monsterPrefab != null && monsterSpawnPoints.Count > 0)
        {
            foreach (Transform spawnPoint in monsterSpawnPoints)
            {
                GameObject monsterObject = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
                Monster monster = monsterObject.GetComponent<Monster>();
                monsters.Add(monster);
            }
        }
    }

    private IEnumerator DrawInitialHand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return StartCoroutine(DrawCardFromDeck());
            yield return new WaitForSeconds(0.2f); // 드로우 간 딜레이
        }
    }

    private IEnumerator Battle()
    {
        int turnCount = 0;

        while (true)
        {
            Debug.Log("----- 플레이어 턴 시작 -----");
            playerTurn = true; // 플레이어 턴 시작
            UIManager.Instance.TurnText.text = PLAYER_TURN_TEXT; // 플레이어 턴 텍스트 설정

            player.InitializeCost();

            // 덱에서 카드 드로우
            yield return StartCoroutine(DrawInitialHand(5));

            yield return new WaitUntil(() => !playerTurn); // 플레이어가 턴을 마칠 때까지 대기

            Debug.Log("----- 몬스터들의 턴 시작 -----");
            UIManager.Instance.TurnText.text = ENEMY_TURN_TEXT; // 적 턴 텍스트 설정

            // 모든 몬스터의 턴 순차적으로 진행
            foreach (Monster monster in monsters)
            {
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
        Debug.Log("cardBasic" + cardBasic);
        GameObject newCard = Instantiate(cardBasic.gameObject, cardSpawnPosition, Quaternion.identity);
        newCard.GetComponent<CardBasic>().CardObj = cardBasic;

        // HandManager에 카드 추가
        handManager.AddCard(newCard.transform);
        Debug.Log("HandManager에 카드 추가 완료.");
        yield return null;
    }

    public void CheckAllMonstersDead()
    {
        if (AllMonstersDead())
        {
            UIManager.Instance.UIClear(true, false, true, true, true);
        }
    }

    private bool AllMonstersDead()
    {
        if (monsters.Count == 0)
           return true;

        Debug.Log($"몬스터 수: {monsters.Count}");
        return false;
    }

    public void RemoveMonsterDead(Monster monster)
    {
        monsters.Remove(monster);

        CheckAllMonstersDead();
    }

    public void OnLobbyButtonClick()
    {
        GameManager_chan.Instance.stageLevel += 1;
        SceneManager.LoadScene(2);
    }

    public void EndMonsterTurn()
    {
        playerTurn = true;
    }

    public void EndPlayerTurn()
    {
        handManager.MoveUnusedCardsToUsed();
        playerTurn = false;
    }
}
