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
    AddCard,
    AddCost
}
public enum special_ability
{
    Fire,
    Ice,
    None
}
public enum Rate{
    Normal,
    Rarity,
    Hero,
    Count
}
[CreateAssetMenu(fileName = "Card", menuName = "newCard/AttackCard", order = 0)]
public class CardSO : ScriptableObject {
    public string cardName;
    public string description;
    public int cost;
    public int ability;
    
    public Sprite Image;
    public Sprite defaultImage;
    public Rate rate;
    public int currentCount;
    public JOB job;
    public GameObject effect;

    public GameObject attackEffect;
    public Kind kind;
    public special_ability special_ability;
}
