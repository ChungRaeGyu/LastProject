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

    public int[,] isStage;

    public int x;
    public int y;

    public List<GameObject> monsters = new List<GameObject>();
    public List<GameObject> spawnMonsters = new List<GameObject>();

    public System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        stage = new GameObject[x, y];

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
                    }
                }
            }
        }

        SaveManager.Instance.playerPosition = stage[x / 2, 0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MonsterSpawn()
    {
        int randomNum1 = random.Next(0, 3);
        int randomNum2 = random.Next(0, 10);
        int random3 = random.Next(0, 2);
        int monsterType = random.Next(0, monsters.Count - 1);

        if (randomNum1 >= 0 && randomNum1 < 5)
        {
            if(randomNum2 >=0 && randomNum2 < 5)
            {
                spawnMonsters.Add(monsters[3]);
                spawnMonsters.Add(monsters[4]);
                spawnMonsters.Add(monsters[3]);
                spawnMonsters.Add(monsters[5]);
            }
            else
            {
                for(int i = 0; i < 4; i++)
                {
                    spawnMonsters.Add(monsters[random3]);
                }
            }
        }
        if (randomNum1 >= 5 && randomNum1 < 25)
        {
            spawnMonsters.Add(monsters[monsterType]);
            spawnMonsters.Add(monsters[monsterType]);
            spawnMonsters.Add(monsters[monsterType]);
        }
        if(randomNum1 >= 25 && randomNum1 < 60)
        {
            spawnMonsters.Add(monsters[monsterType]);
            spawnMonsters.Add(monsters[monsterType]);
        }
        else
        {
            spawnMonsters.Add(monsters[monsterType]);
        }
        DataManager.Instance.Monsters = spawnMonsters;
    }
}
