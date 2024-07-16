using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    // ���� ����� ���ư��� ��ư
    public void BoardBtn()
    {
        SaveManager.Instance.accessDungeon = false;
        DungeonBoardManager.Instance.dungeonBoard.SetActive(true);
        DungeonBoardManager.Instance.dungeon.SetActive(false);
    }

    //�κ�� ���ư��� ��ư
    public void HomeBtn()
    {
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Lobby");
    }

    //�������� Ŭ�����ϸ� �������� ����� ���ư��� ��ư
    public void StageClear()
    {
        DungeonManager.Instance.battleScene.SetActive(false);
        DungeonManager.Instance.eventScene.SetActive(false);
        DungeonManager.Instance.storeScene.SetActive(false);
        DungeonManager.Instance.bossScene.SetActive(false);
    }
}
