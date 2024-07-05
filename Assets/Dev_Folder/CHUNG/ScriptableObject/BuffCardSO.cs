using UnityEngine;

[CreateAssetMenu(fileName = "BuffCardSO", menuName = "newCard/BuffCardSO", order = 1)]
public class BuffCardSO : ScriptableObject
{
    public string cardName;
    public string description;
    public int cost;
    public int ability;
    public GameObject effect;
    public Sprite Image;
    public Sprite defaultImage;
    public Rate rate;
    public int currentCount;
    public JOB job;
}
