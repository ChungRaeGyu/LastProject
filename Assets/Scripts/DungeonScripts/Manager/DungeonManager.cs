using TMPro;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //�ν��Ͻ�
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
        //������ �������� ��
        if (SaveManager.Instance.accessDungeon == true)
        {
            dungeonBoard.SetActive(false); //���� ���� ��Ȱ��ȭ
            dungeon.SetActive(true); //���� Ȱ��ȭ

            int num =   DataManager.Instance.accessDungeonNum;
            dungeonNum[num].SetActive(true);
            DungeonCoin.SetActive(true);
            DungeonHp.SetActive(true);
        }
        //������ �������� �ʾ�����
        else
        {
            dungeonBoard.SetActive(true); //���� ���� Ȱ��ȭ
            dungeon.SetActive(false); //���� ���� ��Ȱ��ȭ

            DungeonCoin.SetActive(false);
            DungeonHp.SetActive(false);
        }

        //�÷��̾ ��ŸƮ �������� ����� ���
        if (!SaveManager.Instance.isStartPoint) 
            player.transform.position = SaveManager.Instance.playerPosition;// �̷��� �ϸ� ���� ���� ȭ���� ��ǥ�� �������� �÷��̾ �̵��ȴ�.
                                                                            // ��� Ŭ���� �� ������ ���������� ��ġ�� �̵���������Ѵ�.

        currentCoinText.text = DataManager.Instance.currentCoin.ToString();
        currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";

    }
}
