using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_CH : MonoBehaviour
{
    //x´Â È¦¼ö
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
        for (int i = 0; i < boardX; i++)
        {
            for (int j = 0; j < boardY; j++)
            {
                if(Mathf.Abs(i-(int)(boardX / 2))<j&&boardY-M)
            }
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
