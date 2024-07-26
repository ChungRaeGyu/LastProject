using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon1 : MonoBehaviour
{
    public GameObject[] stage;

    public int[] isStage;
    public int[,] stageNum;
    
    // ¹è¿­¿ë
    public int i;
    public int j;

    // Start is called before the first frame update
    void Start()
    {
        isStage = new int[] { 0, 0, 0, 1, 0, 1, 0, 0, 0,
                              0, 0, 1, 1, 1, 1, 1, 0, 0,
                              0, 1, 1, 1, 1, 1, 1, 1, 0,
                              1, 1, 1, 1, 1, 1, 1, 1, 1,
                              0, 1, 1, 1, 1, 1, 1, 1, 0,
                              0, 0, 1, 1, 1, 1, 1, 0, 0,
                              0, 0, 0, 1, 0, 1, 0, 0, 0 };
        stageNum = new int[,] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 }, { 0, 5 }, { 0, 6 }, { 0, 7 }, { 0, 8 }, 
                                { 1, 0 }, { 1, 1 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 }, { 1, 6 }, { 1, 7 }, { 1, 8 },
                                { 2, 0 }, { 2, 1 }, { 2, 2 }, { 2, 3 }, { 2, 4 }, { 2, 5 }, { 2, 6 }, { 2, 7 }, { 2, 8 },
                                { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 }, { 3, 4 }, { 3, 5 }, { 3, 6 }, { 3, 7 }, { 3, 8 },
                                { 4, 0 }, { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 4, 5 }, { 4, 6 }, { 4, 7 }, { 4, 8 },
                                { 5, 0 }, { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }, { 5, 5 }, { 5, 6 }, { 5, 7 }, { 5, 8 },
                                { 6, 0 }, { 6, 1 }, { 6, 2 }, { 6, 3 }, { 6, 4 }, { 6, 5 }, { 6, 6 }, { 6, 7 }, { 6, 8 },
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveManager.Instance.playerPosition == stage[0].transform.position)
        {
            if (stageNum[i, j] == 0)
            {
                //stageNum[i + 1, j];
                //stageNum[i + 2, j];
                //stageNum[i + 1, j + 1];
            }
        }
    }
}
