using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtn : MonoBehaviour
{
    public GameObject pause;

    //일시정지 버튼 활성화
    public void OnBtn()
    {
        Time.timeScale = 0.0f;
        pause.SetActive(true);
    }

    //일시정지패널 닫기
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
