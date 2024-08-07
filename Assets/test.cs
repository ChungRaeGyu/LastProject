using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    string path;
    private void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
    }
    public void Save()
    {
        SaveData saveData = new SaveData();
        saveData.activeScenebuildindex = SceneManager.GetActiveScene().buildIndex;
        saveData.accessibleDungeonNum = DataManager.Instance.accessDungeonNum;
        saveData.cardObjs = DataManager.Instance.cardObjs;
        saveData.LobbyDeck= DataManager.Instance.LobbyDeck;
        saveData.currentCrystal = DataManager.Instance.currentCrystal;
        saveData.currentHealth = DataManager.Instance.currenthealth;
        saveData.maxHealth = DataManager.Instance.maxHealth;
        //saveData.dataManager = DataManager.Instance;
        //PlayerCharacter
        if (saveData.activeScenebuildindex == 3)
        {
            saveData.currenthealth = GameManager.instance.player.currenthealth;
            saveData.currentAttack = GameManager.instance.player.currentAttack;
            saveData.currentDefense = GameManager.instance.player.currentDefense;
            saveData.defdown = GameManager.instance.player.defdown;
            saveData.playerStat = GameManager.instance.player.playerStats;
        }
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
        Debug.Log("저장");
    }

    public void Load()
    {
        SaveData saveData = new SaveData();
        string loadJson = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        SceneManager.LoadScene(saveData.activeScenebuildindex);
        //Todo: 로딩 씬 넣기
        DataManager.Instance.accessDungeonNum = saveData.accessibleDungeonNum;
        DataManager.Instance.cardObjs = saveData.cardObjs;
        DataManager.Instance.LobbyDeck= saveData.LobbyDeck;
        DataManager.Instance.currentCrystal = saveData.currentCrystal;
        DataManager.Instance.currenthealth = saveData.currentHealth;
        DataManager.Instance.maxHealth = saveData.maxHealth;

        if (saveData.activeScenebuildindex == 3)
        {
            GameManager.instance.player.currenthealth=saveData.currenthealth;
            GameManager.instance.player.currentAttack=saveData.currentAttack;
            GameManager.instance.player.currentDefense=saveData.currentDefense;
            GameManager.instance.player.defdown = saveData.defdown;
            GameManager.instance.player.playerStats = saveData.playerStat;
            //Todo : 상태이상 넣기
        }
        Debug.Log("로드완료");
    }
}
