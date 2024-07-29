using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_CH : MonoBehaviour
{
    //x는 홀수
    //y >=x
    public int boardX;
    public int boardY;
    bool[,] stages;
    
    void Start()
    {
        stages = new bool[boardX, boardY];
        Init();
    }

    private void Init()
    {
        //5 9
        for (int i = 0; i < boardX; i++)
        {
            for (int j = 0; j < boardY; j++)
            {
                if(Mathf.Abs(i-(int)(boardX / 2)) <= j && j <= MathF.Abs(MathF.Abs(i - (int)(boardX / 2)) - (boardY - 1)))
                {
                    if(i%2!=j%2)
                        //여기다가 생성 로직을 짜면 된다.
                        stages[i,j] = true;
                }
            }
        }
    }
        
    // Update is called once per frame
    void Update()
    {        
    }
}
