using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBtn : MonoBehaviour
{
    private void OnMouseDown()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.BtnClip1);

        SaveManager.Instance.playerPosition = this.gameObject.transform.position;
        DungeonManager.Instance.eventScene.SetActive(true);
    }
}
