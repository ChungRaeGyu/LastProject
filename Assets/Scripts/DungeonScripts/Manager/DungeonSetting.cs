using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSetting : MonoBehaviour
{
    public GameObject enterBtn;
    public GameObject lockDungeon;
    public GameObject explain;
    public GameObject[] dungeon;
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
            enterBtn.SetActive(false);
            lockDungeon.SetActive(false);
            explain.SetActive(false);
        }
        else if (gameObject.name == "05_Dungeon" && DataManager.Instance.accessibleDungeon[4] == true)
        {
            enterBtn.SetActive(false);
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

    //던전으로 들어가는 버튼
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

        // 스탯 초기화 메서드 호출 예정
        DataManager.Instance.maxHealth = DungeonManager.Instance.Player.playerStats.maxhealth;
        DataManager.Instance.currenthealth = DungeonManager.Instance.Player.playerStats.maxhealth;
        DungeonManager.Instance.currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";

        // 기록 초기화 메서드
        DataManager.Instance.ResetRecord();
        SaveManager.Instance.StartTrackingTime();

        // 코인 UI 업데이트
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        // 제거한 카드 수 초기화
        DataManager.Instance.removeCardCount = 0;

        foreach (var card in DataManager.Instance.LobbyDeck)
        {
            DataManager.Instance.AddCard(card);
        }
        switch (gameObject.name)
        {
            case "01_Start_Dungeon":
                dungeon[0].GetComponent<Dungeon>().SetStage();
                DungeonManager.Instance.dungeonNum[0].SetActive(true);
                DataManager.Instance.accessDungeonNum = 0;
                break;

            case "02_Dungeon":
                dungeon[1].GetComponent<Dungeon>().SetStage();
                DungeonManager.Instance.dungeonNum[1].SetActive(true);
                DataManager.Instance.accessDungeonNum = 1;
                break;

            case "03_Dungeon":
                dungeon[2].GetComponent<Dungeon>().SetStage();
                DungeonManager.Instance.dungeonNum[2].SetActive(true);
                DataManager.Instance.accessDungeonNum = 2;
                break;

            case "04_Dungeon":
                dungeon[3].GetComponent<Dungeon>().SetStage();
                DungeonManager.Instance.dungeonNum[3].SetActive(true);
                DataManager.Instance.accessDungeonNum = 3;
                break;

            case "05_Dungeon":
                dungeon[4].GetComponent<Dungeon>().SetStage();
                DungeonManager.Instance.dungeonNum[4].SetActive(true);
                DataManager.Instance.accessDungeonNum = 4;
                break;
        }
        DataManager.Instance.initnum[0] = 3;
        DataManager.Instance.initnum[1] = 0;
    }
}
