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
    }

    private void Start()
    {
        StartCoroutine(Battle());
    }

    private IEnumerator Battle()
    {
        int turnCount = 0;

        while (true)
        {
            Debug.Log("----- �÷��̾� �� ���� -----");
            // �÷��̾� �� ����
            playerTurn = true;
            yield return new WaitUntil(() => !playerTurn); // �÷��̾ ���� ��ĥ ������ ���

            Debug.Log("----- ���͵��� �� ���� -----");
            // ��� ������ �� ���������� ����
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i].Currenthealth > 0)
                {
                    Debug.Log($"----- {i + 1}��° ������ �� ���� -----");
                    yield return StartCoroutine(monsters[i].MonsterTurn());
                    yield return new WaitUntil(() => playerTurn); // �÷��̾� ���� �Ǳ� ������ ���
                }
            }

            Debug.Log("----- ���͵��� �� ���� -----");
            // ��� ������ ���� ������ ���� ���� �غ�
            playerTurn = true; // �÷��̾� �� ����
            player.InitializeCost();

            // �� ���� �ڵ� �߰�
            if (deckPrefab != null)
            {
                Instantiate(deckPrefab, transform.position, Quaternion.identity);
            }

            // ��� ���Ͱ� �׾��� �� �κ� ��ư Ȱ��ȭ
            if (AllMonstersDead() && lobbyButton != null)
            {
                lobbyButton.gameObject.SetActive(true);
            }

            turnCount++;
        }
    }

    private bool AllMonstersDead()
    {
        foreach (Monster monster in monsters)
        {
            if (monster.Currenthealth > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void OnLobbyButtonClick()
    {
        SceneManager.LoadScene(0);
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
