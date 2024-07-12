using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    //�������� ���� ��ư
    public void GoToDungeon()
    {
        DungeonManager.Instance.dungeonBoard.SetActive(false);
        DungeonManager.Instance.dungeon.SetActive(true);
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

    // ���� ����� ���ư��� ��ư
    public void BoardBtn()
    {
        DungeonManager.Instance.dungeonBoard.SetActive(true);
        DungeonManager.Instance.dungeon.SetActive(false);
    }

    //�κ�� ���ư��� ��ư
    public void HomeBtn()
    {
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Lobby");
    }

    // ������ ���� ��ư
    public void Boss()
    {
        DungeonManager.Instance.bossScene.SetActive(true);
    }

    //�������� Ŭ�����ϸ� �������� ����� ���ư��� ��ư
    public void StageClear()
    {
        DungeonManager.Instance.battleScene.SetActive(false);
        DungeonManager.Instance.eventScene.SetActive(false);
        DungeonManager.Instance.storeScene.SetActive(false);
        DungeonManager.Instance.bossScene.SetActive(false);
    }

    //���� Ŭ���� �� �κ�� ���� ��ư
    public void BossClear()
    {
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Lobby");
    }
}
