using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleBtn : MonoBehaviour
{
    private void OnMouseDown()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        if (gameObject.name == "BossBtn")
        {
            SaveManager.Instance.isBossStage = true;
        }
        SaveManager.Instance.playerPosition = this.gameObject.transform.position;
        //DungeonManager.Instance.battleScene.SetActive(true);
        Dungeon.Instance.MonsterSpawn();
        DataManager.Instance.SuffleDeckList();
        SceneManager.LoadScene(3);
        Debug.Log("전투를 개시합니다.");
    }
}
