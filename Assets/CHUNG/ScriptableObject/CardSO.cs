using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public enum JOB{
    Normal,
    Warrior,
    Archor,
    Magician
}
public enum Kind{
    Attack,
    RangeAttack,
    Heal,
}
public enum special_ability
{
    Fire, //지?�딜
    Ice, //빙결 게이지가 ?�승?��? ?�는??
    None
}
public enum Rate{
    Normal,
    Rarity,
    Hero
}
[CreateAssetMenu(fileName = "Card", menuName = "newCard", order = 0)]
public class CardSO : ScriptableObject {
    public string cardName;
    public string description;
    public int cost;
    public int ability;
    public Kind kind;
    public JOB job;
    public special_ability special_ability;
    public Sprite Image;
    public Rate rate;
    public int currentCount;
}
