using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] stage = new GameObject[6];
    public GameObject[] stageBtn = new GameObject[6];
    public GameObject clear;
    public GameObject player;
    public GameObject background;
    public GameObject eventScene;

    public Vector3 position;
    public int stageLevel;


    public void Start()
    {

        for (int i =0; i<stageBtn.Length; i++)
        {
            stageBtn[i].SetActive(false);
        }
    }

    public void Update()
    {
        stageLevel = GameManager_chan.Instance.stageLevel;
        PositionSetting();

        if (stageLevel == 1)
        {
            player.transform.position = new Vector3(-6, 1, 0);
            stageBtn[0].SetActive(true);
        }
        if (stageLevel == 2)
        {
            player.transform.position = new Vector3(-6, 1, 0);
            stageBtn[1].SetActive(true);
            stageBtn[2].SetActive(true);
        }
        if (stageLevel == 3)
        {
            player.transform.position = new Vector3(position.x, position.y, 0);
            stageBtn[3].SetActive(true);
            stageBtn[4].SetActive(true);
        }
        if (stageLevel == 4)
        {
            player.transform.position = new Vector3(position.x, position.y, 0);
            stageBtn[5].SetActive(true);
        }
        if (stageLevel == 5)
        {
            player.transform.position = new Vector3(6, 1, 0);
            clear.SetActive(true);
        }
    }

    private void PositionSetting()
    {
        position = GameManager_chan.Instance.nowPosition;

        if (position.x == 745)
            position.x = -2;
        if (position.x == 1175)
            position.x = 2;
        if (position.y == 755)
            position.y = 3;
        if (position.y == 325)
            position.y = -1;
    }

    public void Event()
    {
        player.SetActive(false);
        background.SetActive(false);
        for (int i = 0; i < stage.Length; i++)
        {
            stage[i].SetActive(false);
            stageBtn[i].SetActive(false);
        }
        eventScene.SetActive(true);
    }

    public void EventEnd()
    {
        
        eventScene.SetActive(false);
        player.SetActive(true);
        background.SetActive(true);
        for (int i = 0; i < stage.Length; i++)
        {
            stage[i].SetActive(true);
        }
        if (stageLevel == 2)
        {
            player.transform.position = new Vector3(position.x, position.y, 0);
            stageBtn[1].SetActive(false);
            stageBtn[2].SetActive(false);
            stageBtn[3].SetActive(true);
            stageBtn[4].SetActive(true);
        }
        if (stageLevel == 3)
        {
            player.transform.position = new Vector3(position.x, position.y, 0);
            stageBtn[3].SetActive(false);
            stageBtn[4].SetActive(false);
            stageBtn[5].SetActive(true);
        }
        GameManager_chan.Instance.stageLevel += 1;
    }
}
