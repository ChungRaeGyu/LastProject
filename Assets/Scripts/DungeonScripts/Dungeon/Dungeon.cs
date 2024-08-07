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

    [Header("Stage")]
    public GameObject[,] stage;
    public Transform stageBoard;
    public bool[,] isStage;

    [Header("DungeonArea")]
    public int x;
    public int y;
    public float a;
    public float b;
    public float dungeonLength;
    public float dungeonProgress;

    [Header("Stage")]
    public GameObject startStage;
    public GameObject battleStage;
    public GameObject warpStage;
    public GameObject eventStage;
    public GameObject storeStage;
    public GameObject eliteStage;
    public GameObject bossStage;

    [Header("NormalMob")]
    public GameObject[] normalMob;
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
        DungeonMapping();

        if (SaveManager.Instance.isStartPoint)
        {
            SaveManager.Instance.playerPosition = stage[(x - 1) / 2, 0].transform.position;
            DungeonManager.Instance.player.transform.position = DungeonManager.Instance.startPosition.transform.position;
            SaveManager.Instance.isStartPoint = false;
        }

        dungeonLength = stage[(x - 1) / 2, y - 1].transform.position.x - stage[(x-1)/2, 0].transform.position.x;
        Debug.Log(dungeonLength);
    }

    public void DungeonMapping()
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
                        if (i == (x - 1) && j == 0)
                            stage[i, j] = Instantiate(startStage, stageBoard);
                        else if (i == (x - 1) && j == (y - 1))
                            stage[i, j] = Instantiate(bossStage, stageBoard);
                        else if (i == 0 && j == (y - 1) / 2)
                            stage[i, j] = Instantiate(warpStage, stageBoard);
                        else if (i == (x - 1) && j == (y - 1) / 2)
                            stage[i, j] = Instantiate(warpStage, stageBoard);
                        else
                        {
                            switch (SaveManager.Instance.num[i, j])
                            {
                                case 0:
                                case 1:
                                    stage[i, j] = Instantiate(eliteStage, stageBoard);
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    stage[i, j] = Instantiate(storeStage, stageBoard);
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                    stage[i, j] = Instantiate(eventStage, stageBoard);
                                    break;
                                default:
                                    stage[i, j] = Instantiate(battleStage, stageBoard);
                                    break;
                            }
                        }
                        
                        stage[i, j].transform.position = new Vector2(j * 0.75f - a, i * 1.3f - b);
                    }
                }
            }
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
        dungeonProgress = (SaveManager.Instance.playerPosition.x - stage[((Dungeon.Instance.x - 1) / 2), 0].transform.position.x) / dungeonLength * 10;
        Debug.Log(dungeonProgress);
        int randomBattle = random.Next(0, normalMob.Length); //고블린이 나올지 몬스터가 나올지에 대한 확률
        int mob = random.Next(0, normalMob.Length - 1); //몬스터 종류
        float num = dungeonProgress * 0.4f;
        for (int i = 0; i < num; i++)
        {
            MobList.Add(normalMob[mob]);
        }
        /*
        int mob = random.Next(0, (int)(normalMob.Length * dungeonProgress));
        
        for (int i = 0; i < num; i++)
            MobList.Add(normalMob[mob]);
        */
    }

    public void EliteSet()
    {
        int mob = random.Next(1, 2); //몬스터 종류
        EliteMobList.Add(eliteMob[mob]);
    }

    
}
