using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : Stage
{
    private void OnMouseDown()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        if (gameObject.name == "Boss")
        {
            SaveManager.Instance.isBossStage = true;
        }
        SceneFader.instance.LoadSceneWithFade(3);
        SaveManager.Instance.playerPosition = stagePosition;
        //DungeonManager.Instance.battleScene.SetActive(true);
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        Debug.Log("전투를 개시합니다.");
    }
}
