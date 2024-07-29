using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager_Test : MonoBehaviour
{
    public static SaveManager_Test Instance = null;

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

    //던전에 입장 가능 여부
    public bool[] accessibleDungeon = new bool[5];

    //던전 입장 여부
    public bool accessDungeon;

    //입장한 던전
    public int accessDungeonNum;

    //플레이어 위치 저장
    public Vector3 playerPosition;

    //현재 보스던전인지 체크
    public bool isBossStage;

    void Start()
    {
        accessDungeon = false;

        accessibleDungeon[0] = true;
        for (int i = 1; i < accessDungeonNum; i++)
        {
            accessibleDungeon[i] = false;
        }
    }

    void Update()
    {
        
        
    }
}
