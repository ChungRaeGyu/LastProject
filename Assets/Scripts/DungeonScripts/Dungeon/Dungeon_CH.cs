using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_CH : MonoBehaviour
{
    bool[,] stages = new bool[5, 7];
    
    void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < stages.GetLength(0); i++)
        {
            for (int j = 0; j < stages.GetLength(1); j++)
            {
                if (i <= stages.GetLength(0) / 2)
                {

                }
                else
                {

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
