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

    public GameObject dungeonBoard;
    public GameObject dungeon;

    public GameObject[] dungeonEntrance = new GameObject[5];

    public GameObject homeButton;
    public GameObject backButton;

    public int accessDungeon = 1;

    void Start()
    {
        

    }

    void Update()
    {
        //던전에 입장했을 때
        if (SaveManager.Instance.accessDungeon == true)
        {
            dungeonBoard.SetActive(false); //던전 보드 비활성화
            dungeon.SetActive(true); //던전 활성화
            backButton.SetActive(false);
            homeButton.SetActive(false);

            for (int i = 0; i < dungeonEntrance.Length; i++)
            {
                dungeonEntrance[i].SetActive(false);
            }
        }

        //던전에 입장하지 않았을때
        if (SaveManager.Instance.accessDungeon == false)
        {
            dungeonBoard.SetActive(true); //던전 보드 활성화
            dungeon.SetActive(false); //던전 보드 비활성화
            backButton.SetActive(true);
            homeButton.SetActive(true);

            for (int i = 0; i < dungeonEntrance.Length; i++)
            {
                dungeonEntrance[i].SetActive(true);
            }
        }
    }
}
