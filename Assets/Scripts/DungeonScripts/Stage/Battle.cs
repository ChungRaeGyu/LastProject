using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : Stage
{
    private void OnMouseDown()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        if (gameObject.name == "BossStage")
        {
            SaveManager.Instance.isBossStage = true;
        }
        SceneManager.LoadScene(3);
        SaveManager.Instance.playerPosition = stagePosition;
        //DungeonManager.Instance.battleScene.SetActive(true);
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        Debug.Log("������ �����մϴ�.");
    }
}
