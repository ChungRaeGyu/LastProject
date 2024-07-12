using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public GameObject battle;
    public GameObject eventStage;
    public GameObject store;

    public string stageName;
    public Vector3 stagePosition;

    public void Start()
    {
        int randomNum = Random.Range(0, 100);
        stagePosition = this.gameObject.transform.position;
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
        {
            stageName = "battle";
            battle.SetActive(true);
            eventStage.SetActive(false);
            store.SetActive(false);
        }
    }

    public void BattleBtn()
    {
        SaveManager.Instance.playerPosition = stagePosition;
        //DungeonManager.Instance.battleScene.SetActive(true);
        SceneManager.LoadScene(3);
    }

    public void EventBtn()
    {
        SaveManager.Instance.playerPosition = stagePosition;
        DungeonManager.Instance.eventScene.SetActive(true);
    }

    public void StoreBtn()
    {
        SaveManager.Instance.playerPosition = stagePosition;
        DungeonManager.Instance.storeScene.SetActive(true);
    }
}
