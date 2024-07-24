using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance = null;    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public GameObject battleScene;
    public GameObject eventScene;
    public GameObject storeScene;
    
    public void Start()
    {
        battleScene.SetActive(false);
        eventScene.SetActive(false);
        storeScene.SetActive(false);
    }
}
