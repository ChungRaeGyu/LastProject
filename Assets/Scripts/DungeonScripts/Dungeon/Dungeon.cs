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

    public List<GameObject> monsters = new List<GameObject>();
    public List<List<GameObject>> setList = new List<List<GameObject>>();

    public List<GameObject> set2_1 = new List<GameObject>();
    public List<GameObject> set2_2 = new List<GameObject>();
    public List<GameObject> set2_3 = new List<GameObject>();
    public List<GameObject> set2_4 = new List<GameObject>();
    public List<GameObject> set3_1 = new List<GameObject>();
    public List<GameObject> set3_2 = new List<GameObject>();
    public List<GameObject> set3_3 = new List<GameObject>();
    public List<GameObject> set4_1 = new List<GameObject>();
    public List<GameObject> set4_2 = new List<GameObject>();
    public List<GameObject> set4_goblins = new List<GameObject>();

    public List<GameObject> Boss = new List<GameObject>();
    public List<GameObject> spawnMonsters = new List<GameObject>();

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
                        stage[i, j].transform.position = new Vector2(j * 0.75f - 5.25f, i * 1.3f - 3.9f);
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
            DungeonManager.Instance.player.transform.position = DungeonManager.Instance.stagePosition.transform.position;
            SaveManager.Instance.isStartPoint = false;
        }
    }

    public void MonsterSpawn()
    {
        MonsterSet();
        int respawn = random.Next(0, setList.Count - 1);

        if (SaveManager.Instance.isBossStage)
            DataManager.Instance.Monsters = Boss;
        else
            DataManager.Instance.Monsters = setList[respawn];
        Debug.Log("몬스터 스폰");
    }

    public void MonsterSet()
    {
        setList.Add(set2_1);
        setList.Add(set2_2);
        setList.Add(set2_3);
        setList.Add(set2_4);
        setList.Add(set3_1);
        setList.Add(set3_2);
        setList.Add(set3_3);
        setList.Add(set4_1);
        setList.Add(set4_2);
        setList.Add(set4_goblins);
    }
}
