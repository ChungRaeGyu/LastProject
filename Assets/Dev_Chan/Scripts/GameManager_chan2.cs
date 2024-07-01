using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_chan2 : MonoBehaviour
{
    public static GameManager_chan2 Instance = null;

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

    public int stageLevel = 1;

    public void Start()
    {

    }
}
