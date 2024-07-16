using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    // 던전 보드로 돌아가는 버튼
    public void BoardBtn()
    {
        SaveManager.Instance.accessDungeon = false;
        DungeonBoardManager.Instance.dungeonBoard.SetActive(true);
        DungeonBoardManager.Instance.dungeon.SetActive(false);
    }

    //로비로 돌아가는 버튼
    public void HomeBtn()
    {
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Lobby");
    }

    //스테이지 클리어하면 스테이지 보드로 돌아가는 버튼
    public void StageClear()
    {
        DungeonManager.Instance.battleScene.SetActive(false);
        DungeonManager.Instance.eventScene.SetActive(false);
        DungeonManager.Instance.storeScene.SetActive(false);
        DungeonManager.Instance.bossScene.SetActive(false);
    }
}
