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
        if (gameObject.name == "02_Dungeon" && DataManager.Instance.accessibleDungeon[1] == true)
        {

            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else if (gameObject.name == "03_Dungeon" && DataManager.Instance.accessibleDungeon[2] == true)
        {
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else if (gameObject.name == "04_Dungeon" && DataManager.Instance.accessibleDungeon[3] == true)
        {
            enterBtn.SetActive(true);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else if (gameObject.name == "05_Dungeon" && DataManager.Instance.accessibleDungeon[4] == true)
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

        // ������ ī�� �� �ʱ�ȭ
        DataManager.Instance.removeCardCount = 0;

        foreach (var card in DataManager.Instance.LobbyDeck)
        {
            DataManager.Instance.AddCard(card);
        }
        switch (gameObject.name)
        {
            case "01_Start_Dungeon":
                DungeonManager.Instance.dungeonNum[0].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                DataManager.Instance.accessDungeonNum = 0;
                break;

            case "02_Dungeon":
                DungeonManager.Instance.dungeonNum[1].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                DataManager.Instance.accessDungeonNum = 1;
                break;

            case "03_Dungeon":
                DungeonManager.Instance.dungeonNum[2].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                DataManager.Instance.accessDungeonNum = 2;
                break;

            case "04_Dungeon":
                DungeonManager.Instance.dungeonNum[3].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                DataManager.Instance.accessDungeonNum = 3;
                break;

            case "05_Dungeon":
                DungeonManager.Instance.dungeonNum[4].SetActive(true);
                SaveManager.Instance.RandomStageNum();
                DataManager.Instance.accessDungeonNum = 4;
                break;
        }
        DataManager.Instance.initnum[0] = 3;
        DataManager.Instance.initnum[1] = 0;
    }
}
