using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    //����
    public int openDungeonNum;
    public int[] currentCount = new int[DataManager.Instance.cardObjs.Count];
    public bool[] isFind = new bool[DataManager.Instance.cardObjs.Count];
    public int[] enhance = new int[DataManager.Instance.cardObjs.Count];
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //�κ񿡼��� �� ����Ʈ
    public int currentCrystal;// ũ����Ż 
    // - ���� ����. PlayerCharacter
    //���� ������ ���� �ʿ��� �͵�


    //public DataManager dataManager;
}