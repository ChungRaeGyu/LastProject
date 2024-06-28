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
    Fire, //지속딜
    Ice, //빙결 게이지가 상승하지 않는다.
    None
}
[CreateAssetMenu(fileName = "Card", menuName = "newCard", order = 0)]
public class CardSO : ScriptableObject {
    public string cardName;
    public string description;
    public float ability;
    public Kind kind;
    public JOB job;
    public special_ability special_ability;
    public Sprite Image;
}
