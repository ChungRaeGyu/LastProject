using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBoardManager : MonoBehaviour
{
    //인스턴스
    public static DungeonBoardManager Instance = null;

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


    public GameObject[] dungeonEntrance = new GameObject[5];

    public GameObject homeButton;
    public GameObject backButton;



    void Start()
    {
        DungeonManager.Instance.dungeonBoard.SetActive(true);
        DungeonManager.Instance.dungeon.SetActive(false);
        backButton.SetActive(true);
        homeButton.SetActive(true);

        for(int i = 0; i<dungeonEntrance.Length; i++) 
        {
            dungeonEntrance[i].SetActive(true);
        }

    }

    void Update()
    {
        
    }
}
