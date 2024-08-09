using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class EventManager : MonoBehaviour
{
    public GameObject Dungeon;

    [Header("FadeImage")]
    public Image fadeImage;

    [Header("Event")]
    public GameObject mimicEvent;
    public GameObject randomCardEvent;
    public GameObject healEvent;

    [Header("MimicEvent")]
    public List<GameObject> boxes;
    public List<GameObject> mimicMonster;

    [Header("RandomCardEvent")]
    public List<CardBasic> randomCardList;
    public TMP_Text randomCardEventDescription;
    public TMP_Text randomCardCoinText;
    public GameObject randomCardEventSelectBtn;
    public TMP_Text closeRandomCardEventText;
    public GameObject randomCardEventImage;

    [Header("HealEvent")]
    public TMP_Text healEventDescription;
    public TMP_Text healCoinText;
    public GameObject healEventSelectBtn;
    public TMP_Text closeHealEventText;
    public GameObject healEventImage;

    [Header("AudioClip")]
    public AudioClip CoinClip;

    // 랜덤 값 변수 돌려쓰기
    private int randomCoin;

    // 페이드 스피드
    private float fadeSpeed = 1f;

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void ShowRandomEvent()
    {
        // 랜덤 숫자 생성기 초기화
        int randomNumber = Random.Range(0, 3);

        // 랜덤 숫자에 따라 다른 Show 메서드 호출
        switch (randomNumber)
        {
            case 0:
                ShowMimicEvent();
                break;
            case 1:
                ShowRandomCardEvent();
                break;
            case 2:
                ShowHealEvent();
                break;
            default:
                break;
        }
    }

    public void ShowMimicEvent()
    {
        StartCoroutine(ShowMimicEventCoroutine());
    }

    private IEnumerator ShowMimicEventCoroutine()
    {
        fadeImage.gameObject.SetActive(true);

        // IncreaseAlpha가 완료될 때까지 기다림
        yield return StartCoroutine(IncreaseAlpha());

        HideDungeon();
        ShuffleBoxes();
        mimicEvent.SetActive(true);

        // DecreaseAlpha가 완료될 때까지 기다림
        yield return StartCoroutine(DecreaseAlpha());
    }

    private void ShuffleBoxes()
    {
        List<Transform> boxPositions = new List<Transform>();

        foreach (var box in boxes)
        {
            boxPositions.Add(box.transform);
        }

        for (int i = boxPositions.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // 상자의 위치를 바꿔준다
            Vector3 tempPosition = boxPositions[i].position;
            boxPositions[i].position = boxPositions[randomIndex].position;
            boxPositions[randomIndex].position = tempPosition;
        }
    }

    public void MimicSurprise()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        DataManager.Instance.SuffleDeckList();
        // 전투에서 미믹이 나와야 함
        DataManager.Instance.Monsters = mimicMonster;
        SceneFader.instance.LoadSceneWithFade(3);
    }

    public void GetCoin()
    {
        SettingManager.Instance.PlaySound(CoinClip);

        int randomCoin = Random.Range(30, 41);
        DataManager.Instance.currentCoin += randomCoin;
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        // 이 메서드가 호출된 버튼이 있는 오브젝트가 제거됨
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null)
        {
            StartCoroutine(FadeOutAndDestroy(clickedButton));
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject buttonObject)
    {
        Transform childTransform = buttonObject.transform.GetChild(0);
        Image childImage = childTransform.GetComponent<Image>();

        if (childImage != null)
        {
            Color originalColor = childImage.color;
            float duration = 0.5f; // 페이드 아웃 시간
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, elapsed / duration);
                childImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }

        Destroy(buttonObject);
    }

    public void HideMimicEvent()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip2);

        mimicEvent.SetActive(false);
        ShowDungeon();
        SceneFader.instance.LoadSceneWithFade(2);
    }

    public void ShowRandomCardEvent()
    {
        StartCoroutine(ShowRandomCardEventCoroutine());
    }

    private IEnumerator ShowRandomCardEventCoroutine()
    {
        fadeImage.gameObject.SetActive(true);

        yield return StartCoroutine(IncreaseAlpha());

        HideDungeon();
        randomCardEvent.SetActive(true);

        // 랜덤 코인 값 계산
        randomCoin = Random.Range(50, 61);

        // 현재 가진 코인을 체크하고 텍스트 색 결정
        bool insufficientCoins = DataManager.Instance.currentCoin < randomCoin;
        string mainTextColor = insufficientCoins ? "#808080" : "#FFFFFF";
        string coinTextColor = insufficientCoins ? "#808080" : "#FFFF00";
        string cardTextColor = insufficientCoins ? "#808080" : "#ADD8E6";

        // 텍스트 설정
        randomCardCoinText.text = $"<color={mainTextColor}>1. <color={coinTextColor}>{randomCoin}코인</color>을 지불하고 <color={cardTextColor}>랜덤한 카드</color>를 받는다.</color>";

        yield return StartCoroutine(DecreaseAlpha());
    }

    // randomCardList에서 카드를 1장 랜덤으로 내 덱에 추가
    public void GetRandomCard()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        if (DataManager.Instance.currentCoin < randomCoin)
        {
            Debug.Log("코인이 부족합니다.");
            return; // 코인이 부족할 때 아무 것도 하지 않고 메서드 종료
        }

        DataManager.Instance.currentCoin -= randomCoin;
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        int randomIndex = Random.Range(0, randomCardList.Count);
        CardBasic selectedCard = randomCardList[randomIndex];

        DataManager.Instance.deckList.Add(selectedCard);

        randomCardEventDescription.text = $"노인이 알 수 없는 표정을 지으며 카드를 건네줍니다. \n" +
            $"이 카드는 당신에게 도움이 될 것입니다. \n" +
            $"<color=#8A2BE2>{selectedCard.cardName}</color>카드를 받았습니다.";

        closeRandomCardEventText.text = "던전을 계속 진행한다.";

        randomCardEventSelectBtn.SetActive(false);
        randomCardEventImage.SetActive(false);
    }

    public void HideRandomCardEvent()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip2);

        randomCardEvent.SetActive(false);
        ShowDungeon();
        SceneFader.instance.LoadSceneWithFade(2);
    }

    public void ShowHealEvent()
    {
        StartCoroutine(ShowHealEventCoroutine());
    }

    private IEnumerator ShowHealEventCoroutine()
    {
        fadeImage.gameObject.SetActive(true);

        yield return StartCoroutine(IncreaseAlpha());

        HideDungeon();
        healEvent.SetActive(true);

        // 랜덤 코인 값 계산
        randomCoin = Random.Range(30, 41);

        // 현재 가진 코인을 체크하고 텍스트 색 결정
        bool insufficientCoins = DataManager.Instance.currentCoin < randomCoin;
        string mainTextColor = insufficientCoins ? "#808080" : "#FFFFFF";
        string coinTextColor = insufficientCoins ? "#808080" : "#FFFF00";
        string cardTextColor = insufficientCoins ? "#808080" : "#red";

        // 텍스트 설정
        healCoinText.text = $"<color={mainTextColor}>1. <color={coinTextColor}>{randomCoin}코인</color>을 지불하고 <color={cardTextColor}>체력 20%</color>를 회복한다.</color>";

        yield return StartCoroutine(DecreaseAlpha());
    }

    public void HealAndUseCoin()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        if (DataManager.Instance.currentCoin < randomCoin)
        {
            Debug.Log("코인이 부족합니다.");
            return; // 코인이 부족할 때 아무 것도 하지 않고 메서드 종료
        }

        DataManager.Instance.currentCoin -= randomCoin;
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        // 체력 회복 (최대 체력의 20%만큼)
        int healAmount = Mathf.CeilToInt(DataManager.Instance.maxHealth * 0.2f); // 20% 회복 (반올림)
        DataManager.Instance.currenthealth = Mathf.Min(DataManager.Instance.currenthealth + healAmount, DataManager.Instance.maxHealth); // 최대 체력을 넘지 않도록 설정

        DungeonManager.Instance.currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";
        healEventDescription.text = "약초상점의 주인이 한 손에 약초 병을 들고 자랑스럽게 말합니다. \n" +
            "이 약초는 자연의 정수를 담아 체력을 빠르게 회복시켜줍니다. \n" +
            "아주 신선하고 효과적인 레시피로 만들었죠!";

        closeHealEventText.text = "던전을 계속 진행한다.";

        healEventSelectBtn.SetActive(false);
        healEventImage.SetActive(false);
    }

    public void HideHealEvent()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip2);

        healEvent.SetActive(false);
        ShowDungeon();
        SceneFader.instance.LoadSceneWithFade(2);

        DecreaseAlpha();
    }

    // 던전패널 비활성화
    public void HideDungeon()
    {
        Dungeon.SetActive(false);
    }

    public void ShowDungeon()
    {
        Dungeon.SetActive(true);
    }

    // 알파값을 천천히 올리는 메서드
    public IEnumerator IncreaseAlpha()
    {
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (fadeImage.color.a < 1f)
        {
            Color color = fadeImage.color;
            color.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }
    }

    // 알파값을 천천히 줄이는 메서드
    public IEnumerator DecreaseAlpha()
    {
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (fadeImage.color.a > 0f)
        {
            Color color = fadeImage.color;
            color.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
