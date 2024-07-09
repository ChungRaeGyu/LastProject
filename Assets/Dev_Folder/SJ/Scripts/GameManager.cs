using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const string PLAYER_TURN_TEXT = "�� ����";
    private const string ENEMY_TURN_TEXT = "�� ��";

    public bool playerTurn { get; private set; } = true;

    [Header("CharacterPrefab")]
    public GameObject playerPrefab;
    public GameObject monsterPrefab;

    [Header("CharacterSpawnPoint")]
    public Transform playerSpawnPoint;
    public List<Transform> monsterSpawnPoints;

    public Vector3 cardSpawnPosition = new Vector3(-7.8f, -4.1f, 0f); // ī�� ��ȯ ��ġ

    public Player player { get; private set; }
    public List<Monster> monsters = new List<Monster>();

    [Header("Manager")]
    public HandManager handManager; // �� �� �Ŵ���
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

        // �÷��̾� ����
        if (playerPrefab != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            player = playerObject.GetComponent<Player>();
            Debug.Log($"{player}");
        }

        // ���� ����
        SpawnMonsters();

        // HandManager �Ҵ�
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
            yield return new WaitForSeconds(0.2f); // ��ο� �� ������
        }
    }

    private IEnumerator Battle()
    {
        int turnCount = 0;

        while (true)
        {
            Debug.Log("----- �÷��̾� �� ���� -----");
            playerTurn = true; // �÷��̾� �� ����
            UIManager.Instance.TurnText.text = PLAYER_TURN_TEXT; // �÷��̾� �� �ؽ�Ʈ ����

            player.InitializeCost();

            // ������ ī�� ��ο�
            yield return StartCoroutine(DrawInitialHand(5));

            yield return new WaitUntil(() => !playerTurn); // �÷��̾ ���� ��ĥ ������ ���

            Debug.Log("----- ���͵��� �� ���� -----");
            UIManager.Instance.TurnText.text = ENEMY_TURN_TEXT; // �� �� �ؽ�Ʈ ����

            // ��� ������ �� ���������� ����
            foreach (Monster monster in monsters)
            {
                if (monster.currenthealth > 0)
                {
                    Debug.Log($"----- ������ �� ���� -----");
                    yield return StartCoroutine(monster.MonsterTurn());
                    yield return new WaitUntil(() => playerTurn); // �÷��̾� ���� �Ǳ� ������ ���
                }
            }
            Debug.Log("----- ���͵��� �� ���� -----");

            turnCount++;
        }
    }

    public IEnumerator DrawCardFromDeck()
    {
        CardBasic cardBasic = DataManager.Instance.PopCard();
        Debug.Log("cardBasic" + cardBasic);
        GameObject newCard = Instantiate(cardBasic.gameObject, cardSpawnPosition, Quaternion.identity);
        newCard.GetComponent<CardBasic>().CardObj = cardBasic;

        // HandManager�� ī�� �߰�
        handManager.AddCard(newCard.transform);
        Debug.Log("HandManager�� ī�� �߰� �Ϸ�.");
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

        Debug.Log($"���� ��: {monsters.Count}");
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
