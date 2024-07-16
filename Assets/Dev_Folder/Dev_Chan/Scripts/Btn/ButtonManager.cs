using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    //던전으로 들어가는 버튼
    public void GoToDungeon()
    {
        DungeonBoardManager.Instance.dungeonBoard.SetActive(false);
        DungeonBoardManager.Instance.dungeon.SetActive(true);
        SaveManager.Instance.accessDungeon = true;
        switch (gameObject.name)
        {
            case "01_Start_Dungeon":
                for (int i = 0; i < 5; i++)
                {
                    if (i == 0)
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(true);
                    }
                    else
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(false);
                    }
                }
                SaveManager.Instance.accessDungeonNum = 1;
                // 플레이어 체력 초기화 (임시)
                DataManager.Instance.ResetPlayerHealth();
                // 몬스터 킬 수 초기화 (임시)
                DataManager.Instance.ResetMonstersKilledCount();
                SaveManager.Instance.playerPosition = Dungeon.Instance.stageNum[0].transform.position;
                Debug.Log("1번째 던전에 입장하셨습니다.");
                break;

            case "02_Dungeon":
                for (int i = 0; i < 5; i++)
                {
                    if (i == 1)
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(true);
                    }
                    else
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(false);
                    }
                }
                SaveManager.Instance.accessDungeonNum = 2;
                Debug.Log("2번째 던전에 입장하셨습니다.");
                break;

            case "03_Dungeon":
                for (int i = 0; i < 5; i++)
                {
                    if (i == 2)
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(true);
                    }
                    else
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(false);
                    }
                }
                SaveManager.Instance.accessDungeonNum = 3;
                Debug.Log("3번째 던전에 입장하셨습니다.");
                break;

            case "04_Dungeon":
                for (int i = 0; i < 5; i++)
                {
                    if (i == 3)
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(true);
                    }
                    else
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(false);
                    }
                }
                SaveManager.Instance.accessDungeonNum = 4;
                Debug.Log("4번째 던전에 입장하셨습니다.");
                break;

            case "05_Dungeon":
                for (int i = 0; i < 5; i++)
                {
                    if (i == 4)
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(true);
                    }
                    else
                    {
                        DungeonManager.Instance.dungeonNum[i].SetActive(false);
                    }
                }
                SaveManager.Instance.accessDungeonNum = 5;
                Debug.Log("5번째 던전에 입장하셨습니다.");
                break;
        }
    }

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

    // 보스로 들어가는 버튼
    public void Boss()
    {
        //DungeonManager.Instance.bossScene.SetActive(true);
    }

    //스테이지 클리어하면 스테이지 보드로 돌아가는 버튼
    public void StageClear()
    {
        DungeonManager.Instance.battleScene.SetActive(false);
        DungeonManager.Instance.eventScene.SetActive(false);
        DungeonManager.Instance.storeScene.SetActive(false);
        DungeonManager.Instance.bossScene.SetActive(false);
    }
    public void BattleClear()
    {

        SceneManager.LoadScene(2);
    }
}
