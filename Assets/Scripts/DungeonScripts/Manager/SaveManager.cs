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



    //던전에 입장 가능 여부
    

    //던전 입장 여부
    public bool accessDungeon;



    //현 위치가 던전 시작 지점인지에 대한 여부
    public bool isStartPoint;

    //플레이어 위치 저장
    public Vector3 playerPosition;
    
    //현재 엘리트몹인지 체크
    public bool isEliteStage;

    //현재 보스던전인지 체크
    public bool isBossStage;

    //스테이지 랜덤 생성
    public int[,] num;
    public List<GameObject> stageList = new List<GameObject>(); //stage임시저장소

    [Header("UI")]
    public TMP_Text timeText; // UI 텍스트 컴포넌트
    public GameObject dungeonTimer; // 타이머가 든 UI 오브젝트

    public Stopwatch stopwatch; // 시간 측정을 위한 Stopwatch


    public System.Random random = new System.Random();
    void Start()
    {
        // Stopwatch 초기화
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
            // 경과 시간 업데이트
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
        stopwatch.Reset(); // Stopwatch 초기화
        stopwatch.Start(); // 시간 측정 시작
    }

    public void StopTrackingTime()
    {
        stopwatch.Stop(); // 시간 측정 중지
    }
}
