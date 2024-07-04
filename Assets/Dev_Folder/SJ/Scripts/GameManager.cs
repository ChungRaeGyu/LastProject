using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerPrefab;
    public GameObject monsterPrefab;
    public Transform playerSpawnPoints;
    public Transform[] monsterSpawnPoints;
    public bool playerTurn { get; private set; } = true;

    public GameObject deckPrefab;
    public Button lobbyButton; // �κ�� ���� ��ư
    public Button turnEndButton; // �� ���� ��ư
    public HandManager handManager; // �� �� �Ŵ���
    public Vector3 cardSpawnPosition = new Vector3(-7.8f, -4.1f, 0f); // ī�� ��ȯ ��ġ
    public Canvas healthBarCanvas; // ĵ���� ����
    public TMP_Text costText;

    public Player player { get; private set; }
    private Monster[] monsters;

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
            GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoints.position, Quaternion.identity);
            player = playerObject.GetComponent<Player>();
        }

        // ���� ����
        if (monsterPrefab != null && monsterSpawnPoints.Length > 0)
        {
            monsters = new Monster[monsterSpawnPoints.Length];
            for (int i = 0; i < monsterSpawnPoints.Length; i++)
            {
                GameObject monsterObject = Instantiate(monsterPrefab, monsterSpawnPoints[i].position, Quaternion.identity);
                monsters[i] = monsterObject.GetComponent<Monster>();
            }
        }

        // �κ� ��ư �ʱ� ��Ȱ��ȭ
        if (lobbyButton != null)
        {
            lobbyButton.gameObject.SetActive(false);
        }

        if (turnEndButton != null)
        {
            turnEndButton.gameObject.SetActive(true);
        }

        // HandManager �Ҵ�
        handManager = FindObjectOfType<HandManager>();
    }

    private void Start()
    {
        DrawInitialHand(4); // �ʱ� �ڵ� ��ο�

        StartCoroutine(Battle());
    }

    private void DrawInitialHand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DrawCardFromDeck();
        }
    }

    private IEnumerator Battle()
    {
        int turnCount = 0;

        while (true)
        {
            Debug.Log("----- �÷��̾� �� ���� -----");
            playerTurn = true; // �÷��̾� �� ����
            player.InitializeCost();

            // ������ ī�� ��ο�
            DrawCardFromDeck();

            yield return new WaitUntil(() => !playerTurn); // �÷��̾ ���� ��ĥ ������ ���

            Debug.Log("----- ���͵��� �� ���� -----");
            // ��� ������ �� ���������� ����
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i].currenthealth > 0)
                {
                    Debug.Log($"----- {i + 1}��° ������ �� ���� -----");
                    yield return StartCoroutine(monsters[i].MonsterTurn());
                    yield return new WaitUntil(() => playerTurn); // �÷��̾� ���� �Ǳ� ������ ���
                }
            }
            Debug.Log("----- ���͵��� �� ���� -----");

            turnCount++;
        }
    }

    private void DrawCardFromDeck()
    {
        // �ڵ� �߰�
        if (deckPrefab != null && DataManager.Instance.deck.Count > 0)
        {
            Debug.Log("������ ī�带 ��ο��մϴ�.");

            // ���ο� ī�� �ν��Ͻ� ����
            GameObject newCard = Instantiate(deckPrefab, cardSpawnPosition, Quaternion.identity);
            Debug.Log("�� ī�� �ν��Ͻ� ���� �Ϸ�.");

            // ī�� �����͸� �Ҵ�
            newCard.GetComponent<CardData>().cardSO = DataManager.Instance.deck.Count != 0 ? DataManager.Instance.deck.Pop() : DataManager.Instance.cardSOs[0];
            Debug.Log("ī�� ������ �Ҵ� �Ϸ�.");

            // ī�� �̹��� ����
            newCard.GetComponentInChildren<SpriteRenderer>().sprite = newCard.GetComponent<CardData>().cardSO.Image;
            Debug.Log("ī�� �̹��� ���� �Ϸ�.");

            // HandManager�� ī�� �߰�
            handManager.AddCard(newCard.transform);
            Debug.Log("HandManager�� ī�� �߰� �Ϸ�.");
        }
        else
        {
            Debug.Log("���� ī�尡 �����ϴ�.");

            // ex) ī�尡 �����Ƿ� �÷��̾ �������� �Դ� �� ȿ���� �ۼ�
        }
    }

    public Canvas GetHealthBarCanvas()
    {
        return healthBarCanvas;
    }

    public TMP_Text GetCostText()
    {
        return costText;
    }

    public bool AllMonstersDead()
    {
        foreach (Monster monster in monsters)
        {
            if (monster != null && monster.currenthealth > 0)
            {
                return false;
            }
        }
        return true;
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
        playerTurn = false;
    }
}
