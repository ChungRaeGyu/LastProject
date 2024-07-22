using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBoardManager : MonoBehaviour
{
    //�ν��Ͻ�
    public static DungeonBoardManager Instance = null;

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

    public GameObject dungeonBoard;
    public GameObject dungeon;

    public GameObject[] dungeonEntrance = new GameObject[5];

    public GameObject homeButton;

    public int accessDungeon = 1;

    void Start()
    {
        homeButton.SetActive(true);

        //������ �������� ��
        if (SaveManager.Instance.accessDungeon == true)
        {
            dungeonBoard.SetActive(false); //���� ���� ��Ȱ��ȭ
            dungeon.SetActive(true); //���� Ȱ��ȭ
        }

        //������ �������� �ʾ�����
        else
        {
            dungeonBoard.SetActive(true); //���� ���� Ȱ��ȭ
            dungeon.SetActive(false); //���� ���� ��Ȱ��ȭ
        }
    }

}
