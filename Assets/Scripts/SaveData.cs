using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    //����
    public int openDungeonNum;
    public int[] currentCount = new int[DataManager.Instance.cardObjs.Count];
    public bool[] isFind = new bool[DataManager.Instance.cardObjs.Count];
    public int[] enhance = new int[DataManager.Instance.cardObjs.Count];
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //�κ񿡼��� �� ����Ʈ
    public List<CardBasic> cardObj = new List<CardBasic>();
    public int currentCrystal;// ũ����Ż 
    public int[] cardPiece = new int[(int)Rate.Count];
    // - ���� ����. PlayerCharacter
    //���� ������ ���� �ʿ��� �͵�


    
}