using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager instance;
    // Start is called before the first frame update
    public List<GameObject> pages;
    public GameObject num;
    public DeckControl deckControl;
    public GameObject deckContent;
    [Header("Canvas")]
    public GameObject deckCanvas;
    public GameObject BookCanvas;
    [Header("InputScript")]
    public GameObject currentCanvas;
    public event Action OnCount;

    public bool isDrawing = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        Init();
    }

    // 도감에 카드를 생성하는 메서드
    private void Init()
    {
        int j = 0;
        for (int i = 0; i < DataManager.Instance.cardObjs.Count; i++)
        {
            if (i!=0&&i % 9 == 0) j++;
            GameObject temp = Instantiate(DataManager.Instance.cardObjs[i].gameObject, pages[j].transform);
            temp.GetComponent<CardBasic>().cardBasic = DataManager.Instance.cardObjs[i];
            Instantiate(num, temp.transform);
        }
    }

    public void InvokeCount()
    {
        OnCount?.Invoke();
    }

}
