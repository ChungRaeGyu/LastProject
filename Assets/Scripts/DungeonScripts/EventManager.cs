using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public GameObject Dungeon;

    [Header("Event")]
    public GameObject mimicEvent;
    public GameObject RandomCardEvent;

    [Header("MimicEvent")]
    public GameObject Box1;
    public GameObject Box2;
    public GameObject Box3;

    [Header("RandomCardEvent")]
    public GameObject aaaaa;

    public void ShowMimicEvent()
    {
        HideDungeon();
        ShuffleBoxes();
        mimicEvent.SetActive(true);
    }

    private void ShuffleBoxes()
    {
        List<Transform> boxPositions = new List<Transform> { Box1.transform, Box2.transform, Box3.transform };

        for (int i = boxPositions.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // 위치를 스왑
            Vector3 tempPosition = boxPositions[i].position;
            boxPositions[i].position = boxPositions[randomIndex].position;
            boxPositions[randomIndex].position = tempPosition;
        }
    }

    public void MimicSurprise()
    {
        // 전투에서 미믹이 나와야 함
        LoadingSceneManager.LoadScene(3);
    }

    public void GetCoin()
    {
        int randomCoin = Random.Range(20, 41);
        DataManager.Instance.currentCoin += randomCoin;
    }

    public void HideMimicEvent()
    {
        mimicEvent.SetActive(false);
        ShowDungeon();
    }

    public void ShowRandomCardEvent()
    {
        HideDungeon();
        RandomCardEvent.SetActive(true);
    }

    public void HideRandomCardEvent()
    {
        RandomCardEvent.SetActive(false);
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
}
