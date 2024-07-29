using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_01 : Dungeon
{
    void Awake()
    {
        isStage = new int[,] { { 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0 },
                               { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 },
                               { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                               { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
                               { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 },
                               { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 },
                               { 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0 } };

        x = 7;
        y = 15;
    }

    void Update()
    {
        
    }
}
