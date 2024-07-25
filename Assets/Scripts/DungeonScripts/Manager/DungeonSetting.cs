using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSetting : MonoBehaviour
{
    public GameObject enterBtn;
    public GameObject lockDungeon;
    public GameObject explain;

    void Start()
    {           
        if (gameObject.name == "02_Dungeon" && SaveManager.Instance.accessibleDungeon[1] == true)
        {
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else if (gameObject.name == "03_Dungeon" && SaveManager.Instance.accessibleDungeon[2] == true)
        {
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else if (gameObject.name == "04_Dungeon" && SaveManager.Instance.accessibleDungeon[3] == true)
        {
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else if (gameObject.name == "05_Dungeon" && SaveManager.Instance.accessibleDungeon[4] == true)
        {
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else
        {
            enterBtn.SetActive(false);
            lockDungeon.SetActive(true);
            explain.SetActive(true);
        }

        if (gameObject.name == "01_Start_Dungeon")
        {
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
    }

    void Update()
    {
       
    }

    //던전으로 들어가는 버튼
    public void GoToDungeon()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);
        DungeonBoardManager.Instance.dungeonBoard.SetActive(false);
        DungeonBoardManager.Instance.dungeon.SetActive(true);
        SaveManager.Instance.accessDungeon = true;
        SettingManager.Instance.UpdateButtonVisibility();

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
                SaveManager.Instance.accessDungeonNum = 0;
                DataManager.Instance.ResetPlayerHealth(); // 플레이어 체력 초기화 (임시)
                DataManager.Instance.ResetMonstersKilledCount(); // 몬스터 킬 수 초기화 (임시)
                SaveManager.Instance.playerPosition = Dungeon01.Instance.stageNum[0].transform.position;
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
                SaveManager.Instance.accessDungeonNum = 1;
                DataManager.Instance.ResetPlayerHealth(); // 플레이어 체력 초기화 (임시)
                DataManager.Instance.ResetMonstersKilledCount(); // 몬스터 킬 수 초기화 (임시)
                SaveManager.Instance.playerPosition = Dungeon02.Instance.stageNum[0].transform.position;
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
                SaveManager.Instance.accessDungeonNum = 2;
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
                SaveManager.Instance.accessDungeonNum = 3;
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
                SaveManager.Instance.accessDungeonNum = 4;
                Debug.Log("5번째 던전에 입장하셨습니다.");
                break;
        }
    }
}
