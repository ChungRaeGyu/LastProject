using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    #region �̱���
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
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //�⺻ī��� ��� ��
                                                              //���ӽ��� ��ư�� ������ �� deckList�� �־������)
                                                          //�ִ� 6�� ���� 6�� �̸��� ���ӽ��� �Ұ���
    public int[] LobbyDeckRateCheck = new int[(int)Rate.Count];//��޺� ���� ���� ����

    [HideInInspector]
    public List<CardBasic> deckList = new List<CardBasic>(); //�������� ����� �� ����Ʈ
    public Stack<CardBasic> deck = new Stack<CardBasic>(); //���� ī�带 �̴� ��( deckList�� ����ؼ� �־��ش�.)

    [Header("Used Cards")]
    public List<CardBasic> usedCards = new List<CardBasic>(); // ���� ī�� ����Ʈ

    [Header("GameObjects")]
    public List<CardBasic> cardObjs = new List<CardBasic>();//���� �����͸� ��� ������ �ִ� ��

    [Header("CardPiece")]
    public int[] CardPiece;

    [Header("CardBack")]
    public Sprite cardBackImage;

    [Header("current Battle Monsters")]
    public List<GameObject> Monsters = new List<GameObject>();

    //�÷��̾��� ������ �����ϴ� ������
    public int maxHealth;
    public int currenthealth { get; set; }

    // ���� Ŭ���� ��ϵ�
    public int ClearMonstersKilledCount { get; set; }
    public int ClearStageClearCount { get; set; }
    public int ClearTotalClearTime { get; set; }
    public int ClearBossesDefeatedCount { get; set; }
    public int ClearTotalCrystal { get; set; }

    // ���� �й� ��ϵ�
    public int DefeatMonstersKilledCount { get; set; }
    public int DefeatStageClearCount { get; set; }
    public int DefeatTotalCrystal { get; set; }

    // �¸� ���� ��� ������
    public int adjustedCurrentCoin { get; set; }
    public int adjustedClearMonstersKilledCount { get; set; }
    public int adjustedClearStageClearCount { get; set; }
    public int adjustedClearTime { get; set; }

    public int adjustedBossesDefeatedCount { get; set; }


    // �й� ���� ��� ������
    public int adjustedDefeatMonstersKilledCount { get; set; }
    public int adjustedDefeatStageClearCount { get; set; }

    // ���� ���� �� ������ ī�� ��
    public int removeCardCount;

    //������ ����
    public int accessDungeonNum;
    public int openDungeonNum;

    // ���� ��ȭ
    //public int currentCoin { get; set; }
    public int currentCoin; // �׽�Ʈ������ �ν����Ϳ��� ������ �����ϰ� �ص�
    // public int currentCrystal { get; set; } = 0; // �ϴ� 0���� (�ӽ�)
    public int currentCrystal; // �׽�Ʈ������ �ν����Ϳ��� ������ �����ϰ� �ص�

    public int[] initnum = { 3, 0 };

    public bool[] accessibleDungeon = new bool[5];

    string path;

    private void Start()
    {
        Init();
    }

    public void OnApplicationQuit()
    {
        //���� ������ ����
        Save();
    }

    public void OnGameExitButtonClick()
    {
        // ������ ����
        Save();

        // �����Ͱ� ������ ����� �Ŀ� �� ����
        Application.Quit();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // ���� ��׶���� ��ȯ�� �� �����͸� ����
            Save();
        }
    }

    private void Init()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");
        accessibleDungeon[0] = true;
        CardPiece = new int[(int)Rate.Count];
        RateSort();
        Load();
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

        //���� ���۽� �����ϰ� �ϱ�
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

            // ��� ��ٸ�
        }
        return deck.Pop();
    }

    public void AddUsedCard(CardBasic usedCard)
    {
        usedCards.Add(usedCard);
    }

    public void DeckClear()
    {
        //�������� ����� ���
        deck.Clear();
        usedCards.Clear();
    }

    public void AddCard(CardBasic newCards)
    {
        deckList.Add(newCards);
        newCards.isFind = true;
    }

    // TotalCrystal�� ����ϴ� �޼���
    public void ClearCalculateTotalCrystal()
    {
        // �ݿø�
        //int adjustedCoin = Mathf.RoundToInt(currentCoin / 100f);
        // �Ҽ��� �Ʒ��� ���� (��ȭ�� 1���� �� �����μ� ���̵� ���)
        adjustedCurrentCoin = Mathf.FloorToInt(currentCoin / 10f);
        adjustedClearMonstersKilledCount = ClearMonstersKilledCount * 5;
        adjustedClearStageClearCount = ClearStageClearCount * 5;
        adjustedClearTime = Mathf.Max(300 - ClearTotalClearTime, 0);
        adjustedBossesDefeatedCount = ClearBossesDefeatedCount * 300;

        // TotalCrystal ���
        ClearTotalCrystal = adjustedCurrentCoin
                     + adjustedClearMonstersKilledCount
                     + adjustedClearStageClearCount
                     + adjustedClearTime
                     + adjustedBossesDefeatedCount;
    }

    public void DefeatCalculateTotalCrystal()
    {
        adjustedDefeatMonstersKilledCount = DefeatMonstersKilledCount * 5;
        adjustedDefeatStageClearCount = DefeatStageClearCount * 5;

        // TotalCrystal ���
        DefeatTotalCrystal = adjustedDefeatMonstersKilledCount
                     + adjustedDefeatStageClearCount;
    }

    // ��� �ʱ�ȭ
    public void ResetRecord()
    {
        currentCoin = 50;
        ClearMonstersKilledCount = 0;
        ClearStageClearCount = 0;
        ClearTotalClearTime = 0;
        ClearBossesDefeatedCount = 0;
        ClearTotalCrystal = 0;

        DefeatMonstersKilledCount = 0;
        DefeatStageClearCount = 0;
        DefeatTotalCrystal = 0;
    }

    // �÷��̾� ü�� �ʱ�ȭ
    public void ResetPlayerHealth()
    {
        currenthealth = 0; // �ϴ� 0���� �ʱ�ȭ (0�϶� �ִ밪���� ���� �س��� ������ player�� ������)
    }
    public void DungeonBoolSetting()
    {
        if (openDungeonNum < accessibleDungeon.Length)
        {
            for (int i = 0; i <= openDungeonNum; i++)
            {

                accessibleDungeon[i] = true;
            }
        }
    }
    public void Save()
    {
        SaveData saveData = new SaveData();

        for(int i=0; i < cardObjs.Count;i++)
        {
            saveData.isFind[i] = cardObjs[i].isFind;
            saveData.currentCount[i] = cardObjs[i].currentCount;
            saveData.enhance[i] = cardObjs[i].enhancementLevel;
        }
        saveData.cardPiece = CardPiece;
        saveData.LobbyDeck = LobbyDeck;
        saveData.currentCrystal = currentCrystal;
        saveData.openDungeonNum = openDungeonNum;


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
        Debug.Log("����");
    }

    public void Load()
    {
        if (!File.Exists(path)) return;

        SaveData saveData = new SaveData();
        string loadJson = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        
        //Todo: �ε� �� �ֱ�
        for (int i = 0; i < cardObjs.Count; i++)
        {
            cardObjs[i].isFind = saveData.isFind[i];
            cardObjs[i].currentCount = saveData.currentCount[i];
            cardObjs[i].enhancementLevel = saveData.enhance[i];
        }
       ;
        CardPiece = saveData.cardPiece;
        LobbyDeck = saveData.LobbyDeck;
        currentCrystal = saveData.currentCrystal;
        openDungeonNum = saveData.openDungeonNum;



        /*
        if (saveData.activeScenebuildindex == 3)
        {
            SceneManager.LoadScene(saveData.activeScenebuildindex);
            GameManager.instance.player.currenthealth = saveData.currenthealth;
            GameManager.instance.player.currentAttack = saveData.currentAttack;
            GameManager.instance.player.currentDefense = saveData.currentDefense;
            GameManager.instance.player.defdown = saveData.defdown;
            GameManager.instance.player.playerStats = saveData.playerStat;
            //Todo : �����̻� �ֱ� ������ �غ�����....
        }*/
        Debug.Log("�ε�Ϸ�");
    }

}
