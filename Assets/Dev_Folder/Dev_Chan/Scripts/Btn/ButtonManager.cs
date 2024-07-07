using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void GoToDungeon()
    {
        DungeonBoardManager.Instance.dungeonBoard.SetActive(false);
        DungeonBoardManager.Instance.dungeon.SetActive(true);
        switch(gameObject.name)
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
                break;

            case "02_Slime_Dungeon":
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
                break;
        }
    }

    public void BoardBtn()
    {
        DungeonBoardManager.Instance.dungeonBoard.SetActive(true);
        DungeonBoardManager.Instance.dungeon.SetActive(false);
    }

    public void HomeBtn()
    {
        SceneManager.LoadScene(3);
    }

    public void BattleScene()
    {
        GameObject.Instantiate(StageManager.Instance.battleScene);
        //SceneManager.LoadScene(°ÔÀÓ¾À);
    }

    public void EventScene()
    {

    }

    public void Store()
    {

    }
}
