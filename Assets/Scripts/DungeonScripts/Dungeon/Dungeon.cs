using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

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

    int[] num = new int[2];
    // Start is called before the first frame update
    void Start()
    {
        DungeonMapping();

        if (SaveManager.Instance.isStartPoint)
        {
            SaveManager.Instance.isStartPoint = false;
        }

        dungeonLength = stage[(x - 1) / 2, y - 1].transform.position.x - stage[(x-1)/2, 0].transform.position.x;
        

        ListUp();
        StageOpen();
        if (!SaveManager.Instance.isStartPoint)
            DungeonManager.Instance.player.transform.position = returnPosition();// 이렇게 하면 현재 보는 화면의 좌표를 기준으로 플레이어가 이동된다.
                                                                                 // 방금 클리어 및 눌렀던 스테이지의 위치에 이동시켜줘야한다.

        DungeonManager.Instance.deckCountText.text = DataManager.Instance.deckList.Count.ToString();
    }

    private void StageOpen()
    {
        //초기값 3,0
        //값이 >=0 , 값이 <x;
        //기준 값 (1,1),(0,2),(-1,1)
        num[0] = DataManager.Instance.initnum[0] + 1;
        num[1] = DataManager.Instance.initnum[1] + 1;
        StageControl(num[0], num[1],true);
        num[0] = DataManager.Instance.initnum[0] + 0;
        num[1] = DataManager.Instance.initnum[1] + 2;
        StageControl(num[0], num[1], true);
        num[0] = DataManager.Instance.initnum[0] - 1;
        num[1] = DataManager.Instance.initnum[1] + 1;
        StageControl(num[0], num[1], true);
    }
    public void StageClose()
    {
        num[0] = DataManager.Instance.initnum[0] + 1;
        num[1] = DataManager.Instance.initnum[1] + 1;
        StageControl(num[0], num[1], false);
        num[0] = DataManager.Instance.initnum[0] + 0;
        num[1] = DataManager.Instance.initnum[1] + 2;
        StageControl(num[0], num[1], false);
        num[0] = DataManager.Instance.initnum[0] - 1;
        num[1] = DataManager.Instance.initnum[1] + 1;
        StageControl(num[0], num[1], false);
    }
    private void StageControl(int numX,int numY,bool set)
    {
        if (0 <= numX && numX < x)
        {
            if (0 <= numY && numY < y)
            {
                if (stage[numX, numY] == null) return;
                GameObject tempChild = stage[numX, numY].transform.GetChild(0).gameObject;
                GameObject tempChild2 = stage[numX, numY].transform.GetChild(2).gameObject;

                tempChild.SetActive(set);
                tempChild2.GetComponent<Button>().enabled = set;
            }

        }
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
                        {
                            stage[i, j] = Instantiate(startStage, stageBoard);
                            stage[i, j].GetComponent<Stage>().SetValue(i, j);
                        }
                        else if (i == (x - 1) / 2 && j == (y - 1)){
                            stage[i, j] = Instantiate(bossStage, stageBoard);
                            stage[i, j].GetComponent<Stage>().SetValue(i, j);

                        }
                        else if (i == 0 && j == (y - 1) / 2)
                        {
                            stage[i, j] = Instantiate(warpStage, stageBoard);
                            stage[i, j].GetComponent<Stage>().SetValue(i, j);
                        }
                        else if (i == (x - 1) && j == (y - 1) / 2)
                        {
                            stage[i, j] = Instantiate(warpStage, stageBoard);
                            stage[i, j].GetComponent<Stage>().SetValue(i, j);
                        }
                        else
                        {
                            switch (SaveManager.Instance.num[i, j])
                            {
                                case 0:
                                case 1:
                                    stage[i, j] = Instantiate(eliteStage, stageBoard);
                                    stage[i, j].GetComponent<Stage>().SetValue(i, j);
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    stage[i, j] = Instantiate(storeStage, stageBoard);
                                    stage[i, j].GetComponent<Stage>().SetValue(i, j);
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                    stage[i, j] = Instantiate(eventStage, stageBoard);
                                    stage[i, j].GetComponent<Stage>().SetValue(i, j);
                                    break;
                                default:
                                    stage[i, j] = Instantiate(battleStage, stageBoard);
                                    stage[i, j].GetComponent<Stage>().SetValue(i, j);
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
    }

    public void MonsterSet()
    {
        dungeonProgress = (SaveManager.Instance.playerPosition.x - stage[((x - 1) / 2), 0].transform.position.x) / dungeonLength * 10;
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

    public void GetValue(int numX, int numY)
    {
        DataManager.Instance.initnum[0] = numX;
        DataManager.Instance.initnum[1] = numY;

    }

    public Vector2 returnPosition()
    {
        return stage[DataManager.Instance.initnum[0], DataManager.Instance.initnum[1]].transform.position;
    }
}
