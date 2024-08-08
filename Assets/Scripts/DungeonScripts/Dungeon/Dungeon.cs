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
    public List<GameObject> FirstMob1 = new List<GameObject>();
    public List<GameObject> FirstMob2 = new List<GameObject>();
    public List<GameObject> FirstMob3 = new List<GameObject>();
    public List<GameObject> FirstMob4 = new List<GameObject>();
    public List<GameObject> SecondMob1 = new List<GameObject>();
    public List<GameObject> SecondMob2 = new List<GameObject>();
    public List<GameObject> SecondMob3 = new List<GameObject>();
    public List<GameObject> SecondMob4 = new List<GameObject>();
    public List<GameObject> SecondMob5 = new List<GameObject>();
    public List<GameObject> SecondMob6 = new List<GameObject>();
    public List<GameObject> ThirdMob1 = new List<GameObject>();
    public List<GameObject> ThirdMob2 = new List<GameObject>();
    public List<GameObject> ThirdMob3 = new List<GameObject>();
    public List<GameObject> ThirdMob4 = new List<GameObject>();
    public List<List<GameObject>> MobList = new List<List<GameObject>>();

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

        ListUp();
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
                        if (i == (x - 1) / 2 && j == 0)
                            stage[i, j] = Instantiate(startStage, stageBoard);
                        else if (i == (x - 1) / 2 && j == (y - 1))
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
        if (SaveManager.Instance.isBossStage)
        {
            DataManager.Instance.Monsters = Boss;
        }
        else if (SaveManager.Instance.isEliteStage)
        {
            EliteSet();
            DataManager.Instance.Monsters = EliteMobList;
        }
        else
        {
            MonsterSet();
        }
        Debug.Log("몬스터 스폰");
    }

    public void MonsterSet()
    {
        dungeonProgress = (SaveManager.Instance.playerPosition.x - stage[((Dungeon.Instance.x - 1) / 2), 0].transform.position.x) / dungeonLength * 10;
        Debug.Log(dungeonProgress);
        int rand = random.Next(0, 4);
        int mob;

        if (dungeonProgress < 3.5)
        {
            mob = random.Next(1, 4);
            DataManager.Instance.Monsters = MobList[mob];
        }
        else if(dungeonProgress >=3.5 && dungeonProgress < 7)
        {
            mob = random.Next(5, 10);
            DataManager.Instance.Monsters = MobList[mob];
        }
        else
        {
            mob = random.Next(11, 14);
            DataManager.Instance.Monsters = MobList[mob];
        }
    }

    public void EliteSet()
    {
        int mob = random.Next(1, 2); //몬스터 종류
        EliteMobList.Add(eliteMob[mob]);
    }

    public void ListUp()
    {
        MobList.Add(FirstMob1);
        MobList.Add(FirstMob2);
        MobList.Add(FirstMob3);
        MobList.Add(FirstMob4);
        MobList.Add(SecondMob1);
        MobList.Add(SecondMob2);
        MobList.Add(SecondMob3);
        MobList.Add(SecondMob4);
        MobList.Add(SecondMob5);
        MobList.Add(SecondMob6);
        MobList.Add(ThirdMob1);
        MobList.Add(ThirdMob2);
        MobList.Add(ThirdMob3);
        MobList.Add(ThirdMob4);
    }
}
