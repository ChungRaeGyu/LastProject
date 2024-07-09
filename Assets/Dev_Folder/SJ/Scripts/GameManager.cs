using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using static UnityEditor.PlayerSettings;

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
    public Transform playerSpawnPoints;
    public Transform[] monsterSpawnPoints;

    [Header("BUTTON")]
    public Button lobbyButton; // �κ�� ���� ��ư
    public Button turnEndButton; // �� ���� ��ư
    public Button openCardSelectionButton; // ī�� ���� â ���� ��ư
    [Header("Reward")]
    public Image fadeRewardPanel; // ���� �г� ���� �� ��ο�����
    public GameObject rewardPanel; // ���� �г�

    [Header("UI")]
    public Vector3 cardSpawnPosition = new Vector3(-7.8f, -4.1f, 0f); // ī�� ��ȯ ��ġ
    public Canvas healthBarCanvas; // ĵ���� ����
    public TMP_Text costText;
    public TMP_Text TurnText;

    [Header("???")]
    public Transform cardOptionsParent; // ī�� �ɼ��� ������ �θ� ��ü
    public GameObject cardOptionPrefab; // ī�� �ɼ� ������
    public Player player { get; private set; }
    public Monster[] monsters { get; private set; }
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
        UIClear(false, true, false, false, false);

        // HandManager �Ҵ�
        handManager = FindObjectOfType<HandManager>();
    }

    private void Start()
    {

        StartCoroutine(Battle());
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
            TurnText.text = PLAYER_TURN_TEXT; // �÷��̾� �� �ؽ�Ʈ ����

            player.InitializeCost();

            // ������ ī�� ��ο�
            yield return StartCoroutine(DrawInitialHand(5));

            yield return new WaitUntil(() => !playerTurn); // �÷��̾ ���� ��ĥ ������ ���



            Debug.Log("----- ���͵��� �� ���� -----");
            TurnText.text = ENEMY_TURN_TEXT; // �� �� �ؽ�Ʈ ����
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

    public IEnumerator DrawCardFromDeck()
    {
        // �ڵ� �߰�

        /*
        GameObject newCard = Instantiate(deckPrefab, cardSpawnPosition, Quaternion.identity);
        Debug.Log("�� ī�� �ν��Ͻ� ���� �Ϸ�.");

        // ī�� �����͸� �Ҵ�
        newCard.GetComponent<CardData>().CardObj = DataManager.Instance.deck.Count != 0 ? DataManager.Instance.deck.Pop() : DataManager.Instance.cardObjs[0];
        Debug.Log("ī�� ������ �Ҵ� �Ϸ�.");

        // ī�� �̹��� ����
        newCard.GetComponentInChildren<SpriteRenderer>().sprite = newCard.GetComponent<CardData>().CardObj.image;
        Debug.Log("ī�� �̹��� ���� �Ϸ�.");

        */
        // �� �� : �����͸Ŵ������� �쿡�� ī�带 �̾Ƽ� Instantiate�Ѵ�.
        CardBasic cardBasic = DataManager.Instance.PopCard();
        Debug.Log("cardBasic" + cardBasic);
        GameObject newCard = Instantiate(cardBasic.gameObject, cardSpawnPosition, Quaternion.identity);
        newCard.GetComponent<CardData>().CardObj = cardBasic;

        // HandManager�� ī�� �߰�
        handManager.AddCard(newCard.transform);
        Debug.Log("HandManager�� ī�� �߰� �Ϸ�.");
        yield return null;
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
    public void UIClear(bool lobbyBtn, bool turnEndBtn, bool setRewardPanel, bool setFadeRewardPanel, bool setOpenCardSelectionButton)
    {
        if (lobbyButton != null)
        {
            lobbyButton.gameObject.SetActive(lobbyBtn);
        }

        if (turnEndButton != null)
        {
            turnEndButton.gameObject.SetActive(turnEndBtn);
        }

        if (rewardPanel != null)
        {
            rewardPanel.gameObject.SetActive(setRewardPanel);
        }

        if (fadeRewardPanel != null)
        {
            fadeRewardPanel.gameObject.SetActive(setFadeRewardPanel);
        }

        if (openCardSelectionButton != null)
        {
            openCardSelectionButton.gameObject.SetActive(setOpenCardSelectionButton);

        }
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
