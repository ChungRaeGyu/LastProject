using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBoardScene_2 : MonoBehaviour
{
    public GameObject[] stageInfo = new GameObject[16];
    public GameObject[] stageBtn = new GameObject[16];
    public GameObject[] clearStage = new GameObject[16];

    public GameObject clear;
    public GameObject playerCharacter;

    public Vector3 player;
    public Vector3 position;
    public int stageLevel;


    public void Start()
    {

        for (int i = 0; i < stageBtn.Length; i++)
        {
            stageInfo[i].SetActive(true);
            stageBtn[i].SetActive(false);
            clearStage[i].SetActive(false);
        }
    }

    public void Update()
    {
        stageLevel = GameManager_chan.Instance.stageLevel;
        position = GameManager_chan.Instance.nowPosition;
        player = playerCharacter.transform.position;

        if (stageLevel == 1)
        {
            player = new Vector3(-750, 100, 0);
            stageBtn[0].SetActive(true);
        }
        if (stageLevel == 2)
        {
            player = position;
            stageBtn[1].SetActive(true);
            stageBtn[2].SetActive(true);
            
        }
        if (stageLevel == 3)
        {
            player = position;
            clearStage[0].SetActive(true);
            if (player == stageInfo[1].transform.position)
            {
                stageBtn[3].SetActive(true);
                stageBtn[4].SetActive(true);
            }
            if(player == stageInfo[2].transform.position)
            {
                stageBtn[4].SetActive(true);
                stageBtn[5].SetActive(true);
            }
        }
        if (stageLevel == 4)
        {
            player = position;
            if (player == stageInfo[3].transform.position)
            {
                stageBtn[6].SetActive(true);
                stageBtn[7].SetActive(true);
            }
            if (player == stageInfo[4].transform.position)
            {
                stageBtn[7].SetActive(true);
                stageBtn[8].SetActive(true);
            }
            if (player == stageInfo[1].transform.position)
            {
                stageBtn[8].SetActive(true);
                stageBtn[9].SetActive(true);
            }
        }
        if (stageLevel == 5)
        {
            player = position;
            if (player == stageInfo[6].transform.position)
            {
                stageBtn[10].SetActive(true);
            }
            if (player == stageInfo[1].transform.position)
            {
                stageBtn[10].SetActive(true);
                stageBtn[11].SetActive(true);
            }
            if (player == stageInfo[1].transform.position)
            {
                stageBtn[11].SetActive(true);
                stageBtn[12].SetActive(true);
            }
            if (player == stageInfo[1].transform.position)
            {
                stageBtn[12].SetActive(true);
            }
        }
        if (stageLevel == 6)
        {
            player = position;
            if (player == stageInfo[10].transform.position)
            {
                stageBtn[13].SetActive(true);
            }
            if (player == stageInfo[11].transform.position)
            {
                stageBtn[13].SetActive(true);
                stageBtn[14].SetActive(true);
            }
            if (player == stageInfo[12].transform.position)
            {
                stageBtn[14].SetActive(true);
            }

        }
        if (stageLevel == 7)
        {
            player = position;
            stageBtn[15].SetActive(true);
        }
    }
}
