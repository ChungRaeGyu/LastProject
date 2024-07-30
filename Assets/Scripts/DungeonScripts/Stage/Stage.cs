using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public GameObject start;
    public GameObject battle;
    public GameObject warp;
    public GameObject eventStage;
    public GameObject store;
    public GameObject eliteMob;
    public GameObject boss;

    public Vector3 stagePosition;
    //public List<GameObject> monsters = new List<GameObject>();

    public void Start()
    {
        stagePosition = this.gameObject.transform.position;


        //위치에 따라 각종 스테이지 개방
        if (stagePosition == Dungeon.Instance.stage[((Dungeon.Instance.x - 1) / 2), 0].transform.position)
        {
            start.SetActive(true);
        }
        else if (stagePosition == Dungeon.Instance.stage[0, ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            warp.SetActive(true);
        }
        else if (stagePosition == Dungeon.Instance.stage[(Dungeon.Instance.x - 1), ((Dungeon.Instance.y - 1) / 2)].transform.position)
        {
            warp.SetActive(true);
        }
        else if (stagePosition == Dungeon.Instance.stage[((Dungeon.Instance.x - 1) / 2), (Dungeon.Instance.y - 1)].transform.position)
        {
            boss.SetActive(true);
        }
        else
        {
            battle.SetActive(true);
        }

        //이벤트와 스토어 스테이지 아직 코딩 못함
        /*int randomNum = Random.Range(0, 100);
        if (randomNum < 6)
        {
            stageName = "store";
            battle.SetActive(false);
            eventStage.SetActive(false);
            store.SetActive(true);
        }
        else if(randomNum >= 6 && randomNum <= 15)
        {
            stageName = "event";
            battle.SetActive(false);
            eventStage.SetActive(true);
            store.SetActive(false);
        }
        else
        {*/
    }
}
