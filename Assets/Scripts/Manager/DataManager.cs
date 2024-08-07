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
    public int monstersKilledCount { get; set; }
    public int stageClearCount { get; set; }
    public int totalClearTime { get; set; }
    public int bossesDefeatedCount { get; set; }
    public int totalCrystal { get; set; }

    // ���� ��� ������
    public int adjustedCurrentCoin { get; set; }
    public int adjustedClearTime { get; set; }

    // ���� ���� �� ������ ī�� ��
    public int removeCardCount;

    //������ ����
    public int accessDungeonNum;

    // ���� ��ȭ
    //public int currentCoin { get; set; }
    public int currentCoin; // �׽�Ʈ������ �ν����Ϳ��� ������ �����ϰ� �ص�
    // public int currentCrystal { get; set; } = 0; // �ϴ� 0���� (�ӽ�)
    public int currentCrystal; // �׽�Ʈ������ �ν����Ϳ��� ������ �����ϰ� �ص�

    string path;
    private void Start()
    {
        Init();
    }
    private void OnApplicationQuit()
    {
        //���� ������ ����
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
        Debug.Log("����");
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
    public void CalculateTotalCrystal()
    {
        // �ݿø�
        //int adjustedCoin = Mathf.RoundToInt(currentCoin / 100f);
        // �Ҽ��� �Ʒ��� ���� (��ȭ�� 1���� �� �����μ� ���̵� ���)
        adjustedCurrentCoin = Mathf.FloorToInt(currentCoin / 100f);
        adjustedClearTime = Mathf.Max(300 - totalClearTime, 0);

        // TotalCrystal ���
        totalCrystal = adjustedCurrentCoin
                     + monstersKilledCount
                     + stageClearCount
                     + adjustedClearTime
                     + bossesDefeatedCount;
    }

    // ��� �ʱ�ȭ
    public void ResetRecord()
    {
        currentCoin = 0;
        monstersKilledCount = 0;
        stageClearCount = 0;
        totalClearTime = 0;
        bossesDefeatedCount = 0;
        totalCrystal = 0;
    }

    // �÷��̾� ü�� �ʱ�ȭ
    public void ResetPlayerHealth()
    {
        currenthealth = 0; // �ϴ� 0���� �ʱ�ȭ (0�϶� �ִ밪���� ���� �س��� ������ player�� ������)
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
        Debug.Log("����");
    }

    public void Load()
    {
        if (!File.Exists(path)) return;

        SaveData saveData = new SaveData();
        string loadJson = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        //Todo: �ε� �� �ֱ�
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
            //Todo : �����̻� �ֱ� ������ �غ�����....
        }*/
        Debug.Log("�ε�Ϸ�");
    }
}
