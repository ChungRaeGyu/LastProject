using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtn : MonoBehaviour
{
    public GameObject pause;

    //�Ͻ����� ��ư Ȱ��ȭ
    public void OnBtn()
    {
        Time.timeScale = 0.0f;
        pause.SetActive(true);
    }

    //�Ͻ������г� �ݱ�
    public void OffBtn()
    {
        Time.timeScale = 1.0f;
        pause.SetActive(false);
    }

    public void GoDungeonBoard()
    {
        Time.timeScale = 1.0f;
        pause.SetActive(false);
        SaveManager.Instance.accessDungeon = false;
        DungeonManager.Instance.dungeonBoard.SetActive(true);
        DungeonManager.Instance.dungeon.SetActive(false);
    }
}
