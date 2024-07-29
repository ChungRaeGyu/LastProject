using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const string PLAYER_TURN_TEXT = "�� ����";
    private const string ENEMY_TURN_TEXT = "�� ��";

    public bool playerTurn { get; private set; } = true;

    [Header("CharacterPrefab")]
    public GameObject playerPrefab;
    public List<GameObject> monsterPrefab = new List<GameObject>();

    [Header("CharacterSpawnPoint")]
    public Transform playerSpawnPoint;
    public List<Transform> monsterSpawnPoint1;
    public List<Transform> monsterSpawnPoint2;
    public List<Transform> monsterSpawnPoint3;
    public List<Transform> monsterSpawnPoint4;

    public Transform cardSpawnPoint; // ī�� ��ȯ ��ġ

    public Player player { get; private set; }
    public List<MonsterCharacter> monsters = new List<MonsterCharacter>();

    [Header("Manager")]
    public HandManager handManager; // �� �� �Ŵ���
    public EffectManager effectManager;

    [Header("MonsterInfoPrefabs")]
    public GameObject attackActionPrefab;
    public GameObject monsterNextActionListPrefab;
    public GameObject actionDescriptionPrefab;
    public GameObject monsterNamePrefab;
    
    [Header("Condition")]
    public GameObject conditionBoxPrefab;
    public Condition defenseconditionPrefab;
    public Condition frozenConditionPrefab;
    public Condition weakerConditionPrefab;
    public Condition defDownConditionPrefab;
    public Condition burnConditionPrefab; //ȭ�� �̷� ���� �Ѵ�. �ϴ� ȭ���� ������.
    public Condition poisonConditionPrefab;
    public Condition bleedingConditioinPrefab;

    [Header("DamageText")]
    public GameObject damageTextPrefab;

    [Header("AudioSource")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip CardDrawClip;

    [Header("ScrollView")]
    [SerializeField] private GameObject unUsedScrollView;
    [SerializeField] private GameObject usedScrollView;

    [Header("CardUtil")]
    public bool volumeUp = false;

    // ���Ϳ� ���� ���� ���� �ջ�
    public int monsterTotalRewardCoin;

    public CardListManager cardListManager;

    // ī�� �巡�׳� ��ư�۵��� ���ߴ� �����̸� ��ŵ�ϰ� �ϴ� ��
    public bool skip;

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
            GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            player = playerObject.GetComponent<Player>();
            Debug.Log($"{player}");
        }

        cardListManager = GetComponent<CardListManager>();

        // ���� ����
        SpawnMonsters();
    }

    private void Start()
    {
        StartCoroutine(Battle());
    }

    private void SpawnMonsters()
    {
        int i = 0;
        monsterPrefab = DataManager.Instance.Monsters;
        switch (monsterPrefab.Count)
        {
            case 1:
                foreach (Transform spawnPoint in monsterSpawnPoint1)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;
            case 2:
                foreach (Transform spawnPoint in monsterSpawnPoint2)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;
            case 3:
                foreach (Transform spawnPoint in monsterSpawnPoint3)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;
            case 4:
                foreach (Transform spawnPoint in monsterSpawnPoint4)
                {
                    GameObject monsterObject = Instantiate(monsterPrefab[i], spawnPoint.position, Quaternion.identity);
                    MonsterCharacter monsterCharacter = monsterObject.GetComponent<MonsterCharacter>();
                    monsters.Add(monsterCharacter);
                    i++;
                }
                break;

        }
    }

    public IEnumerator DrawInitialHand(int count, bool skip = true)
    {
        for (int i = 0; i < count; i++)
        {
            audioSource.PlayOneShot(CardDrawClip);

            yield return StartCoroutine(DrawCardFromDeck());
            yield return new WaitForSeconds(0.2f); // ��ο� �� ������
        }
    }

    private IEnumerator Battle()
    {
        int turnCount = 1;

        while (true)
        {
            Debug.Log("----- �÷��̾� �� ���� -----");
            playerTurn = true; // �÷��̾� �� ����
            UIManager.instance.UpdatePlayerTurnCount(turnCount);
            UIManager.instance.TurnText.text = PLAYER_TURN_TEXT; // �÷��̾� �� �ؽ�Ʈ ����

            player.InitializeCost();

            skip = false;
            // ������ ī�� ��ο�
            yield return StartCoroutine(DrawInitialHand(5));
            skip = true;

            foreach (MonsterCharacter monster in monsters)
            {
                if (monster.monsterNextAction != null)
                {
                    if (monster.frozenTurnsRemaining < 1)
                    monster.monsterNextAction.gameObject.SetActive(true); // ��� ������ ���� �׼� ������Ʈ Ȱ��ȭ
                }
            }

            yield return new WaitUntil(() => !playerTurn); // �÷��̾ ���� ��ĥ ������ ���

            Debug.Log("----- ���͵��� �� ���� -----");
            UIManager.instance.UpdateMonsterTurnCount(turnCount);
            UIManager.instance.TurnText.text = ENEMY_TURN_TEXT; // �� �� �ؽ�Ʈ ����

            // ��� ������ �� ���������� ����
            for (int i = 0; i < monsters.Count; i++)
            {
                MonsterCharacter monster = monsters[i];
                if (monster.currenthealth > 0)
                {
                    Debug.Log($"----- ������ �� ���� -----");
                    yield return StartCoroutine(monster.MonsterTurn());
                    yield return new WaitUntil(() => playerTurn); // �÷��̾� ���� �Ǳ� ������ ���
                }
            }

            Debug.Log("----- ���͵��� �� ���� -----");

            turnCount++;
        }
    }

    public IEnumerator DrawCardFromDeck()
    {
        CardBasic cardBasic = DataManager.Instance.PopCard();
        AddCard(cardBasic);
        yield return null;
    }

    public void AddCard(CardBasic cardBasic)
    {
        GameObject newCard = Instantiate(cardBasic.gameObject, cardSpawnPoint.position, Quaternion.identity); // ī�� ��ȯ ��ġ ���
        newCard.GetComponent<CardBasic>().cardBasic = cardBasic;
        handManager.AddCard(newCard.transform);
    }

    public void CheckAllMonstersDead()
    {
        if (AllMonstersDead())
        {
            StartCoroutine(WaitAndClearUI());
        }
    }

    private IEnumerator WaitAndClearUI()
    {
        UIManager.instance.ResetUIPositions();
        handManager.HideAllCards();
        DataManager.Instance.currenthealth = player.currenthealth;
        // ��� ��ٸ�
        yield return new WaitForSeconds(1.0f);

        UIManager.instance.UIClear(true, false, true, true, true);
    }

    private bool AllMonstersDead()
    {
        if (monsters.Count == 0)
            return true;

        Debug.Log($"���� ��: {monsters.Count}");
        return false;
    }

    public void RemoveMonsterDead(MonsterCharacter monster)
    {
        monsters.Remove(monster);
        CheckAllMonstersDead();
    }

    public void OnLobbyButtonClick()
    {
        DataManager.Instance.stageClearCount++;

        // ���� Ŭ���� Ȯ��
        if (SaveManager.Instance.isBossStage)
        {
            DataManager.Instance.bossesDefeatedCount++;
            SaveManager.Instance.StopTrackingTime();
            DataManager.Instance.totalClearTime = (int)Math.Floor(SaveManager.Instance.stopwatch.Elapsed.TotalSeconds);

            // Ŭ���� �г��� ��� ��
            UIManager.instance.victoryPanel.gameObject.SetActive(true);

            // �ؽ�Ʈ ������Ʈ
            UpdateVictoryTexts();

            // ȹ���� ũ����Ż ��� �� ǥ��
            DataManager.Instance.CalculateTotalCrystal();
            if (UIManager.instance.victoryTotalCrystal != null)
            {
                UIManager.instance.victoryTotalCrystal.text = $"{DataManager.Instance.totalCrystal}";
            }
        }
        else
        {
            SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);
            SceneManager.LoadScene(2);
        }
    }

    private void UpdateVictoryTexts()
    {
        DataManager.Instance.adjustedCurrentCoin = Mathf.FloorToInt(DataManager.Instance.currentCoin / 100f);
        DataManager.Instance.adjustedClearTime = Mathf.Max(300 - DataManager.Instance.totalClearTime, 0);

        SetText(UIManager.instance.victoryMonstersKilledText, $"óġ�� ���� ({DataManager.Instance.monstersKilledCount})");
        SetText(UIManager.instance.victoryStageClearCountText, $"Ŭ������ �������� ({DataManager.Instance.stageClearCount})");
        SetText(UIManager.instance.victoryTotalClearTimeText, $"���� Ŭ���� �ð� ({SaveManager.Instance.FormatTime(SaveManager.Instance.stopwatch.Elapsed.TotalSeconds)})");
        SetText(UIManager.instance.victoryBossesDefeatedCountText, $"���� óġ ({DataManager.Instance.bossesDefeatedCount})");
        SetText(UIManager.instance.victoryRemainingCoinCountText, $"�ܿ� ���� ({DataManager.Instance.currentCoin})");

        SetText(UIManager.instance.victoryMonstersKilledPointText, $"{DataManager.Instance.monstersKilledCount}");
        SetText(UIManager.instance.victoryStageClearCountPointText, $"{DataManager.Instance.stageClearCount}");
        SetText(UIManager.instance.victoryTotalClearTimePointText, $"{DataManager.Instance.adjustedClearTime}");
        SetText(UIManager.instance.victoryBossesDefeatedCountPointText, $"{DataManager.Instance.bossesDefeatedCount}");
        SetText(UIManager.instance.victoryRemainingCoinCountPointText, $"{DataManager.Instance.adjustedCurrentCoin}");
    }

    private void SetText(TMP_Text textComponent, string text)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
        }
    }

    // ���� �������� �رݰ� ���ε�, Ŭ���� ���� �� �ϴ��� ���� ��ư
    public void ClearGoToLobbyScene()
    {
        DataManager.Instance.currentCrystal += DataManager.Instance.totalCrystal;
        StageCheck();
        SceneManager.LoadScene(1);
    }

    private void StageCheck()
    {
        SaveManager.Instance.isBossStage = false;
        SaveManager.Instance.accessDungeon = false;
        if (SaveManager.Instance.accessDungeonNum < SaveManager.Instance.accessibleDungeon.Length - 1)
        {
            SaveManager.Instance.accessibleDungeon[SaveManager.Instance.accessDungeonNum + 1] = true;
        }
    }
    public void EndMonsterTurn()
    {
        playerTurn = true;
    }

    public void EndPlayerTurn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);

        if (!playerTurn) return;
        if (!handManager.setCardEnd) return;

        handManager.MoveUnusedCardsToUsed();
        playerTurn = false;
    }

    public void DeBuffAnim(GameObject deBuff)
    {
        StartCoroutine(DelayDestroy(deBuff));
    }
    IEnumerator DelayDestroy(GameObject deBuff)
    {
        if (deBuff != null)
        {
            deBuff.GetComponentInChildren<Animator>().SetTrigger("IceOff");
            yield return new WaitForSecondsRealtime(1f);
            Destroy(deBuff);
        }
    }

    // ScrollView�� Ȱ��ȭ/��Ȱ��ȭ ���� �޼���
    private void ToggleScrollView(GameObject scrollView, Action showCardsAction, Action hideCardsAction, Action resetUIAction, bool fadeRewardPanelActive, Action updateList)
    {
        if (scrollView != null)
        {
            if (scrollView.activeSelf)
            {
                // ��Ȱ��ȭ
                SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip2);
                UIManager.instance.MoveUIElementsToStartPositions();
                UIManager.instance.fadeRewardPanel.gameObject.SetActive(false);
                showCardsAction?.Invoke();
            }
            else
            {
                // Ȱ��ȭ
                updateList?.Invoke();
                SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardPassClip);
                resetUIAction?.Invoke();
                UIManager.instance.fadeRewardPanel.gameObject.SetActive(fadeRewardPanelActive);
                hideCardsAction?.Invoke();
            }

            scrollView.SetActive(!scrollView.activeSelf);
        }
    }

    // unUsedScrollView Ȱ��ȭ/��Ȱ��ȭ �޼���
    public void ToggleUnUsedScrollView()
    {
        ToggleScrollView(
            unUsedScrollView,
            handManager.ShowAllCardsActive,  // ī�� ǥ��
            handManager.HideAllCardsActive,  // ī�� �����
            UIManager.instance.UnUsedCardsResetUIPositions, // UI ��ġ �缳��
            true, // fadeRewardPanel Ȱ��ȭ
            cardListManager.UpdateDeckList
        );
    }

    // usedScrollView Ȱ��ȭ/��Ȱ��ȭ �޼���
    public void ToggleUsedScrollView()
    {
        ToggleScrollView(
            usedScrollView,
            handManager.ShowAllCardsActive,  // ī�� ǥ��
            handManager.HideAllCardsActive,  // ī�� �����
            UIManager.instance.UsedCardsResetUIPositions, // UI ��ġ �缳��
            true, // fadeRewardPanel Ȱ��ȭ
            cardListManager.UpdateUsedCardsList
        );
    }

}
