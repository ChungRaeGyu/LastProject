using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    #region 싱글톤
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DataManager").AddComponent<DataManager>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
                Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    [Header("Deck")]
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //기본카드로 들고갈 덱
                                                              //게임시작 버튼을 눌렀을 때 deckList에 넣어줘야함)
                                                          //최대 6장 고정 6장 미만시 게임시작 불가능
    public int[] LobbyDeckRateCheck = new int[(int)Rate.Count];//등급별 개수 제한 변수

    [HideInInspector]
    public List<CardBasic> deckList = new List<CardBasic>(); //던전에서 사용할 덱 리스트
    public Stack<CardBasic> deck = new Stack<CardBasic>(); //실제 카드를 뽑는 덱( deckList를 사용해서 넣어준다.)

    [Header("Used Cards")]
    public List<CardBasic> usedCards = new List<CardBasic>(); // 사용된 카드 리스트

    [Header("GameObjects")]
    public List<CardBasic> cardObjs = new List<CardBasic>();//실제 데이터를 모두 가지고 있는 곳

    [Header("CardPiece")]
    public int[] CardPiece;

    [Header("CardBack")]
    public Sprite cardBackImage;

    [Header("current Battle Monsters")]
    public List<GameObject> Monsters = new List<GameObject>();

    //플레이어의 스탯을 저장하는 변수들
    public int maxHealth;
    public int currenthealth { get; set; }

    // 던전 클리어 기록들
    public int monstersKilledCount { get; set; }
    public int stageClearCount { get; set; }
    public int totalClearTime { get; set; }
    public int bossesDefeatedCount { get; set; }
    public int totalCrystal { get; set; }

    // 점수 계산 변수들
    public int adjustedCurrentCoin { get; set; }
    public int adjustedClearTime { get; set; }

    // 던전 진행 중 제거한 카드 수
    public int removeCardCount;

    //입장한 던전
    public int accessDungeonNum;

    // 게임 재화
    //public int currentCoin { get; set; }
    public int currentCoin; // 테스트용으로 인스펙터에서 변경이 가능하게 해둠
    // public int currentCrystal { get; set; } = 0; // 일단 0으로 (임시)
    public int currentCrystal; // 테스트용으로 인스펙터에서 변경이 가능하게 해둠

    string path;
    private void Start()
    {
        Init();
    }
    private void OnApplicationQuit()
    {
        //앱이 꺼질때 저장
        Save();
    }
    private void Init()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        Load();
        CardPiece = new int[(int)Rate.Count];
        RateSort();
        LobbyDeckRateCheckInit();
    }

    private void LobbyDeckRateCheckInit()
    {
        if (LobbyDeck.Count == 0) return;
        foreach (CardBasic cardBasic in LobbyDeck)
        {
            LobbyDeckRateCheck[(int)(cardBasic.rate)]++;
        }
    }

    private void RateSort()
    {
        Debug.Log("정리");
        cardObjs = cardObjs.OrderBy(card => card.rate).ToList();
    }

    public void SuffleDeckList()
    {
        Suffle(deckList);
    }
    public void SuffleUsedCards()
    {
        Suffle(usedCards);
    }
    private void Suffle(List<CardBasic> deckList)
    {
        deck.Clear();

        //게임 시작시 셔플하게 하기
        List<CardBasic> temp = deckList.OrderBy(_ => UnityEngine.Random.Range(0, deckList.Count)).ToList();
        foreach (CardBasic tempCard in temp)
        {
            deck.Push(tempCard);
        }

        usedCards.Clear();
    }

    public CardBasic PopCard()
    {
        if (deck.Count == 0)
        {
            SuffleUsedCards();
        }
        return deck.Pop();
    }

    public void AddUsedCard(CardBasic usedCard)
    {
        usedCards.Add(usedCard);
    }

    public void DeckClear()
    {
        //스테이지 종료시 사용
        deck.Clear();
        usedCards.Clear();
    }

    public void AddCard(CardBasic newCards)
    {
        deckList.Add(newCards);
        newCards.isFind = true;
    }

    // TotalCrystal을 계산하는 메서드
    public void CalculateTotalCrystal()
    {
        // 반올림
        //int adjustedCoin = Mathf.RoundToInt(currentCoin / 100f);
        // 소수점 아래를 버림 (재화를 1개라도 덜 줌으로서 난이도 상승)
        adjustedCurrentCoin = Mathf.FloorToInt(currentCoin / 100f);
        adjustedClearTime = Mathf.Max(300 - totalClearTime, 0);

        // TotalCrystal 계산
        totalCrystal = adjustedCurrentCoin
                     + monstersKilledCount
                     + stageClearCount
                     + adjustedClearTime
                     + bossesDefeatedCount;
    }

    // 기록 초기화
    public void ResetRecord()
    {
        currentCoin = 0;
        monstersKilledCount = 0;
        stageClearCount = 0;
        totalClearTime = 0;
        bossesDefeatedCount = 0;
        totalCrystal = 0;
    }

    // 플레이어 체력 초기화
    public void ResetPlayerHealth()
    {
        currenthealth = 0; // 일단 0으로 초기화 (0일때 최대값으로 들어가게 해놓은 로직이 player에 존재함)
    }

    public void Save()
    {
        SaveData saveData = new SaveData();
        saveData.cardObjs = cardObjs;
        saveData.LobbyDeck = LobbyDeck;
        saveData.currentCrystal = currentCrystal;
        saveData.currentHealth = currenthealth;
        saveData.maxHealth = maxHealth;
        saveData.accessibleDungeonNum = accessDungeonNum;
        //saveData.dataManager = DataManager.Instance;
        //PlayerCharacter
        /*
        if (saveData.activeScenebuildindex == 3)
        {
            saveData.activeScenebuildindex = SceneManager.GetActiveScene().buildIndex;
            saveData.currenthealth = GameManager.instance.player.currenthealth;
            saveData.currentAttack = GameManager.instance.player.currentAttack;
            saveData.currentDefense = GameManager.instance.player.currentDefense;
            saveData.defdown = GameManager.instance.player.defdown;
            saveData.playerStat = GameManager.instance.player.playerStats;
        }*/
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
        Debug.Log("저장");
    }

    public void Load()
    {
        if (!File.Exists(path)) return;

        SaveData saveData = new SaveData();
        string loadJson = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        //Todo: 로딩 씬 넣기
        cardObjs = saveData.cardObjs;
        LobbyDeck = saveData.LobbyDeck;
        currentCrystal = saveData.currentCrystal;
        currenthealth = saveData.currentHealth;
        maxHealth = saveData.maxHealth;
        accessDungeonNum = saveData.accessibleDungeonNum;
        /*
        if (saveData.activeScenebuildindex == 3)
        {
            SceneManager.LoadScene(saveData.activeScenebuildindex);
            GameManager.instance.player.currenthealth = saveData.currenthealth;
            GameManager.instance.player.currentAttack = saveData.currentAttack;
            GameManager.instance.player.currentDefense = saveData.currentDefense;
            GameManager.instance.player.defdown = saveData.defdown;
            GameManager.instance.player.playerStats = saveData.playerStat;
            //Todo : 상태이상 넣기 언젠간 해봐야지....
        }*/
        Debug.Log("로드완료");
    }
}
