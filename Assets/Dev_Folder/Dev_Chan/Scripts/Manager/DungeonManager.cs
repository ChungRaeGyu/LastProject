using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public GameObject dungeonBoard;
    public GameObject dungeon;

    public GameObject[] dungeonNum = new GameObject[5];

    public GameObject homeButton;
    public GameObject backButton;
    public GameObject player;
    public GameObject stage;
    public GameObject battleScene;
    public GameObject eventScene;
    public GameObject storeScene;
    public GameObject bossScene;

    public Vector3 playerPosition;
    public Vector3 stage01;

    private void Start()
    {
        backButton.SetActive(true);
        homeButton.SetActive(true);
        player.SetActive(true);

        stage.SetActive(true);
        battleScene.SetActive(false);
        eventScene.SetActive(false);
        storeScene.SetActive(false);
        bossScene.SetActive(false);
    }

    private void Update()
    {
        player.transform.position = playerPosition;
    }
}
