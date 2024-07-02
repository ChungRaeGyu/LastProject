using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player/Stats", order = 0)]
public class PlayerStats : ScriptableObject
{
    [Header("Base Stats")]
    public int maxhealth;
    public int defense;
}