using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_chan : MonoBehaviour
{
    public static GameManager_chan Instance = null;

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
    public bool[] clearCheck = new bool[6];

    public void Start()
    {

    }
}
