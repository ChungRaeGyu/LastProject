using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public GameObject Dungeon;

    [Header("Event")]
    public GameObject mimicEvent;
    public GameObject randomCardEvent;
    public GameObject healEvent;

    [Header("MimicEvent")]
    public List<GameObject> boxes;

    [Header("RandomCardEvent")]
    public List<CardBasic> randomCardList;

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
        LoadingSceneManager.LoadScene(3);
    }

    public void GetCoin()
    {
        int randomCoin = Random.Range(20, 41);
        DataManager.Instance.currentCoin += randomCoin;

        // �� �޼��尡 ȣ��� ��ư ������Ʈ�� ���ŵ�
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null)
        {
            Destroy(clickedButton);
        }
    }

    public void HideMimicEvent()
    {
        mimicEvent.SetActive(false);
        ShowDungeon();
    }

    public void ShowRandomCardEvent()
    {
        HideDungeon();
        randomCardEvent.SetActive(true);
    }

    public void HideRandomCardEvent()
    {
        randomCardEvent.SetActive(false);
        ShowDungeon();
    }

    public void HideDungeon()
    {
        Dungeon.SetActive(false);
    }

    public void ShowDungeon()
    {
        Dungeon.SetActive(true);
    }

    // randomCardList���� ī�带 1�� �������� ��ȯ
    public CardBasic GetRandomCard()
    {
        if (randomCardList == null || randomCardList.Count == 0)
        {
            Debug.Log("���� ī�� ����Ʈ�� ����ֽ��ϴ�.");
            return null;
        }

        int randomIndex = Random.Range(0, randomCardList.Count);
        return randomCardList[randomIndex];
    }
}
