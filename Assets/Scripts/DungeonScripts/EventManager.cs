using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class EventManager : MonoBehaviour
{
    public GameObject Dungeon;

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

    [Header("HealEvent")]
    public TMP_Text healEventDescription;
    public TMP_Text healCoinText;
    public GameObject healEventSelectBtn;
    public TMP_Text closeHealEventText;

    // ���� �� ���� ��������
    private int randomCoin;

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
        }
    }

    public void ShowMimicEvent()
    {
        HideDungeon();
        ShuffleBoxes();
        mimicEvent.SetActive(true);
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
        // �������� �̹��� ���;� ��
        DataManager.Instance.Monsters = mimicMonster;
        LoadingSceneManager.LoadScene(3);
    }

    public void GetCoin()
    {
        int randomCoin = Random.Range(20, 41);
        DataManager.Instance.currentCoin += randomCoin;
        DungeonManager.Instance.currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        // �� �޼��尡 ȣ��� ��ư�� �ִ� ������Ʈ�� ���ŵ�
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null)
            Destroy(clickedButton);
    }

    public void HideMimicEvent()
    {
        mimicEvent.SetActive(false);
        ShowDungeon();
        LoadingSceneManager.LoadScene(2);
    }

    public void ShowRandomCardEvent()
    {
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
    }

    // randomCardList���� ī�带 1�� �������� �� ���� �߰�
    public void GetRandomCard()
    {
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
            $"<color=#8A2BE2>{selectedCard.name}</color>ī�带 �޾ҽ��ϴ�.";

        closeRandomCardEventText.text = "������ ��� �����Ѵ�.";

        randomCardEventSelectBtn.SetActive(false);
    }

    public void HideRandomCardEvent()
    {
        randomCardEvent.SetActive(false);
        ShowDungeon();
        LoadingSceneManager.LoadScene(2);
    }

    public void ShowHealEvent()
    {
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
        randomCardCoinText.text = $"<color={mainTextColor}>1. <color={coinTextColor}>{randomCoin}����</color>�� �����ϰ� <color={cardTextColor}>ü�� 20%</color>�� ȸ���Ѵ�.</color>";
    }

    public void HealAndUseCoin()
    {
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
    }

    public void HideHealEvent()
    {
        healEvent.SetActive(false);
        ShowDungeon();
        LoadingSceneManager.LoadScene(2);
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
}
