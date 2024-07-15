using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        accessDungeon = false;
        accessDungeonNum = 0;

        accessibleDungeon[0] = true;
        for(int i = 1; i< accessDungeonNum; i++)
        {
            accessibleDungeon[i] = false;
        }
    }

    void Update()
    {
        
    }

    
}
