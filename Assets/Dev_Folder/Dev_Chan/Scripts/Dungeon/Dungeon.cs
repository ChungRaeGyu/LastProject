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

    //보스 클리어 후 로비로 가는 버튼
    public void BossClear()
    {
        SaveManager.Instance.accessDungeon = false;
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Lobby");

        switch (SaveManager.Instance.accessDungeonNum)
        {
            case 1:
                //2던전 개방
                SaveManager.Instance.accessibleDungeon[1] = true;
                Debug.Log("1번째 던전을 클리어하셨습니다.");
                break;

            case 2:
                //3던전 개방
                SaveManager.Instance.accessibleDungeon[2] = true;
                Debug.Log("2번째 던전을 클리어하셨습니다.");
                break;

            case 3:
                //4던전 개방
                SaveManager.Instance.accessibleDungeon[3] = true;
                Debug.Log("3번째 던전을 클리어하셨습니다.");
                break;

            case 4:
                //5던전 개방
                SaveManager.Instance.accessibleDungeon[4] = true;
                Debug.Log("4번째 던전을 클리어하셨습니다.");
                break;

            case 5:
                //다음 던전 개방 미정
                Debug.Log("5번째 던전을 클리어하셨습니다.");
                break;
        }

    }
}
