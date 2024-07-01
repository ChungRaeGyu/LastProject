using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character/Stats", order = 0)]
public class CharacterStats : ScriptableObject
{
    [Header("Base Stats")]
    public int maxhealth;
    public int defense;
}
