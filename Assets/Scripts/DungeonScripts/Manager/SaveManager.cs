using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        accessDungeon = false;
        isStartPoint = false;

    }



    //������ ���� ���� ����
    

    //���� ���� ����
    public bool accessDungeon;



    //�� ��ġ�� ���� ���� ���������� ���� ����
    public bool isStartPoint;

    //�÷��̾� ��ġ ����
    public Vector3 playerPosition;
    
    //���� ����Ʈ������ üũ
    public bool isEliteStage;

    //���� ������������ üũ
    public bool isBossStage;

    //�������� ���� ����
    public int[,] num;
    public List<GameObject> stageList = new List<GameObject>(); //stage�ӽ������

    [Header("UI")]
    public TMP_Text timeText; // UI �ؽ�Ʈ ������Ʈ
    public GameObject dungeonTimer; // Ÿ�̸Ӱ� �� UI ������Ʈ

    public Stopwatch stopwatch; // �ð� ������ ���� Stopwatch


    public System.Random random = new System.Random();
    void Start()
    {
        // Stopwatch �ʱ�ȭ
        if (stopwatch == null)
        {
            stopwatch = new Stopwatch();
        }

        dungeonTimer.SetActive(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (DungeonManager.Instance != null && DungeonManager.Instance.dungeon.activeSelf)
            {
                dungeonTimer.SetActive(true);
            }
            else
            {
                dungeonTimer.SetActive(false);
            }
        }
        else
        {
            dungeonTimer.SetActive(false);
        }

        if (stopwatch.IsRunning && timeText != null)
        {
            // ��� �ð� ������Ʈ
            timeText.text = FormatTime(stopwatch.Elapsed.TotalSeconds);
        }
    }

    public string FormatTime(double seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
                              time.Hours,
                              time.Minutes,
                              time.Seconds);
    }

    public void StartTrackingTime()
    {
        stopwatch.Reset(); // Stopwatch �ʱ�ȭ
        stopwatch.Start(); // �ð� ���� ����
    }

    public void StopTrackingTime()
    {
        stopwatch.Stop(); // �ð� ���� ����
    }
}
