using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    //�������� ���� ��ư
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
                // �÷��̾� ü�� �ʱ�ȭ (�ӽ�)
                DataManager.Instance.ResetPlayerHealth();
                // ���� ų �� �ʱ�ȭ (�ӽ�)
                DataManager.Instance.ResetMonstersKilledCount();
                SaveManager.Instance.playerPosition = Dungeon.Instance.stageNum[0].transform.position;
                Debug.Log("1��° ������ �����ϼ̽��ϴ�.");
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
                Debug.Log("2��° ������ �����ϼ̽��ϴ�.");
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
                Debug.Log("3��° ������ �����ϼ̽��ϴ�.");
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
                Debug.Log("4��° ������ �����ϼ̽��ϴ�.");
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
                Debug.Log("5��° ������ �����ϼ̽��ϴ�.");
                break;
        }
    }

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

    // ������ ���� ��ư
    public void Boss()
    {
        //DungeonManager.Instance.bossScene.SetActive(true);
    }

    //�������� Ŭ�����ϸ� �������� ����� ���ư��� ��ư
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
