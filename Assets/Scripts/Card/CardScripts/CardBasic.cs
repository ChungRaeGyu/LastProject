using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    Legend,
    Count
}
public class CardBasic : MonoBehaviour
{
    public CardBasic cardBasic;

    [Header("BasicData")]
    public string cardName;
    //public string description;
    public int cost;
    public int damageAbility;
    public int utilAbility;
    public int currentCount;
    public JOB job;
    public Rate rate;
    public bool dragLineCard;
    public GameObject deckCardImage;
    public Sprite image;
    public Sprite firstEnhanceImage;
    public Sprite secondEnhanceImage;

    [Header("FindCheck")]
    public bool isFind = false;

    [Header("EffectPrefab")]
    public GameObject playerEffect;
    public GameObject attackEffect = null;
    public GameObject debuffEffectPrefab;

    [Header("UI Components")]
    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text descriptionText;

    [Header("UI Components")]
    public AudioSource audioSource;

    [Header("AudioClip")]
    public AudioClip CardClip1;
    public AudioClip CardClip2;

    // 초기 ability 값 저장
    protected int initialDamageAbility;
    protected int initialUtilAbility;
    protected int initialCost;

    // 강화 단계
    public int enhancementLevel = 0;

    // 카드 사용 완료
    public bool playCardCompleted;

    public virtual void CardUse(Monster targetMonster, Player player)
    {
       
    }

    public IEnumerator PlayCard()
    {
        playCardCompleted = false; // 초기화
        yield return StartCoroutine(TryUseCard());
        playCardCompleted = true; // 완료 설정
    }

    public virtual IEnumerator TryUseCard()
    {
        yield return null;
    }

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 초기 ability 값 저장
        initialDamageAbility = damageAbility;
        initialUtilAbility = utilAbility;
        initialCost = cost;

        ApplyEnhancements();

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
    public virtual void SetDescription()
    {
        if (costText != null)
        {
            costText.text = cost.ToString();
        }

        if (enhancementLevel > 0)
        {
            nameText.text = $"{cardName} +{enhancementLevel}";
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && audioSource.enabled)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // 강화 메서드
    public void EnhanceCard()
    {
        enhancementLevel++;
    }

    // 강화 단계에 따른 능력치 적용
    public virtual void ApplyEnhancements()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) return;

        Image childImage = transform.GetChild(1).GetComponent<Image>();

        switch (enhancementLevel)
        {
            case 1:
                if (childImage != null) childImage.sprite = firstEnhanceImage;
                break;
            case 2:
                if (childImage != null) childImage.sprite = secondEnhanceImage;
                break;
            default:
                break;
        }
    }
}
