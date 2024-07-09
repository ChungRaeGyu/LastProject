using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using static UnityEditor.PlayerSettings;

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
    public Transform playerSpawnPoints;
    public Transform[] monsterSpawnPoints;

    [Header("BUTTON")]
    public Button lobbyButton; // 로비로 가는 버튼
    public Button turnEndButton; // 턴 종료 버튼
    public Button openCardSelectionButton; // 카드 선택 창 열기 버튼
    [Header("Reward")]
    public Image fadeRewardPanel; // 보상 패널 열릴 때 어두워지게
    public GameObject rewardPanel; // 보상 패널

    [Header("UI")]
    public Vector3 cardSpawnPosition = new Vector3(-7.8f, -4.1f, 0f); // 카드 소환 위치
    public Canvas healthBarCanvas; // 캔버스 참조
    public TMP_Text costText;
    public TMP_Text TurnText;

    [Header("???")]
    public Transform cardOptionsParent; // 카드 옵션을 보여줄 부모 객체
    public GameObject cardOptionPrefab; // 카드 옵션 프리팹
    public Player player { get; private set; }
    public Monster[] monsters { get; private set; }
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

            GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoints.position, Quaternion.identity);
            player = playerObject.GetComponent<Player>();
        }

        // 몬스터 생성
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

        // HandManager 할당
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
            TurnText.text = PLAYER_TURN_TEXT; // 플레이어 턴 텍스트 설정

            player.InitializeCost();

            // 덱에서 카드 드로우
            yield return StartCoroutine(DrawInitialHand(5));

            yield return new WaitUntil(() => !playerTurn); // 플레이어가 턴을 마칠 때까지 대기



            Debug.Log("----- 몬스터들의 턴 시작 -----");
            TurnText.text = ENEMY_TURN_TEXT; // 적 턴 텍스트 설정
            // 모든 몬스터의 턴 순차적으로 진행
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i].currenthealth > 0)
                {
                    Debug.Log($"----- {i + 1}번째 몬스터의 턴 시작 -----");
                    yield return StartCoroutine(monsters[i].MonsterTurn());
                    yield return new WaitUntil(() => playerTurn); // 플레이어 턴이 되기 전까지 대기
                }
            }
            Debug.Log("----- 몬스터들의 턴 종료 -----");

            turnCount++;
        }
    }

    public IEnumerator DrawCardFromDeck()
    {
        // 핸드 추가

        /*
        GameObject newCard = Instantiate(deckPrefab, cardSpawnPosition, Quaternion.identity);
        Debug.Log("새 카드 인스턴스 생성 완료.");

        // 카드 데이터를 할당
        newCard.GetComponent<CardData>().CardObj = DataManager.Instance.deck.Count != 0 ? DataManager.Instance.deck.Pop() : DataManager.Instance.cardObjs[0];
        Debug.Log("카드 데이터 할당 완료.");

        // 카드 이미지 설정
        newCard.GetComponentInChildren<SpriteRenderer>().sprite = newCard.GetComponent<CardData>().CardObj.image;
        Debug.Log("카드 이미지 설정 완료.");

        */
        // 할 일 : 데이터매니저에서 댁에서 카드를 뽑아서 Instantiate한다.
        CardBasic cardBasic = DataManager.Instance.PopCard();
        Debug.Log("cardBasic" + cardBasic);
        GameObject newCard = Instantiate(cardBasic.gameObject, cardSpawnPosition, Quaternion.identity);
        newCard.GetComponent<CardData>().CardObj = cardBasic;

        // HandManager에 카드 추가
        handManager.AddCard(newCard.transform);
        Debug.Log("HandManager에 카드 추가 완료.");
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
