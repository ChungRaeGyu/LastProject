using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Monster[] monsters; // ���� �����
    public bool playerTurn { get; private set; } = true;

    public GameObject deckPrefab;
    public Button lobbyButton; // �κ�� ���� ��ư
    public Button TurnEndButton; // �κ�� ���� ��ư

    private void Awake()
    {
        //���ڵ� �׽�Ʈ
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

        if (TurnEndButton != null)
        {
            TurnEndButton.gameObject.SetActive(true);
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

            // ��� ������ ���� ������ ���� ���� �غ�
            playerTurn = true; // �÷��̾� �� ����
            player.InitializeCost();

            // �ڵ� �߰�
            if (deckPrefab != null)
            {
                if (DataManager.Instance.deck.Count != 0)
                {

                }
                GameObject temp = deckPrefab;
                temp.GetComponent<CardData>().cardSO = DataManager.Instance.deck.Count != 0 ? DataManager.Instance.deck.Pop() : DataManager.Instance.cardSOs[0];
                temp.GetComponentInChildren<SpriteRenderer>().sprite = temp.GetComponent<CardData>().cardSO.Image;
                Instantiate(deckPrefab, transform.position, Quaternion.identity);
            }

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
