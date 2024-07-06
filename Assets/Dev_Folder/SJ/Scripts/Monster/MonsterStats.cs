using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterStats", menuName = "Monster/Stats", order = 0)]
public class MonsterStats : ScriptableObject
{
    [Header("Base Stats")]
    public string monsterName;
    public int maxhealth;
    public int attackPower;
    public int defense;
}
