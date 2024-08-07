using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int activeScenebuildindex; //현재 씬 번호

    //공통
    public PlayerStats playerStat;
    public int accessibleDungeonNum;
    public List<CardBasic> cardObjs = new List<CardBasic>();
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //로비에서의 덱 리스트
    public int currentCrystal;// 크리스탈 
    public int maxHealth;
    public int currentHealth;
    // - 현재 스탯. PlayerCharacter
    //게임 씬에서 끌때 필요한 것들
    public int currenthealth=0;
    public int currentDefense=0;
    public int currentAttack=0;
    public float defdown;


    //public DataManager dataManager;이거는 됌
}
