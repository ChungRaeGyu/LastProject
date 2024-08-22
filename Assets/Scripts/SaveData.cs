using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    //공통
    public int openDungeonNum;
    public int[] currentCount = new int[DataManager.Instance.cardObjs.Count];
    public bool[] isFind = new bool[DataManager.Instance.cardObjs.Count];
    public int[] enhance = new int[DataManager.Instance.cardObjs.Count];
    public List<CardBasic> LobbyDeck = new List<CardBasic>(); //로비에서의 덱 리스트
    public List<CardBasic> cardObj = new List<CardBasic>();
    public int currentCrystal;// 크리스탈 
    public int[] cardPiece = new int[(int)Rate.Count];
    // - 현재 스탯. PlayerCharacter
    //게임 씬에서 끌때 필요한 것들


    
}