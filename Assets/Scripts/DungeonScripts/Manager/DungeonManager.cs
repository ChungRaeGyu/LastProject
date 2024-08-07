using TMPro;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //인스턴스
    public static DungeonManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    [Header("DungeonScene")]
    public GameObject dungeonBoard;
    public GameObject dungeon;

    [Header("DungeonBoard")]
    public GameObject[] dungeonEntrance = new GameObject[5];

    [Header("Dungeon")]
    public GameObject[] dungeonNum = new GameObject[5];

    [Header("Player")]
    public GameObject player;

    [Header("TextUI")]
    public TMP_Text currentCoinText;
    public TMP_Text currentHpText;

    [Header("Info")]
    public GameObject DungeonCoin;
    public GameObject DungeonHp;

    [Header("Player")]
    public Player Player;
    public Transform startPosition;

    [Header("Manager")]
    public EventManager eventManager;


    void Start()
    {
        //던전에 입장했을 때
        if (SaveManager.Instance.accessDungeon == true)
        {
            dungeonBoard.SetActive(false); //던전 보드 비활성화
            dungeon.SetActive(true); //던전 활성화

            int num =   DataManager.Instance.accessDungeonNum;
            dungeonNum[num].SetActive(true);
            DungeonCoin.SetActive(true);
            DungeonHp.SetActive(true);
        }
        //던전에 입장하지 않았을때
        else
        {
            dungeonBoard.SetActive(true); //던전 보드 활성화
            dungeon.SetActive(false); //던전 보드 비활성화

            DungeonCoin.SetActive(false);
            DungeonHp.SetActive(false);
        }

        //플레이어가 스타트 지점에서 벗어났을 경우
        if (!SaveManager.Instance.isStartPoint) 
            player.transform.position = SaveManager.Instance.playerPosition;// 이렇게 하면 현재 보는 화면의 좌표를 기준으로 플레이어가 이동된다.
                                                                            // 방금 클리어 및 눌렀던 스테이지의 위치에 이동시켜줘야한다.

        currentCoinText.text = DataManager.Instance.currentCoin.ToString();
        currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";

    }
}
