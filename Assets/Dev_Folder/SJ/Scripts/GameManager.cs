using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Monster[] monsters; // ���� �����
    public bool playerTurn { get; private set; } = true;

    public GameObject deckPrefab;
    public Button lobbyButton; // �κ�� ���� ��ư
    public Button turnEndButton; // �� ���� ��ư
    public HandManager handManager; // �� �� �Ŵ���

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

        // �÷��̾� �Ҵ�
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // ��� ���� ã�Ƽ� �Ҵ�
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        monsters = new Monster[monsterObjects.Length];
        for (int i = 0; i < monsterObjects.Length; i++)
        {
            monsters[i] = monsterObjects[i].GetComponent<Monster>();
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
            GameObject newCard = Instantiate(deckPrefab, transform.position, Quaternion.identity);
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

            // ī�尡 �����Ƿ� �÷��̾ �������� �Դ� �� ȿ���� �ۼ�
        }
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
