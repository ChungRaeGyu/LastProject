using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterStats", menuName = "Monster/Stats", order = 0)]
public class MonsterInfo : ScriptableObject
{
    [Header("Base Stats")]
    public string name;
    public int maxhealth;
    public int attackPower;
    public int defense;
}
