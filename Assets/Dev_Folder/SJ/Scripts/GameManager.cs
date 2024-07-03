using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Monster[] monsters; // 몬스터 저장소
    public bool playerTurn { get; private set; } = true;

    public GameObject deckPrefab;
    public Button lobbyButton; // 로비로 가는 버튼

    private void Awake()
    {
        //인코딩 테스트
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
            Debug.Log("----- 플레이어 턴 시작 -----");
            // 플레이어 턴 시작
            playerTurn = true;
            yield return new WaitUntil(() => !playerTurn); // 플레이어가 턴을 마칠 때까지 대기

            Debug.Log("----- 몬스터들의 턴 시작 -----");
            // 모든 몬스터의 턴 순차적으로 진행
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i].Currenthealth > 0)
                {
                    Debug.Log($"----- {i + 1}번째 몬스터의 턴 시작 -----");
                    yield return StartCoroutine(monsters[i].MonsterTurn());
                    yield return new WaitUntil(() => playerTurn); // 플레이어 턴이 되기 전까지 대기
                }
            }

            Debug.Log("----- 몬스터들의 턴 종료 -----");
            // 모든 몬스터의 턴이 끝나면 다음 라운드 준비
            playerTurn = true; // 플레이어 턴 시작
            player.InitializeCost();

            // 핸드 추가
            if (deckPrefab != null)
            {
                if(DataManager.Instance.deck.Count != 0){
                    
                }
                GameObject temp = deckPrefab;
                temp.GetComponent<CardData>().cardSO = DataManager.Instance.deck.Count != 0?DataManager.Instance.deck.Pop():DataManager.Instance.cardSOs[0];
                temp.GetComponentInChildren<SpriteRenderer>().sprite = temp.GetComponent<CardData>().cardSO.Image;
                Instantiate(deckPrefab, transform.position, Quaternion.identity);
            }

            // 모든 몬스터가 죽었을 때 로비 버튼 활성화
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
