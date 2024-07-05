using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance = null;

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

    public GameObject[] dungeonNum = new GameObject[5];

    public GameObject homeButton;
    public GameObject backButton;

    private void Start()
    {
        backButton.SetActive(true);
        homeButton.SetActive(true);
    }

    private void Update()
    {
        
    }
}
