using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event : Stage
{
    private void OnMouseDown()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = stagePosition;
        DungeonManager.Instance.eventScene.SetActive(true);
    }
}
