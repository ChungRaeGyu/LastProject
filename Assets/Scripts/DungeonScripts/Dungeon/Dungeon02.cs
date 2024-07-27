using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon02 : MonoBehaviour
{
    public static Dungeon02 Instance = null;

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

    public GameObject[] stageNum;
    public GameObject[] stageBtn;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<stageNum.Length; i++)
        {
            stageNum[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stageBtn.Length; i++)
        {
            stageBtn[i].SetActive(false);
        }
        if (SaveManager.Instance.playerPosition == stageNum[0].transform.position)
        {
            stageBtn[0].SetActive(true);
            stageBtn[1].SetActive(true);
            stageBtn[2].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[1].transform.position)
        {
            stageBtn[1].SetActive(true);
            stageBtn[3].SetActive(true);
            stageBtn[4].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[2].transform.position)
        {
            stageBtn[4].SetActive(true);
            stageBtn[5].SetActive(true);
            stageBtn[6].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[3].transform.position)
        {
            stageBtn[1].SetActive(true);
            stageBtn[6].SetActive(true);
            stageBtn[7].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[4].transform.position)
        {
            stageBtn[4].SetActive(true);
            stageBtn[8].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[5].transform.position)
        {
            stageBtn[5].SetActive(true);
            stageBtn[8].SetActive(true);
            stageBtn[9].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[6].transform.position)
        {
            stageBtn[9].SetActive(true);
            stageBtn[10].SetActive(true);
            stageBtn[11].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[7].transform.position)
        {
            stageBtn[5].SetActive(true);
            stageBtn[11].SetActive(true);
            stageBtn[12].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[8].transform.position)
        {
            stageBtn[6].SetActive(true);
            stageBtn[12].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[9].transform.position)
        {
            stageBtn[9].SetActive(true);
            stageBtn[13].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[10].transform.position)
        {
            stageBtn[10].SetActive(true);
            stageBtn[13].SetActive(true);
            stageBtn[14].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[11].transform.position)
        {
            stageBtn[14].SetActive(true);
            stageBtn[15].SetActive(true);
            stageBtn[16].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[12].transform.position)
        {
            stageBtn[10].SetActive(true);
            stageBtn[16].SetActive(true);
            stageBtn[17].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[13].transform.position)
        {
            stageBtn[11].SetActive(true);
            stageBtn[17].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[14].transform.position)
        {
            stageBtn[14].SetActive(true);
            stageBtn[18].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[15].transform.position)
        {
            stageBtn[15].SetActive(true);
            stageBtn[18].SetActive(true);
            stageBtn[19].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[16].transform.position)
        {
            stageBtn[19].SetActive(true);
            stageBtn[20].SetActive(true);
            stageBtn[21].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[17].transform.position)
        {
            stageBtn[15].SetActive(true);
            stageBtn[21].SetActive(true);
            stageBtn[22].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[18].transform.position)
        {
            stageBtn[16].SetActive(true);
            stageBtn[22].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[19].transform.position)
        {
            stageBtn[19].SetActive(true);
            stageBtn[23].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[20].transform.position)
        {
            stageBtn[20].SetActive(true);
            stageBtn[23].SetActive(true);
            stageBtn[24].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[21].transform.position)
        {
            stageBtn[24].SetActive(true);
            stageBtn[25].SetActive(true);
            stageBtn[26].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[22].transform.position)
        {
            stageBtn[20].SetActive(true);
            stageBtn[26].SetActive(true);
            stageBtn[27].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[23].transform.position)
        {
            stageBtn[21].SetActive(true);
            stageBtn[27].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[24].transform.position)
        {
            stageBtn[24].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[25].transform.position)
        {
            stageBtn[25].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[26].transform.position)
        {
            //clear
        }
        if (SaveManager.Instance.playerPosition == stageNum[27].transform.position)
        {
            stageBtn[25].SetActive(true);
        }
        if (SaveManager.Instance.playerPosition == stageNum[28].transform.position)
        {
            stageBtn[26].SetActive(true);
        }
    }
    public void Warp1()
    {
        SaveManager.Instance.playerPosition = stageNum[18].transform.position;
    }

    public void Warp2()
    {
        SaveManager.Instance.playerPosition = stageNum[14].transform.position;
    }

}
