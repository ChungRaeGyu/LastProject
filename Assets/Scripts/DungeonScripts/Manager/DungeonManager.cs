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

    public Vector3 stage01;

    [Header("TextUI")]
    public TMP_Text currentCoinText;
    public TMP_Text currentHpText;

    [Header("Info")]
    public GameObject DungeonCoin;
    public GameObject DungeonHp;

    public Player Player;

    private void Start()
    {
        eventScene.SetActive(false);

        currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";
    }

    private void Update()
    {
        if (SaveManager.Instance.accessDungeon == true)
        {
            player.SetActive(true);
            int num = SaveManager.Instance.accessDungeonNum;
            dungeonNum[num].SetActive(true);
            DungeonCoin.SetActive(true);
            DungeonHp.SetActive(true);

            stage.SetActive(true);
        }
        if (SaveManager.Instance.accessDungeon == false)
        {
            player.SetActive(false);
            DungeonCoin.SetActive(false);
            DungeonHp.SetActive(false);

            stage.SetActive(false);
        }
        player.transform.position = SaveManager.Instance.playerPosition;
    }
}
