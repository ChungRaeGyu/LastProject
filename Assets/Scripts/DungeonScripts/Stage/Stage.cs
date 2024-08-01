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

    public System.Random random = new System.Random();

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
            for(int i = 0; i<Dungeon.Instance.x; i++)
            {
                for(int j = 0; j<Dungeon.Instance.y; j++)
                {
                    if(Dungeon.Instance.isStage[i, j] && stagePosition == Dungeon.Instance.stage[i, j].transform.position)
                    {
                        switch (SaveManager.Instance.num[i, j])
                        {
                            case 0:
                                eliteMob.SetActive(true);
                                break;
                            case 1:
                            case 2:
                                store.SetActive(true);
                                break;
                            case 3:
                            case 4:
                            case 5:
                                eventStage.SetActive(true);
                                break;
                            default:
                                battle.SetActive(true);
                                break;
                        }
                    }
                }
            }
        }
    }
}
