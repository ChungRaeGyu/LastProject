using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Test : MonoBehaviour
{
    public GameObject[] stage;
    public GameObject stagePrefab;

    //public Transform stagePosition;

    public int[,] isStage;
    public Vector2[,] stagePosition;

    // Start is called before the first frame update
    void Start()
    {
        isStage = new int[7, 9] { { 0, 0, 0, 1, 0, 1, 0, 0, 0 },
                                  { 0, 0, 1, 0, 1, 0, 1, 0, 0 },
                                  { 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                                  { 1, 0, 1, 0, 1, 0, 1, 0, 1 },
                                  { 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                                  { 0, 0, 1, 0, 1, 0, 1, 0, 0 },
                                  { 0, 0, 0, 1, 0, 1, 0, 0, 0 } };

        //{0,0}

        stagePosition = new Vector2[7, 9];

        for (int a = 0; a < 7; a++)
        {
            for (int b = 0; b < 9; b++)
            {
                stagePosition[a, b] = new Vector2(b, a * 1.8f);
            }
        }

        stage = new GameObject[63];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (isStage[i, j] == 1)
                {
                    stage[9 * i + j] = Instantiate(stagePrefab, stagePosition[i, j], Quaternion.identity);
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (SaveManager.Instance.playerPosition == stage[0].transform.position)
        {
            // (stageNum[i, j] == 0)
            {
                //stageNum[i + 1, j];
                //stageNum[i + 2, j];
                //stageNum[i + 1, j + 1];
            }
        }
    }
}
