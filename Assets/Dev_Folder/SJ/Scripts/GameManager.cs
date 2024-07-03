using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Monster[] monsters; // 몬스터 저장소
    public bool playerTurn { get; private set; } = true;

    public GameObject deckPrefab;
    public Button lobbyButton; // 로비로 가는 버튼
    public Button turnEndButton; // 턴 종료 버튼
    public HandManager handManager; // 손 패 매니저

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

        // 플레이어 할당
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // 모든 몬스터 찾아서 할당
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        monsters = new Monster[monsterObjects.Length];
        for (int i = 0; i < monsterObjects.Length; i++)
        {
            monsters[i] = monsterObjects[i].GetComponent<Monster>();
        }

        // 로비 버튼 초기 비활성화
        if (lobbyButton != null)
        {
            lobbyButton.gameObject.SetActive(false);
        }

        if (turnEndButton != null)
        {
            turnEndButton.gameObject.SetActive(true);
        }

        // HandManager 할당
        handManager = FindObjectOfType<HandManager>();
    }

    private void Start()
    {
        DrawInitialHand(4); // 초기 핸드 드로우

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
            Debug.Log("----- 플레이어 턴 시작 -----");
            playerTurn = true; // 플레이어 턴 시작
            player.InitializeCost();

            // 덱에서 카드 드로우
            DrawCardFromDeck();

            yield return new WaitUntil(() => !playerTurn); // 플레이어가 턴을 마칠 때까지 대기

            Debug.Log("----- 몬스터들의 턴 시작 -----");
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

    private void DrawCardFromDeck()
    {
        // 핸드 추가
        if (deckPrefab != null && DataManager.Instance.deck.Count > 0)
        {
            Debug.Log("덱에서 카드를 드로우합니다.");

            // 새로운 카드 인스턴스 생성
            GameObject newCard = Instantiate(deckPrefab, transform.position, Quaternion.identity);
            Debug.Log("새 카드 인스턴스 생성 완료.");

            // 카드 데이터를 할당
            newCard.GetComponent<CardData>().cardSO = DataManager.Instance.deck.Count != 0 ? DataManager.Instance.deck.Pop() : DataManager.Instance.cardSOs[0];
            Debug.Log("카드 데이터 할당 완료.");

            // 카드 이미지 설정
            newCard.GetComponentInChildren<SpriteRenderer>().sprite = newCard.GetComponent<CardData>().cardSO.Image;
            Debug.Log("카드 이미지 설정 완료.");

            // HandManager에 카드 추가
            handManager.AddCard(newCard.transform);
            Debug.Log("HandManager에 카드 추가 완료.");
        }
        else
        {
            Debug.Log("덱에 카드가 없습니다.");

            // 카드가 없으므로 플레이어가 데미지를 입는 등 효과를 작성
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
