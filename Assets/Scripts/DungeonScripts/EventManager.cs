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

    // ���� �� ���� ��������
    private int randomCoin;

    // ���̵� ���ǵ�
    private float fadeSpeed = 1f;

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void ShowRandomEvent()
    {
        // ���� ���� ������ �ʱ�ȭ
        int randomNumber = Random.Range(0, 3);

        // ���� ���ڿ� ���� �ٸ� Show �޼��� ȣ��
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

        // IncreaseAlpha�� �Ϸ�� ������ ��ٸ�
        yield return StartCoroutine(IncreaseAlpha());

        HideDungeon();
        ShuffleBoxes();
        mimicEvent.SetActive(true);

        // DecreaseAlpha�� �Ϸ�� ������ ��ٸ�
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
            // ������ ��ġ�� �ٲ��ش�
            Vector3 tempPosition = boxPositions[i].position;
            boxPositions[i].position = boxPositions[randomIndex].position;
            boxPositions[randomIndex].position = tempPosition;
        }
    }

    public void MimicSurprise()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        DataManager.Instance.SuffleDeckList();
        // �������� �̹��� ���;� ��
        DataManager.Instance.Monsters = mimicMonster;
        SceneFader.instance.LoadSceneWithFade(3);
    }

    public void GetCoin()
    {
        SettingManager.Instance.PlaySound(CoinClip);

        int randomCoin = Random.Range(30, 41);
        DataManager.Instance.currentCoin += randomCoin;
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        // �� �޼��尡 ȣ��� ��ư�� �ִ� ������Ʈ�� ���ŵ�
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
            float duration = 0.5f; // ���̵� �ƿ� �ð�
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

        // ���� ���� �� ���
        randomCoin = Random.Range(50, 61);

        // ���� ���� ������ üũ�ϰ� �ؽ�Ʈ �� ����
        bool insufficientCoins = DataManager.Instance.currentCoin < randomCoin;
        string mainTextColor = insufficientCoins ? "#808080" : "#FFFFFF";
        string coinTextColor = insufficientCoins ? "#808080" : "#FFFF00";
        string cardTextColor = insufficientCoins ? "#808080" : "#ADD8E6";

        // �ؽ�Ʈ ����
        randomCardCoinText.text = $"<color={mainTextColor}>1. <color={coinTextColor}>{randomCoin}����</color>�� �����ϰ� <color={cardTextColor}>������ ī��</color>�� �޴´�.</color>";

        yield return StartCoroutine(DecreaseAlpha());
    }

    // randomCardList���� ī�带 1�� �������� �� ���� �߰�
    public void GetRandomCard()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        if (DataManager.Instance.currentCoin < randomCoin)
        {
            Debug.Log("������ �����մϴ�.");
            return; // ������ ������ �� �ƹ� �͵� ���� �ʰ� �޼��� ����
        }

        DataManager.Instance.currentCoin -= randomCoin;
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        int randomIndex = Random.Range(0, randomCardList.Count);
        CardBasic selectedCard = randomCardList[randomIndex];

        DataManager.Instance.deckList.Add(selectedCard);

        randomCardEventDescription.text = $"������ �� �� ���� ǥ���� ������ ī�带 �ǳ��ݴϴ�. \n" +
            $"�� ī��� ��ſ��� ������ �� ���Դϴ�. \n" +
            $"<color=#8A2BE2>{selectedCard.cardName}</color>ī�带 �޾ҽ��ϴ�.";

        closeRandomCardEventText.text = "������ ��� �����Ѵ�.";

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

        // ���� ���� �� ���
        randomCoin = Random.Range(30, 41);

        // ���� ���� ������ üũ�ϰ� �ؽ�Ʈ �� ����
        bool insufficientCoins = DataManager.Instance.currentCoin < randomCoin;
        string mainTextColor = insufficientCoins ? "#808080" : "#FFFFFF";
        string coinTextColor = insufficientCoins ? "#808080" : "#FFFF00";
        string cardTextColor = insufficientCoins ? "#808080" : "#red";

        // �ؽ�Ʈ ����
        healCoinText.text = $"<color={mainTextColor}>1. <color={coinTextColor}>{randomCoin}����</color>�� �����ϰ� <color={cardTextColor}>ü�� 20%</color>�� ȸ���Ѵ�.</color>";

        yield return StartCoroutine(DecreaseAlpha());
    }

    public void HealAndUseCoin()
    {
        SettingManager.Instance.PlaySound(SettingManager.Instance.BtnClip1);

        if (DataManager.Instance.currentCoin < randomCoin)
        {
            Debug.Log("������ �����մϴ�.");
            return; // ������ ������ �� �ƹ� �͵� ���� �ʰ� �޼��� ����
        }

        DataManager.Instance.currentCoin -= randomCoin;
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        // ü�� ȸ�� (�ִ� ü���� 20%��ŭ)
        int healAmount = Mathf.CeilToInt(DataManager.Instance.maxHealth * 0.2f); // 20% ȸ�� (�ݿø�)
        DataManager.Instance.currenthealth = Mathf.Min(DataManager.Instance.currenthealth + healAmount, DataManager.Instance.maxHealth); // �ִ� ü���� ���� �ʵ��� ����

        DungeonManager.Instance.currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";
        healEventDescription.text = "���ʻ����� ������ �� �տ� ���� ���� ��� �ڶ������� ���մϴ�. \n" +
            "�� ���ʴ� �ڿ��� ������ ��� ü���� ������ ȸ�������ݴϴ�. \n" +
            "���� �ż��ϰ� ȿ������ �����Ƿ� �������!";

        closeHealEventText.text = "������ ��� �����Ѵ�.";

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

    // �����г� ��Ȱ��ȭ
    public void HideDungeon()
    {
        Dungeon.SetActive(false);
    }

    public void ShowDungeon()
    {
        Dungeon.SetActive(true);
    }

    // ���İ��� õõ�� �ø��� �޼���
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

    // ���İ��� õõ�� ���̴� �޼���
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
