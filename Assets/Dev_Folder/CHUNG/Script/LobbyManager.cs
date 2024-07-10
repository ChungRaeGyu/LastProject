using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager instance;
    // Start is called before the first frame update
    public List<GameObject> pages;
    public GameObject num;
    public event Action OnCount;
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

    private void Init()
    {
        int j = 0;
        for (int i = 0; i < DataManager.Instance.cardObjs.Count; i++)
        {
            if (i!=0&&i % 9 == 0) j++;
            GameObject temp = Instantiate(DataManager.Instance.cardObjs[i].gameObject, pages[j].transform);
            Instantiate(num, temp.transform);
        }
    }

    public void InvokeCount()
    {
        Debug.Log("����GameManager");
        OnCount?.Invoke();
    }

}
