using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_CH : MonoBehaviour
{
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
        for (int i = 0; i < stages.GetLength(0); i++)
        {
            for (int j = 0; j < stages.GetLength(1); j++)
            {
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
