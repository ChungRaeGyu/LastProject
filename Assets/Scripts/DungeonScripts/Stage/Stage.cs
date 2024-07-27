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
    public List<GameObject> monsters = new List<GameObject>();
    public void Start()
    {
        stagePosition = this.gameObject.transform.position;

        if (monsters.Count == 1) return;
        //stagePosition = this.gameObject.transform.position;
        
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
        stageName = "battle";
        battle.SetActive(true);
        eventStage.SetActive(false);
        store.SetActive(false);
    }

    public void BattleBtn()
    {
        
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        if (gameObject.name == "BossStage")
        {
            SaveManager.Instance.isBossStage = true;
        }
        SceneManager.LoadScene(3);
        SaveManager.Instance.playerPosition = stagePosition;
        //DungeonManager.Instance.battleScene.SetActive(true);
        DataManager.Instance.Monsters = monsters;
        DataManager.Instance.SuffleDeckList();
    }

    public void EventBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;
        DungeonManager.Instance.eventScene.SetActive(true);
    }

    public void StoreBtn()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;
        DungeonManager.Instance.storeScene.SetActive(true);
    }
}
