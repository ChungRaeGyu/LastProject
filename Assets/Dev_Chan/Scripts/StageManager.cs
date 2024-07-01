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

    public GameObject[] stage = new GameObject[6];
    public GameObject[] stageBtn = new GameObject[6];
    public GameObject clear;
    public GameObject player;

    public GameManager_chan gm;


    public void Start()
    {
        player.transform.position = new Vector3(-6, 1, 0);

        stageBtn[0].SetActive(true);
        for (int i = 1; i < stageBtn.Length - 1; i++)
        {
            stageBtn[i].SetActive(false);
        }
    }

    public void FixedUpdate()
    {

    }
}
