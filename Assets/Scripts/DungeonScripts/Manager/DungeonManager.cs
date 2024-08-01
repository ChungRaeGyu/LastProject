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

    public Transform startPosition;

    private void Start()
    {
        eventScene.SetActive(false);

        currentCoinText.text = DataManager.Instance.currentCoin.ToString();

        currentHpText.text = $"{DataManager.Instance.currenthealth} / {DataManager.Instance.maxHealth}";

        if (!SaveManager.Instance.isStartPoint)
            player.transform.position = SaveManager.Instance.playerPosition; // 이렇게 하면 현재 보는 화면의 좌표를 기준으로 플레이어가 이동된다.
        // 방금 클리어 및 눌렀던 스테이지의 위치에 이동시켜줘야한다.
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
    }
}
