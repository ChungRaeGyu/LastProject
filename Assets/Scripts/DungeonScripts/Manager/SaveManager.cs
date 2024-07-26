using System;
using System.Collections;
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
    }

    //������ ���� ���� ����
    public bool[] accessibleDungeon = new bool[5];

    //���� ���� ����
    public bool accessDungeon;

    //������ ����
    public int accessDungeonNum;

    //�÷��̾� ��ġ ����
    public Vector3 playerPosition;

    //���� ������������ üũ
    public bool isBossStage;

    [Header("UI")]
    public TMP_Text timeText; // UI �ؽ�Ʈ ������Ʈ
    public GameObject dungeonTimer; // Ÿ�̸Ӱ� �� UI ������Ʈ

    public Stopwatch stopwatch; // �ð� ������ ���� Stopwatch

    void Start()
    {
        accessDungeon = false;

        accessibleDungeon[0] = true;
        for(int i = 1; i< accessDungeonNum; i++)
        {
            accessibleDungeon[i] = false;
        }

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
            dungeonTimer.SetActive(true);
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
