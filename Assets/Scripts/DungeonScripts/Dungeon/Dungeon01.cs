using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon01 : Dungeon
{
    [Header("NormalMob")]
    public List<GameObject> mob1 = new List<GameObject>();


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
