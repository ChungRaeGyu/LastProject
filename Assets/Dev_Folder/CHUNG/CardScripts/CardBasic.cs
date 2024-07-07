using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum JOB
{
    Normal,
    Warrior,
    Archor,
    Magician
}
public enum Kind
{
    Attack,
    MagicAttack,
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
public enum Rate
{
    Normal,
    Rarity,
    Hero,
    Count
}
public class CardBasic : MonoBehaviour
{
    [Header("BasicData")]
    public string cardName;
    public string description;
    public int cost;
    public int ability;
    public int currentCount;
    public Sprite image;
    public Sprite defaultImage; //µÞ¸í
    public JOB job;
    public Rate rate;
    public GameObject effect;
    public GameObject attackEffect;
    public virtual void CardUse(Monster targetMonster, Player player)
    {
        Debug.Log("CardBasic");
    }
    public virtual void TryUseCard()
    {

    }
}
