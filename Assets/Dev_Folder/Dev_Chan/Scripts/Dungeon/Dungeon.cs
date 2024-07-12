using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{

    public GameObject[] stageNum;
    public GameObject[] stageBtn;

    public int num;

    void Start()
    {
        for(int i = 0; i<stageNum.Length; i++)
        {
            stageNum[i].SetActive(true);
        }
        
        DungeonManager.Instance.playerPosition = stageNum[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stageBtn.Length; i++)
        {
            stageBtn[i].SetActive(false);
        }
    }

    
}
