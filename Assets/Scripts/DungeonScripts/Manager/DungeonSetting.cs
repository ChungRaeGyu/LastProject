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

    //�������� ���� ��ư
    public void GoToDungeon()
    {
        SettingManager.Instance.SFXAudioSource.PlayOneShot(SettingManager.Instance.CardSelect);
        DungeonManager.Instance.dungeonBoard.SetActive(false);
        DungeonManager.Instance.dungeon.SetActive(true);
        SaveManager.Instance.accessDungeon = true;
        SaveManager.Instance.isStartPoint = true;
        SettingManager.Instance.UpdateButtonVisibility();
        DungeonManager.Instance.DungeonCoin.SetActive(true);
        DungeonManager.Instance.DungeonHp.SetActive(true);

        // ���� �ʱ�ȭ �޼��� ȣ�� ����
        DataManager.Instance.maxHealth = DungeonManager.Instance.Player.playerStats.maxhealth;
        DataManager.Instance.currenthealth = DungeonManager.Instance.Player.playerStats.maxhealth;

        DungeonManager.Instance.currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";

        // ��� �ʱ�ȭ �޼���
        DataManager.Instance.ResetRecord();
        SaveManager.Instance.StartTrackingTime();

        foreach (var card in DataManager.Instance.LobbyDeck)
        {
            DataManager.Instance.AddCard(card);
        }
        switch (gameObject.name)
        {
            case "01_Start_Dungeon":
                DungeonManager.Instance.dungeonNum[0].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                SaveManager.Instance.accessDungeonNum = 0;
                DungeonManager.Instance.startPosition.transform.position += new Vector3(3, 0, 0);
                Debug.Log("1��° ������ �����ϼ̽��ϴ�.");
                break;

            case "02_Dungeon":
                DungeonManager.Instance.dungeonNum[1].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                SaveManager.Instance.accessDungeonNum = 1;
                Debug.Log("2��° ������ �����ϼ̽��ϴ�.");
                break;

            case "03_Dungeon":
                DungeonManager.Instance.dungeonNum[2].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                SaveManager.Instance.accessDungeonNum = 2;
                Debug.Log("3��° ������ �����ϼ̽��ϴ�.");
                break;

            case "04_Dungeon":
                DungeonManager.Instance.dungeonNum[3].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                SaveManager.Instance.accessDungeonNum = 3;
                Debug.Log("4��° ������ �����ϼ̽��ϴ�.");
                break;

            case "05_Dungeon":
                DungeonManager.Instance.dungeonNum[4].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                SaveManager.Instance.accessDungeonNum = 4;
                Debug.Log("5��° ������ �����ϼ̽��ϴ�.");
                break;
        }
    }
}
