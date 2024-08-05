using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public static Dungeon Instance = null;

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

    public GameObject[,] stage;
    public GameObject stagePrefab;

    public Transform stageBoard;

    public bool[,] isStage;

    public int x;
    public int y;
    public float a;
    public float b;

    [Header("NormalMob")]
    public GameObject[] normalMob;
    public GameObject[] goblins;
    public List<GameObject> MobList = new List<GameObject>();

    [Header("EliteMob")]
    public GameObject[] eliteMob;
    public List<GameObject> EliteMobList = new List<GameObject>();

    [Header("Boss")]
    public List<GameObject> Boss = new List<GameObject>();

    public System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        stage = new GameObject[x, y];
        isStage = new bool[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (Mathf.Abs(i - (int)(x / 2)) <= j && j <= MathF.Abs(MathF.Abs(i - (int)(x / 2)) - (y - 1)))
                {
                    if (i % 2 != j % 2)
                    {
                        //여기다가 생성 로직을 짜면 된다.
                        stage[i, j] = Instantiate(stagePrefab, stageBoard);
                        stage[i, j].transform.position = new Vector2(j * 0.75f - a, i * 1.3f - b);
                        isStage[i, j] = true;
                    }
                    else
                        isStage[i, j] = false;
                }
                else
                    isStage[i, j] = false;
            }
        }

        if (SaveManager.Instance.isStartPoint)
        {
            SaveManager.Instance.playerPosition = stage[(x - 1) / 2, 0].transform.position;
            DungeonManager.Instance.player.transform.position = DungeonManager.Instance.startPosition.transform.position;
            SaveManager.Instance.isStartPoint = false;
        }
    }

    public void MonsterSpawn()
    {
        MonsterSet();
        EliteSet();

        if (SaveManager.Instance.isBossStage)
            DataManager.Instance.Monsters = Boss;
        else if (SaveManager.Instance.isEliteStage)
            DataManager.Instance.Monsters = EliteMobList;
        else
            DataManager.Instance.Monsters = MobList;
        Debug.Log("몬스터 스폰");
    }

    public void MonsterSet()
    {
        int randomBattle = random.Next(0, normalMob.Length); //고블린이 나올지 몬스터가 나올지에 대한 확률
        int mob = random.Next(0, normalMob.Length - 1); //몬스터 종류
        int num = random.Next(1, 4); //몬스터 수
        if (num == normalMob.Length)
        {
            for (int i = 0; i < num; i++)
            {
                int goblin = random.Next(0, goblins.Length - 1);
                MobList.Add(goblins[goblin]);
            }
        }
        else
        {
            for (int i = 0; i < num; i++)
            {
                MobList.Add(normalMob[mob]);
            }
        }
    }

    public void EliteSet()
    {
        int mob = random.Next(1, 2); //몬스터 종류
        int num = random.Next(1, 2); //몬스터 수
        for (int i = 0; i < num; i++)
            EliteMobList.Add(eliteMob[mob]);
    }
}
