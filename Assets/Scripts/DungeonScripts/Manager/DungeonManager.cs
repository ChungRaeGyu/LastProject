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
    public GameObject dungeonBoardBackground;
    public GameObject[] dungeonEntrance = new GameObject[5];
    //public int accessDungeon = 1;

    [Header("Dungeon")]
    public GameObject[] dungeonNum = new GameObject[5];

    [Header("Player")]
    public GameObject player;

    [Header("Stage")]
    public GameObject stage;
    public GameObject eventScene;
    //public Vector3 stage01;

    [Header("TextUI")]
    public TMP_Text currentCoinText;
    public TMP_Text currentHpText;

    [Header("Info")]
    public GameObject DungeonCoin;
    public GameObject DungeonHp;

    public Player Player;

    public Transform stagePosition;


    void Start()
    {
        //������ �������� ��
        if (SaveManager.Instance.accessDungeon == true)
        {
            dungeonBoard.SetActive(false); //���� ���� ��Ȱ��ȭ
            dungeon.SetActive(true); //���� Ȱ��ȭ

            int num = SaveManager.Instance.accessDungeonNum;
            dungeonNum[num].SetActive(true);
            DungeonCoin.SetActive(true);
            DungeonHp.SetActive(true);

            stage.SetActive(true);
        }
        //������ �������� �ʾ�����
        else
        {
            dungeonBoard.SetActive(true); //���� ���� Ȱ��ȭ
            dungeon.SetActive(false); //���� ���� ��Ȱ��ȭ

            DungeonCoin.SetActive(false);
            DungeonHp.SetActive(false);

            stage.SetActive(false); //�������� ��Ȱ��ȭ
        }

        //�÷��̾ ��ŸƮ �������� ����� ���
        if (!SaveManager.Instance.isStartPoint) 
            player.transform.position = SaveManager.Instance.playerPosition;// �̷��� �ϸ� ���� ���� ȭ���� ��ǥ�� �������� �÷��̾ �̵��ȴ�.
                                                                            // ��� Ŭ���� �� ������ ���������� ��ġ�� �̵���������Ѵ�.

        eventScene.SetActive(false);
        currentCoinText.text = DataManager.Instance.currentCoin.ToString();
        currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";
    }
}
