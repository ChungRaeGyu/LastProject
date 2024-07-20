using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    [HideInInspector]
    public CardBasic cardBasic;

    [Header("BasicData")]
    public string cardName;
    //public string description;
    public int cost;
    public int damageAbility;
    public int utilAbility;
    public int currentCount;
    public Sprite image;
    public Sprite defaultImage;
    public JOB job;
    public Rate rate;
    public GameObject effect;
    public bool dragLineCard;
    public GameObject attackEffect = null;
    public GameObject deckCardImage;
    [Header("attackEffect_Image")]
    public GameObject debuffEffectPrefab;

    [Header("UI Components")]
    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected TMP_Text costText;
    [SerializeField] protected TMP_Text descriptionText;

    // 초기 ability 값 저장
    protected int initialDamageAbility;
    protected int initialUtilAbility;

    public virtual void CardUse(Monster targetMonster, Player player)
    {
        Debug.Log("CardBasic");
    }
    public virtual bool TryUseCard()
    {
        return false;
    }

    protected virtual void Start()
    {
        // 초기 ability 값 저장
        initialDamageAbility = damageAbility;
        initialUtilAbility = utilAbility;

        if (costText != null)
        {
            costText.text = cost.ToString();
        }
        if (nameText != null)
        {
            nameText.text = cardName;
        }
    }

    // 설명을 설정하는 메서드
    protected virtual void SetDescription()
    {

    }
}
