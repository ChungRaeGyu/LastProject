using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int activeScenebuildindex; //���� �� ��ȣ

    //����
    public PlayerStats playerStat;
    public int accessibleDungeonNum;
    public List<CardBasic> cardObjs = new List<CardBasic>();
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //�κ񿡼��� �� ����Ʈ
    public int currentCrystal;// ũ����Ż 
    public int maxHealth;
    public int currentHealth;
    // - ���� ����. PlayerCharacter
    //���� ������ ���� �ʿ��� �͵�
    public int currenthealth=0;
    public int currentDefense=0;
    public int currentAttack=0;
    public float defdown;


    //public DataManager dataManager;�̰Ŵ� ��
}
