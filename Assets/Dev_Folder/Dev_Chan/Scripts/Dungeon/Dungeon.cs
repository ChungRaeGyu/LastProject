using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon : MonoBehaviour
{

    public GameObject[] stageNum;
    public GameObject[] stageBtn;

    public int num;

    void Start()
    {
        for(int i = 0; i<stageNum.Length; i++)
        {
            stageNum[i].SetActive(true);
        }
        SaveManager.Instance.playerPosition = stageNum[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stageBtn.Length; i++)
        {
            stageBtn[i].SetActive(false);
        }
    }

    //���� Ŭ���� �� �κ�� ���� ��ư
    public void BossClear()
    {
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Lobby");

        switch (SaveManager.Instance.accessDungeonNum)
        {
            case 1:
                //2���� ����
                SaveManager.Instance.accessibleDungeon[1] = true;
                Debug.Log("1��° ������ Ŭ�����ϼ̽��ϴ�.");
                break;

            case 2:
                //3���� ����
                SaveManager.Instance.accessibleDungeon[2] = true;
                Debug.Log("2��° ������ Ŭ�����ϼ̽��ϴ�.");
                break;

            case 3:
                //4���� ����
                SaveManager.Instance.accessibleDungeon[3] = true;
                Debug.Log("3��° ������ Ŭ�����ϼ̽��ϴ�.");
                break;

            case 4:
                //5���� ����
                SaveManager.Instance.accessibleDungeon[4] = true;
                Debug.Log("4��° ������ Ŭ�����ϼ̽��ϴ�.");
                break;

            case 5:
                //���� ���� ���� ����
                Debug.Log("5��° ������ Ŭ�����ϼ̽��ϴ�.");
                break;
        }

    }
}
