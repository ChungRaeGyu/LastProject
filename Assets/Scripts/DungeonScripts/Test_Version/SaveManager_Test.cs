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
