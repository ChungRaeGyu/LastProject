using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    public GameObject player;
    public GameObject stage;
    public GameObject eventScene;
    public GameObject storeScene;

    public Vector3 stage01;

    [Header("TextUI")]
    public TMP_Text currentCoinText;

    private void Start()
    {
        eventScene.SetActive(false);
        storeScene.SetActive(false);

        currentCoinText.text = DataManager.Instance.currentCoin.ToString();
    }

    private void Update()
    {
        if (SaveManager.Instance.accessDungeon == true)
        {
            player.SetActive(true);
            int num = SaveManager.Instance.accessDungeonNum;
            dungeonNum[num].SetActive(true);

            stage.SetActive(true);
        }
        if (SaveManager.Instance.accessDungeon == false)
        {
            player.SetActive(false);

            stage.SetActive(false);
        }
        player.transform.position = SaveManager.Instance.playerPosition;
    }
}
