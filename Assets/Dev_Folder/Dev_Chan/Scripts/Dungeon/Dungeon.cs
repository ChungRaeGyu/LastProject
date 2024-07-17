using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon : MonoBehaviour
{
    public static Dungeon Instance = null;

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


    public GameObject[] stageNum;
    public GameObject[] stageBtn;

    public int num;

    void Start()
    {
        for(int i = 0; i<stageNum.Length; i++)
        {
            stageNum[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stageBtn.Length; i++)
        {
            stageBtn[i].SetActive(false);
        }
    }
}
